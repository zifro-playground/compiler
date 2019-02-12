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
            IParseTree child = context.GetChild(0);

            if (child is Python3Parser.Simple_stmtContext simple)
                return VisitSimple_stmt(simple);

            if (child is Python3Parser.Compound_stmtContext compound)
                return VisitCompound_stmt(compound);

            throw new SyntaxException(context.GetSourceReference(),
                nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                Localized_Python3_Parser.Ex_Syntax_ExpectedChild,
                Python3Parser.ruleNames[context.RuleIndex]);
        }

        public override SyntaxNode VisitSimple_stmt(Python3Parser.Simple_stmtContext context)
        {
            // simple_stmt: small_stmt (';' small_stmt)* [';'] NEWLINE

            // This ignores newlines and semicolons atm
            List<Statement> statements = context.GetChildren()
                .OfType<Python3Parser.Small_stmtContext>()
                .Select(child => (Statement) VisitSmall_stmt(child))
                .ToList();

            switch (statements.Count)
            {
                case 0:
                    throw new SyntaxException(context.GetSourceReference(),
                        nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                        Localized_Python3_Parser.Ex_Syntax_ExpectedChild,
                        Python3Parser.ruleNames[context.RuleIndex]);
                case 1:
                    return statements[0];

                default:
                    return new StatementList(context.GetSourceReference(), statements);
            }
        }

        public override SyntaxNode VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            // small_stmt: (expr_stmt | del_stmt | pass_stmt | flow_stmt |
            //    import_stmt | global_stmt | nonlocal_stmt | assert_stmt)
            var child = context.GetChild(0) as ParserRuleContext;

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

                case null:
                    throw new SyntaxException(context.GetSourceReference(),
                        nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                        Localized_Python3_Parser.Ex_Syntax_ExpectedChild,
                        Python3Parser.ruleNames[context.RuleIndex]);

                default:
                    throw new SyntaxException(context.GetSourceReference(),
                        nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                        Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType,
                        Python3Parser.ruleNames[context.RuleIndex],
                        Python3Parser.ruleNames[child.RuleIndex]);
            }
        }

        public override SyntaxNode VisitCompound_stmt(Python3Parser.Compound_stmtContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }
    }
}