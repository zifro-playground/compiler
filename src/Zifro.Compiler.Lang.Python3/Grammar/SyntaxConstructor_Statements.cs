using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitStmt(Python3Parser.StmtContext context)
        {
            IParseTree child = context.GetChild(0);

            if (child is Python3Parser.Simple_stmtContext simple)
                return VisitSimple_stmt(simple);

            if (child is Python3Parser.Compound_stmtContext compound)
                return VisitCompound_stmt(compound);

            throw new SyntaxException(context.GetSourceReference(),
                localizeKey: nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                localizedMessageFormat: Localized_Python3_Parser.Ex_Syntax_ExpectedChild);
        }

        public override SyntaxNode VisitSimple_stmt(Python3Parser.Simple_stmtContext context)
        {
            return VisitChildren(context);
        }

        public override SyntaxNode VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            IParseTree child = context.GetChild(0);

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
                    throw new SyntaxNotYetImplementedException(context.GetSourceReference());

                case ParserRuleContext childRule:
                    throw new SyntaxException(context.GetSourceReference(),
                        nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                        Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType,
                        Python3Parser.ruleNames[context.RuleIndex],
                        Python3Parser.ruleNames[childRule.RuleIndex]);

                case null:
                default:
                    throw new SyntaxException(context.GetSourceReference(),
                        localizeKey: nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                        localizedMessageFormat: Localized_Python3_Parser.Ex_Syntax_ExpectedChild);
            }
        }

        public override SyntaxNode VisitCompound_stmt(Python3Parser.Compound_stmtContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }
    }
}