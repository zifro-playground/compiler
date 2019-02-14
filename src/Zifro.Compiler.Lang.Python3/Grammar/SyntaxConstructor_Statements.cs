using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitStmt(Python3Parser.StmtContext context)
        {
            // stmt: simple_stmt | compound_stmt
            var child = context.GetChildOrThrow<ParserRuleContext>(0);

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
                    continue;

                if (!(ruleContext is Python3Parser.Small_stmtContext smallStmt))
                    throw context.UnexpectedChildType(ruleContext);

                allRules.Add(smallStmt);
            }

            if (allRules.Count == 0)
                throw context.ExpectedChild();

            var firstRule = allRules[0];
            var firstStmt = VisitSmall_stmt(firstRule)
                .AsTypeOrThrow<Statement>();

            if (allRules.Count == 1)
                return firstStmt;

            // but wait, there's more!
            var statements = new Statement[allRules.Count];
            statements[0] = firstStmt;

            for (var i = 1; i < allRules.Count; i++)
            {
                var rule = allRules[i];
                var stmt = VisitSmall_stmt(rule).AsTypeOrThrow<Statement>();

                statements[i] = stmt;
            }

            return new StatementList(context.GetSourceReference(), statements);
        }

        public override SyntaxNode VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            // small_stmt: (expr_stmt | del_stmt | pass_stmt | flow_stmt |
            //    import_stmt | global_stmt | nonlocal_stmt | assert_stmt)
            var child = context.GetChildOrThrow<ParserRuleContext>(0);

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
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }
    }
}