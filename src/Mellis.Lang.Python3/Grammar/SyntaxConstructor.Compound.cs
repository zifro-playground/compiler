using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
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
            Python3Parser.TestContext testRule = context.GetChildOrThrow<Python3Parser.TestContext>(1);
            context.GetChildOrThrow(2, Python3Parser.COLON)
                .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_If_MissingColon));
            Python3Parser.SuiteContext suiteRule = context.GetChildOrThrow<Python3Parser.SuiteContext>(3);

            ExpressionNode testExpr = VisitTest(testRule)
                .AsTypeOrThrow<ExpressionNode>();
            Statement suiteStmt = VisitSuite(suiteRule)
                .AsTypeOrThrow<Statement>();

            Statement elseStmt = null;

            var elIfStatements = new List<(
                SourceReference source,
                ExpressionNode testExpr,
                Statement suite
                )>();

            // note: increment by 4
            for (int i = 4; i < context.ChildCount; i += 4)
            {
                ITerminalNode elseOrElif = context.GetChildOrThrow<ITerminalNode>(i);
                switch (elseOrElif.Symbol.Type)
                {
                case Python3Parser.ELSE when elseStmt != null:
                case Python3Parser.ELIF when elseStmt != null:
                    throw context.UnexpectedChildType(elseOrElif);

                case Python3Parser.ELIF:
                    Python3Parser.TestContext elifTestRule = context.GetChildOrThrow<Python3Parser.TestContext>(i + 1);

                    context.GetChildOrThrow(i + 2, Python3Parser.COLON)
                        .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_If_Elif_MissingColon));

                    Python3Parser.SuiteContext elifSuiteRule =
                        context.GetChildOrThrow<Python3Parser.SuiteContext>(i + 3);

                    ExpressionNode elifExpr = VisitTest(elifTestRule)
                        .AsTypeOrThrow<ExpressionNode>();
                    Statement elifStmt = VisitSuite(elifSuiteRule)
                        .AsTypeOrThrow<Statement>();

                    var source = SourceReference.Merge(
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

                    Python3Parser.SuiteContext elseRule = context.GetChildOrThrow<Python3Parser.SuiteContext>(i + 2);
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
            // while_stmt: 'while' test ':' suite ['else' ':' suite]
            context.GetChildOrThrow(0, Python3Parser.WHILE);
            Python3Parser.TestContext testNode = context.GetChildOrThrow<Python3Parser.TestContext>(1);
            context.GetChildOrThrow(2, Python3Parser.COLON)
                .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_While_MissingColon));
            Python3Parser.SuiteContext suiteNode = context.GetChildOrThrow<Python3Parser.SuiteContext>(3);

            if (context.ChildCount > 4)
            {
                ITerminalNode elseTerm = context.GetChildOrThrow(4, Python3Parser.ELSE);
                context.GetChildOrThrow(5, Python3Parser.COLON)
                    .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_While_Else_MissingColon));
                context.GetChildOrThrow<Python3Parser.SuiteContext>(6);

                if (context.ChildCount > 7)
                {
                    throw context.UnexpectedChildType(context.GetChild(7));
                }

                throw elseTerm.NotYetImplementedException("while..else");
            }

            ExpressionNode testExpr = VisitTest(testNode)
                .AsTypeOrThrow<ExpressionNode>();

            Statement suiteStmt = VisitSuite(suiteNode)
                .AsTypeOrThrow<Statement>();

            return new WhileStatement(context.GetSourceReference(), testExpr, suiteStmt);
        }

        public override SyntaxNode VisitFor_stmt(Python3Parser.For_stmtContext context)
        {
            // for_stmt: 'for' exprlist 'in' testlist ':' suite ['else' ':' suite]
            context.GetChildOrThrow(0, Python3Parser.FOR);
            Python3Parser.ExprlistContext operandNode = context.GetChildOrThrow<Python3Parser.ExprlistContext>(1);
            context.GetChildOrThrow(2, Python3Parser.IN);
            Python3Parser.TestlistContext iterNode = context.GetChildOrThrow<Python3Parser.TestlistContext>(3);
            context.GetChildOrThrow(4, Python3Parser.COLON);
            Python3Parser.SuiteContext suiteNode = context.GetChildOrThrow<Python3Parser.SuiteContext>(5);

            if (context.ChildCount > 6)
            {
                ITerminalNode elseTerm = context.GetChildOrThrow(6, Python3Parser.ELSE);
                context.GetChildOrThrow(7, Python3Parser.COLON);
                context.GetChildOrThrow<Python3Parser.SuiteContext>(8);

                if (context.ChildCount > 9)
                {
                    throw context.UnexpectedChildType(context.GetChild(9));
                }

                throw elseTerm.NotYetImplementedException("for..else");
            }

            ExpressionNode operandExpr = VisitExprlist(operandNode)
                .AsTypeOrThrow<ExpressionNode>();
            ExpressionNode iterExpr = VisitTestlist(iterNode)
                .AsTypeOrThrow<ExpressionNode>();

            Statement suiteStmt = VisitSuite(suiteNode)
                .AsTypeOrThrow<Statement>();

            return new ForStatement(context.GetSourceReference(), operandExpr, iterExpr, suiteStmt);
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