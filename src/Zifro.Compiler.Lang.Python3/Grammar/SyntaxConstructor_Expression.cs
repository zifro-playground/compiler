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
        public override SyntaxNode VisitExpr_stmt(Python3Parser.Expr_stmtContext context)
        {
            // expr_stmt: testlist_star_expr
            // (
            //    annassign |
            //    augassign (yield_expr|testlist) |
            //    (
            //       '=' (yield_expr | testlist_star_expr)
            //    ) *
            // )
            var expressions = new List<ExpressionNode>();
            var lastWasAssign = false;

            foreach (IParseTree child in context.GetChildren())
            {
                switch (child)
                {
                    case Python3Parser.Testlist_star_exprContext testListStarExpr
                        when (expressions.Count == 1 && lastWasAssign) ||
                             expressions.Count == 0:
                        var expr = (ExpressionNode) VisitTestlist_star_expr(testListStarExpr);
                        // Append expression
                        expressions.Add(expr);
                        break;

                    case Python3Parser.Testlist_star_exprContext testListStarExpr:
                        throw context.UnexpectedChildType(testListStarExpr);

                    case ITerminalNode term
                        when term.Symbol.Type == Python3Parser.ASSIGN:
                        switch (expressions.Count)
                        {
                            case 0: throw context.UnexpectedChildType(term);
                            case 1:
                                lastWasAssign = true;
                                continue;
                            default: throw term.NotYetImplementedException();
                        }

                    // These are all in name for custom error messages
                    case ITerminalNode term:
                        throw context.UnexpectedChildType(term);

                    case Python3Parser.AugassignContext augassign
                        when augassign.ChildCount == 1 && augassign.GetChild(0) is ITerminalNode term:
                        throw augassign.NotYetImplementedException(term.Symbol.Text);

                    case Python3Parser.AnnassignContext annassign:
                        throw annassign.NotYetImplementedException(":");

                    case Python3Parser.Yield_exprContext yield:
                        throw yield.NotYetImplementedException("yield");

                    case ParserRuleContext rule:
                        throw context.UnexpectedChildType(rule);
                }

                lastWasAssign = false;
            }

            if (expressions.Count == 2)
                return new Assignment(context.GetSourceReference(),
                    leftOperand: expressions[0],
                    rightOperand: expressions[1]);

            throw context.ExpectedChild();
        }

        public override SyntaxNode VisitTestlist_star_expr(Python3Parser.Testlist_star_exprContext context)
        {
            // testlist_star_expr: (test|star_expr) (',' (test|star_expr))* [',']
            var children = context.GetChildren()
                .OfType<ParserRuleContext>();

            SyntaxNode result = null;

            foreach (ParserRuleContext child in children)
            {
                switch (child)
                {
                    case Python3Parser.TestContext test
                        when result == null:
                        result = VisitTest(test);
                        break;

                    case Python3Parser.TestContext test:
                        throw test.NotYetImplementedException();

                    case Python3Parser.Star_exprContext star:
                        throw star.NotYetImplementedException();

                    default:
                        throw context.UnexpectedChildType(child);
                }
            }

            if (result == null)
                throw context.ExpectedChild();

            return result;
        }

        public override SyntaxNode VisitAnnassign(Python3Parser.AnnassignContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitAugassign(Python3Parser.AugassignContext context)
        {
            VisitChildren(context);
            var child = context.GetChild<ITerminalNode>(0);
            if (child != null)
                throw context.NotYetImplementedException(child.Symbol.Text);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTest(Python3Parser.TestContext context)
        {
            // test: or_test ['if' or_test 'else' test] | lambdef

            if (context.ChildCount == 0)
                throw context.ExpectedChild();

            var orTestOrLambda = context.GetChild(0);
            switch (orTestOrLambda)
            {
                case ITerminalNode firstTerm:
                    throw context.UnexpectedChildType(firstTerm);

                case Python3Parser.LambdefContext _
                    when context.ChildCount > 1:
                    throw context.UnexpectedChildType(context.GetChild(1));

                case Python3Parser.LambdefContext lambda:
                    throw lambda.NotYetImplementedException("lambda");

                case Python3Parser.Or_testContext orTest:
                    switch (context.ChildCount)
                    {
                        case 1:
                            return VisitOr_test(orTest);
                        case 5:
                            throw context.NotYetImplementedException("if");
                        default:
                            throw context.UnexpectedChildType(context.GetChild(1));
                    }

                default:
                    throw context.UnexpectedChildType(orTestOrLambda);
            }
        }

        public override SyntaxNode VisitOr_test(Python3Parser.Or_testContext context)
        {
            if (context.GetToken(Python3Parser.OR, 0) != null)
                throw context.NotYetImplementedException("or");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitAnd_test(Python3Parser.And_testContext context)
        {
            if (context.GetToken(Python3Parser.AND, 0) != null)
                throw context.NotYetImplementedException("and");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitNot_test(Python3Parser.Not_testContext context)
        {
            if (context.GetToken(Python3Parser.NOT, 0) != null)
                throw context.NotYetImplementedException("not");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitComparison(Python3Parser.ComparisonContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitComp_op(Python3Parser.Comp_opContext context)
        {
            if (context.GetToken(Python3Parser.NOT_EQ_1, 0) != null)
                throw context.NotYetImplementedException("<>");
            var child = context.GetChild<ITerminalNode>(0);
            if (child != null)
                throw context.NotYetImplementedException(child.Symbol.Text);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitStar_expr(Python3Parser.Star_exprContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitExpr(Python3Parser.ExprContext context)
        {
            if (context.GetToken(Python3Parser.OR_OP, 0) != null)
                throw context.NotYetImplementedException("|");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitXor_expr(Python3Parser.Xor_exprContext context)
        {
            if (context.GetToken(Python3Parser.XOR, 0) != null)
                throw context.NotYetImplementedException("^");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitAnd_expr(Python3Parser.And_exprContext context)
        {
            if (context.GetToken(Python3Parser.AND_OP, 0) != null)
                throw context.NotYetImplementedException("&");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitShift_expr(Python3Parser.Shift_exprContext context)
        {
            if (context.GetToken(Python3Parser.LEFT_SHIFT, 0) != null)
                throw context.NotYetImplementedException("<<");
            if (context.GetToken(Python3Parser.RIGHT_SHIFT, 0) != null)
                throw context.NotYetImplementedException(">>");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitArith_expr(Python3Parser.Arith_exprContext context)
        {
            if (context.GetToken(Python3Parser.ADD, 0) != null)
                throw context.NotYetImplementedException("+");
            if (context.GetToken(Python3Parser.MINUS, 0) != null)
                throw context.NotYetImplementedException("-");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitTerm(Python3Parser.TermContext context)
        {
            if (context.GetToken(Python3Parser.STAR, 0) != null)
                throw context.NotYetImplementedException("*");
            if (context.GetToken(Python3Parser.AT, 0) != null)
                throw context.NotYetImplementedException("@");
            if (context.GetToken(Python3Parser.DIV, 0) != null)
                throw context.NotYetImplementedException("/");
            if (context.GetToken(Python3Parser.MOD, 0) != null)
                throw context.NotYetImplementedException("%");
            if (context.GetToken(Python3Parser.IDIV, 0) != null)
                throw context.NotYetImplementedException("//");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitFactor(Python3Parser.FactorContext context)
        {
            if (context.GetToken(Python3Parser.ADD, 0) != null)
                throw context.NotYetImplementedException("+");
            if (context.GetToken(Python3Parser.MINUS, 0) != null)
                throw context.NotYetImplementedException("-");
            if (context.GetToken(Python3Parser.NOT_OP, 0) != null)
                throw context.NotYetImplementedException("~");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitPower(Python3Parser.PowerContext context)
        {
            if (context.GetToken(Python3Parser.POWER, 0) != null)
                throw context.NotYetImplementedException("**");
            return VisitChildren(context);
        }

        public override SyntaxNode VisitAtom_expr(Python3Parser.Atom_exprContext context)
        {
            if (context.GetToken(Python3Parser.AWAIT, 0) != null)
                throw context.NotYetImplementedException("await");
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitAtom(Python3Parser.AtomContext context)
        {
            if (context.GetToken(Python3Parser.OPEN_PAREN, 0) != null)
                throw context.NotYetImplementedException("()");
            if (context.GetToken(Python3Parser.OPEN_BRACK, 0) != null)
                throw context.NotYetImplementedException("[]");
            if (context.GetToken(Python3Parser.OPEN_BRACE, 0) != null)
                throw context.NotYetImplementedException("{}");
            var child = context.GetChild<ITerminalNode>(0);
            if (child != null)
                throw context.NotYetImplementedException(child.Symbol.Text);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTestlist_comp(Python3Parser.Testlist_compContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTrailer(Python3Parser.TrailerContext context)
        {
            if (context.GetToken(Python3Parser.OPEN_PAREN, 0) != null)
                throw context.NotYetImplementedException("()");
            if (context.GetToken(Python3Parser.OPEN_BRACK, 0) != null)
                throw context.NotYetImplementedException("[]");
            if (context.GetToken(Python3Parser.DOT, 0) != null)
                throw context.NotYetImplementedException(".");
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitSubscriptlist(Python3Parser.SubscriptlistContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitSubscript(Python3Parser.SubscriptContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitSliceop(Python3Parser.SliceopContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException(":");
        }

        public override SyntaxNode VisitExprlist(Python3Parser.ExprlistContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTestlist(Python3Parser.TestlistContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitDictorsetmaker(Python3Parser.DictorsetmakerContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }
    }
}