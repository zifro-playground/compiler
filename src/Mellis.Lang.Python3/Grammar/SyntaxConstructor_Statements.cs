using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitSuite(Python3Parser.SuiteContext context)
        {
            // suite: simple_stmt | NEWLINE INDENT stmt+ DEDENT
            IParseTree first = context.GetChildOrThrow<IParseTree>(0);

            switch (first)
            {
                case Python3Parser.Simple_stmtContext _
                    when context.ChildCount > 1:
                    throw context.UnexpectedChildType(context.GetChild(1));

                case Python3Parser.Simple_stmtContext simple:
                    return VisitSimple_stmt(simple);

                case ITerminalNode term
                    when term.Symbol.Type != Python3Parser.NEWLINE:
                    throw context.UnexpectedChildType(term);

                case ITerminalNode _:
                    context.GetChildOrThrow(1, Python3Parser.INDENT);
                    context.GetChildOrThrow(context.ChildCount - 1, Python3Parser.DEDENT);

                    var statements = new List<Statement>();
                    for (int i = 2; i < context.ChildCount - 1; i++)
                    {
                    Python3Parser.StmtContext rule = context.GetChildOrThrow<Python3Parser.StmtContext>(i);
                    Statement stmt = VisitStmt(rule).AsTypeOrThrow<Statement>();

                        if (stmt is StatementList list)
                        {
                            statements.AddRange(list.Statements);
                        }
                        else
                        {
                            statements.Add(stmt);
                        }
                    }

                    if (statements.Count == 0)
                    {
                        throw context.ExpectedChild();
                    }

                    if (statements.Count == 1)
                    {
                        return statements[0];
                    }

                    return new StatementList(context.GetSourceReference(), statements);

                default:
                    throw context.UnexpectedChildType(first);
            }
        }

        public override SyntaxNode VisitStmt(Python3Parser.StmtContext context)
        {
            // stmt: simple_stmt | compound_stmt
            ParserRuleContext child = context.GetChildOrThrow<ParserRuleContext>(0);

            switch (child)
            {
                case Python3Parser.Simple_stmtContext simple:
                    return VisitSimple_stmt(simple);

                case Python3Parser.Compound_stmtContext compound:
                    return VisitCompound_stmt(compound);

                default:
                    throw context.UnexpectedChildType(child);
            }
        }

        public override SyntaxNode VisitSimple_stmt(Python3Parser.Simple_stmtContext context)
        {
            // simple_stmt: small_stmt (';' small_stmt)* [';'] NEWLINE

            var allRules = new List<Python3Parser.Small_stmtContext>();
            foreach (IParseTree child in context.GetChildren())
            {
                // This ignores newlines and semicolons atm
                if (!(child is ParserRuleContext ruleContext))
                {
                    continue;
                }

                if (!(ruleContext is Python3Parser.Small_stmtContext smallStmt))
                {
                    throw context.UnexpectedChildType(ruleContext);
                }

                allRules.Add(smallStmt);
            }

            if (allRules.Count == 0)
            {
                throw context.ExpectedChild();
            }

            Python3Parser.Small_stmtContext firstRule = allRules[0];
            Statement firstStmt = VisitSmall_stmt(firstRule)
                .AsTypeOrThrow<Statement>();

            if (allRules.Count == 1)
            {
                return firstStmt;
            }

            // but wait, there's more!
            var statements = new Statement[allRules.Count];
            statements[0] = firstStmt;

            for (int i = 1; i < allRules.Count; i++)
            {
                Python3Parser.Small_stmtContext rule = allRules[i];
                Statement stmt = VisitSmall_stmt(rule).AsTypeOrThrow<Statement>();

                statements[i] = stmt;
            }

            return new StatementList(context.GetSourceReference(), statements);
        }

        public override SyntaxNode VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            // small_stmt: (expr_stmt | del_stmt | pass_stmt | flow_stmt |
            //    import_stmt | global_stmt | nonlocal_stmt | assert_stmt)
            ParserRuleContext child = context.GetChildOrThrow<ParserRuleContext>(0);

            switch (child)
            {
                case Python3Parser.Expr_stmtContext expr:
                    return VisitExpr_stmt(expr);

                case Python3Parser.Assert_stmtContext _:
                case Python3Parser.Del_stmtContext _:
                case Python3Parser.Flow_stmtContext _:
                case Python3Parser.Global_stmtContext _:
                case Python3Parser.Import_stmtContext _:
                case Python3Parser.Nonlocal_stmtContext _:
                case Python3Parser.Pass_stmtContext _:
                    Visit(child);
                    throw child.NotYetImplementedException();

                default:
                    throw context.UnexpectedChildType(child);
            }
        }

        public override SyntaxNode VisitCompound_stmt(Python3Parser.Compound_stmtContext context)
        {
            // compound_stmt: if_stmt | while_stmt | for_stmt
            //    | try_stmt | with_stmt | funcdef | classdef
            //    | decorated | async_stmt

            ParserRuleContext first = context.GetChildOrThrow<ParserRuleContext>(0);

            if (context.ChildCount > 1)
            {
                throw context.UnexpectedChildType(context.GetChild(1));
            }

            switch (first)
            {
                case Python3Parser.If_stmtContext ifStmt:
                    return VisitIf_stmt(ifStmt);

                case Python3Parser.While_stmtContext whileStmt:
                    return VisitWhile_stmt(whileStmt);

                case Python3Parser.For_stmtContext forStmt:
                    return VisitFor_stmt(forStmt);

                case Python3Parser.Try_stmtContext tryStmt:
                    return VisitTry_stmt(tryStmt);

                case Python3Parser.With_stmtContext withStmt:
                    return VisitWith_stmt(withStmt);

                case Python3Parser.FuncdefContext funcDef:
                    return VisitFuncdef(funcDef);

                case Python3Parser.ClassdefContext classDef:
                    return VisitClassdef(classDef);

                case Python3Parser.DecoratedContext decorated:
                    return VisitDecorated(decorated);

                case Python3Parser.Async_stmtContext asyncStmt:
                    return VisitAsync_stmt(asyncStmt);

                default:
                    throw context.UnexpectedChildType(first);
            }
        }
    }
}