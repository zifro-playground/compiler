using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Statements;

namespace Mellis.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitAssert_stmt(Python3Parser.Assert_stmtContext context)
        {
            throw context.NotYetImplementedException("assert");
        }

        public override SyntaxNode VisitAsync_stmt(Python3Parser.Async_stmtContext context)
        {
            throw context.NotYetImplementedException("async");
        }

        public override SyntaxNode VisitIf_stmt(Python3Parser.If_stmtContext context)
        {
            // if_stmt: 'if' test ':' suite
            //      ('elif' test ':' suite)*
            //      ['else' ':' suite]
            context.GetChildOrThrow(0, Python3Parser.IF);
            var testRule = context.GetChildOrThrow<Python3Parser.TestContext>(1);
            context.GetChildOrThrow(2, Python3Parser.COLON)
                .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_If_MissingColon));
            var suiteRule = context.GetChildOrThrow<Python3Parser.SuiteContext>(3);

            var testExpr = VisitTest(testRule)
                .AsTypeOrThrow<ExpressionNode>();
            var suiteStmt = VisitSuite(suiteRule)
                .AsTypeOrThrow<Statement>();

            Statement elseStmt = null;

            var elIfStatements = new List<(
                SourceReference source,
                ExpressionNode testExpr,
                Statement suite
            )>();

            // note: increment by 4
            for (var i = 4; i < context.ChildCount; i += 4)
            {
                var elseOrElif = context.GetChildOrThrow<ITerminalNode>(i);
                switch (elseOrElif.Symbol.Type)
                {
                    case Python3Parser.ELSE when elseStmt != null:
                    case Python3Parser.ELIF when elseStmt != null:
                        throw context.UnexpectedChildType(elseOrElif);

                    case Python3Parser.ELIF:
                        var elifTestRule = context.GetChildOrThrow<Python3Parser.TestContext>(i + 1);

                        context.GetChildOrThrow(i + 2, Python3Parser.COLON)
                            .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_If_Elif_MissingColon));

                        var elifSuiteRule = context.GetChildOrThrow<Python3Parser.SuiteContext>(i + 3);

                        var elifExpr = VisitTest(elifTestRule)
                            .AsTypeOrThrow<ExpressionNode>();
                        var elifStmt = VisitSuite(elifSuiteRule)
                            .AsTypeOrThrow<Statement>();

                        SourceReference source = SourceReference.Merge(
                            elseOrElif.GetSourceReference(),
                            elifSuiteRule.GetSourceReference()
                        );

                        elIfStatements.Add((
                            source,
                            elifExpr,
                            elifStmt
                        ));
                        break;

                    case Python3Parser.ELSE:
                        context.GetChildOrThrow(i + 1, Python3Parser.COLON)
                            .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_If_Else_MissingColon));

                        var elseRule = context.GetChildOrThrow<Python3Parser.SuiteContext>(i + 2);
                        elseStmt = VisitSuite(elseRule)
                            .AsTypeOrThrow<Statement>();
                        break;

                    default:
                        throw context.UnexpectedChildType(elseOrElif);
                }
            }

            // Combine elif statements into the elseStmt var
            for (int i = elIfStatements.Count - 1; i >= 0; i--)
            {
                (SourceReference elifSource,
                    ExpressionNode elifCondition,
                    Statement elifSuite) = elIfStatements[i];

                elseStmt = new IfStatement(
                    source: elifSource,
                    condition: elifCondition,
                    ifSuite: elifSuite,
                    elseSuite: elseStmt
                );
            }

            return new IfStatement(context.GetSourceReference(),
                condition: testExpr,
                ifSuite: suiteStmt,
                elseSuite: elseStmt
            );
        }

        public override SyntaxNode VisitWhile_stmt(Python3Parser.While_stmtContext context)
        {
            throw context.NotYetImplementedException("while");
        }

        public override SyntaxNode VisitFor_stmt(Python3Parser.For_stmtContext context)
        {
            throw context.NotYetImplementedException("for");
        }

        public override SyntaxNode VisitTry_stmt(Python3Parser.Try_stmtContext context)
        {
            throw context.NotYetImplementedException("try");
        }

        public override SyntaxNode VisitWith_stmt(Python3Parser.With_stmtContext context)
        {
            throw context.NotYetImplementedException("with");
        }
    }
}