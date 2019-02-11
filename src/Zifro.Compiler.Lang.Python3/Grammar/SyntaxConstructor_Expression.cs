using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals;
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
                    return HandleOrTest(orTest);

                default:
                    throw context.UnexpectedChildType(orTestOrLambda);
            }

            SyntaxNode HandleOrTest(Python3Parser.Or_testContext orTest)
            {
                if (context.ChildCount == 1)
                    return VisitOr_test(orTest);

                if (context.ChildCount > 5)
                    throw context.UnexpectedChildType(context.GetChild(5));

                // expecting: or_test 'if' or_test 'else' test
                ITerminalNode ifNode = context.GetChildOrThrow(1, Python3Parser.IF);
                var condition = context.GetChildOrThrow<Python3Parser.Or_testContext>(2);
                ITerminalNode elseNode = context.GetChildOrThrow(3, Python3Parser.ELSE);
                var elseTest = context.GetChildOrThrow<Python3Parser.TestContext>(4);

                throw ifNode.NotYetImplementedException();
            }
        }

        public override SyntaxNode VisitOr_test(Python3Parser.Or_testContext context)
        {
            // or_test: and_test ('or' and_test)*
            var rule = context.GetChildOrThrow<Python3Parser.And_testContext>(0);

            var expr = VisitAnd_test(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                context.GetChildOrThrow(i, Python3Parser.OR);

                var secondRule = context.GetChildOrThrow<Python3Parser.And_testContext>(i + 1);
                var secondExpr = VisitAnd_test(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                expr = new LogicalOr(expr, secondExpr);
            }

            return expr;
        }

        public override SyntaxNode VisitAnd_test(Python3Parser.And_testContext context)
        {
            // and_test: not_test ('and' not_test)*
            var rule = context.GetChildOrThrow<Python3Parser.Not_testContext>(0);

            var expr = VisitNot_test(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                context.GetChildOrThrow(i, Python3Parser.AND);
                var secondRule = context.GetChildOrThrow<Python3Parser.Not_testContext>(i + 1);
                var secondExpr = VisitNot_test(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                expr = new LogicalAnd(expr, secondExpr);
            }

            return expr;
        }

        public override SyntaxNode VisitNot_test(Python3Parser.Not_testContext context)
        {
            // not_test: 'not' not_test | comparison
            var first = context.GetChild(0)
                        ?? throw context.ExpectedChild();

            switch (first)
            {
                case ITerminalNode term
                    when term.Symbol.Type != Python3Parser.NOT:
                    throw context.UnexpectedChildType(term);

                case ITerminalNode _:
                    var nested = context.GetChildOrThrow<Python3Parser.Not_testContext>(1);

                    if (context.ChildCount > 2)
                        throw context.UnexpectedChildType(context.GetChild(2));

                    var nestedExpr = VisitNot_test(nested)
                        .AsTypeOrThrow<ExpressionNode>();

                    return new LogicalNot(context.GetSourceReference(), nestedExpr);

                case Python3Parser.ComparisonContext _
                    when context.ChildCount > 1:
                    throw context.UnexpectedChildType(context.GetChild(1));

                case Python3Parser.ComparisonContext comp:
                    return VisitComparison(comp);

                default:
                    throw context.UnexpectedChildType(first);
            }
        }

        public override SyntaxNode VisitComparison(Python3Parser.ComparisonContext context)
        {
            // comparison: expr (comp_op expr)*
            var rule = context.GetChildOrThrow<Python3Parser.ExprContext>(0);

            var expr = VisitExpr(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var compRule = context.GetChildOrThrow<Python3Parser.Comp_opContext>(i);
                var rhsRule = context.GetChildOrThrow<Python3Parser.ExprContext>(i + 1);

                var compFactory = VisitComp_op(compRule)
                    .AsTypeOrThrow<ComparisonFactory>();
                var secondExpr = VisitExpr(rhsRule)
                    .AsTypeOrThrow<ExpressionNode>();

                expr = compFactory.Create(expr, secondExpr);
            }

            return expr;
        }

        public override SyntaxNode VisitComp_op(Python3Parser.Comp_opContext context)
        {
            // comp_op: '<'|'>'|'=='|'>='|'<='|'<>'|'!='|
            //    'in'|'not' 'in'|'is'|'is' 'not'
            IParseTree first = context.GetChild(0);

            if (first == null)
                throw context.ExpectedChild();

            if (!(first is ITerminalNode term))
                throw context.UnexpectedChildType(first);

            ComparisonType type = GetType(term.Symbol.Type);

            if (type == ComparisonType.InNot ||
                type == ComparisonType.IsNot)
                ThrowIfMoreThan(2);
            else
                ThrowIfMoreThan(1);

            return new ComparisonFactory(type);

            void ThrowIfMoreThan(int count)
            {
                if (context.ChildCount > count)
                    throw context.UnexpectedChildType(context.GetChild(count));
            }

            ComparisonType GetType(int symbolType)
            {
                switch (symbolType)
                {
                    case Python3Parser.LESS_THAN:
                        return ComparisonType.LessThan;
                    case Python3Parser.GREATER_THAN:
                        return ComparisonType.GreaterThan;
                    case Python3Parser.EQUALS:
                        return ComparisonType.Equals;
                    case Python3Parser.GT_EQ:
                        return ComparisonType.GreaterThanOrEqual;
                    case Python3Parser.LT_EQ:
                        return ComparisonType.LessThanOrEqual;
                    case Python3Parser.NOT_EQ_1:
                        return ComparisonType.NotEqualsABC;
                    case Python3Parser.NOT_EQ_2:
                        return ComparisonType.NotEquals;
                    case Python3Parser.IN:
                        return ComparisonType.In;

                    case Python3Parser.IS when context.ChildCount > 1:
                        context.GetChildOrThrow(1, Python3Parser.NOT);
                        return ComparisonType.IsNot;

                    case Python3Parser.IS:
                        return ComparisonType.Is;

                    case Python3Parser.NOT:
                        context.GetChildOrThrow(1, Python3Parser.IN);
                        return ComparisonType.InNot;

                    default:
                        throw context.UnexpectedChildType(term);
                }
            }
        }


        public override SyntaxNode VisitStar_expr(Python3Parser.Star_exprContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitExpr(Python3Parser.ExprContext context)
        {
            // expr: xor_expr ('|' xor_expr)*
            var rule = context.GetChildOrThrow<Python3Parser.Xor_exprContext>(0);

            var expr = VisitXor_expr(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                context.GetChildOrThrow(i, Python3Parser.OR_OP);
                var secondRule = context.GetChildOrThrow<Python3Parser.Xor_exprContext>(i + 1);
                var secondExpr = VisitXor_expr(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                expr = new BinaryOr(expr, secondExpr);
            }

            return expr;
        }

        public override SyntaxNode VisitXor_expr(Python3Parser.Xor_exprContext context)
        {
            // xor_expr: and_expr ('^' and_expr)*
            var rule = context.GetChildOrThrow<Python3Parser.And_exprContext>(0);

            var expr = VisitAnd_expr(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                context.GetChildOrThrow(i, Python3Parser.XOR);
                var secondRule = context.GetChildOrThrow<Python3Parser.And_exprContext>(i + 1);
                var secondExpr = VisitAnd_expr(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                expr = new BinaryXor(expr, secondExpr);
            }

            return expr;
        }

        public override SyntaxNode VisitAnd_expr(Python3Parser.And_exprContext context)
        {
            // and_expr: shift_expr ('&' shift_expr)*
            var rule = context.GetChildOrThrow<Python3Parser.Shift_exprContext>(0);

            var expr = VisitShift_expr(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                context.GetChildOrThrow(i, Python3Parser.AND_OP);
                var secondRule = context.GetChildOrThrow<Python3Parser.Shift_exprContext>(i + 1);
                var secondExpr = VisitShift_expr(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                expr = new BinaryAnd(expr, secondExpr);
            }

            return expr;
        }

        public override SyntaxNode VisitShift_expr(Python3Parser.Shift_exprContext context)
        {
            // shift_expr: arith_expr (('<<'|'>>') arith_expr)*
            var rule = context.GetChildOrThrow<Python3Parser.Arith_exprContext>(0);

            var expr = VisitArith_expr(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.GetChildOrThrow<ITerminalNode>(i);
                var secondRule = context.GetChildOrThrow<Python3Parser.Arith_exprContext>(i + 1);
                var secondExpr = VisitArith_expr(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                switch (op.Symbol.Type)
                {
                    case Python3Parser.LEFT_SHIFT:
                        expr = new BinaryLeftShift(expr, secondExpr);
                        break;
                    case Python3Parser.RIGHT_SHIFT:
                        expr = new BinaryRightShift(expr, secondExpr);
                        break;
                    default:
                        throw context.UnexpectedChildType(op);
                }
            }

            return expr;
        }

        public override SyntaxNode VisitArith_expr(Python3Parser.Arith_exprContext context)
        {
            // arith_expr: term (('+'|'-') term)*
            var rule = context.GetChildOrThrow<Python3Parser.TermContext>(0);

            var expr = VisitTerm(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.GetChildOrThrow<ITerminalNode>(i);
                var secondRule = context.GetChildOrThrow<Python3Parser.TermContext>(i + 1);
                var secondExpr = VisitTerm(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                switch (op.Symbol.Type)
                {
                    case Python3Parser.ADD:
                        expr = new ArithmeticAdd(expr, secondExpr);
                        break;
                    case Python3Parser.MINUS:
                        expr = new ArithmeticSubtract(expr, secondExpr);
                        break;
                    default:
                        throw context.UnexpectedChildType(op);
                }
            }

            return expr;
        }

        public override SyntaxNode VisitTerm(Python3Parser.TermContext context)
        {
            // term: factor (('*'|'@'|'/'|'%'|'//') factor)*
            var rule = context.GetChildOrThrow<Python3Parser.FactorContext>(0);

            var expr = VisitFactor(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                var op = context.GetChildOrThrow<ITerminalNode>(i);
                var secondRule = context.GetChildOrThrow<Python3Parser.FactorContext>(i + 1);
                var secondExpr = VisitFactor(secondRule)
                    .AsTypeOrThrow<ExpressionNode>();

                switch (op.Symbol.Type)
                {
                    case Python3Parser.STAR:
                        expr = new ArithmeticMultiply(expr, secondExpr);
                        break;

                    case Python3Parser.AT:
                        throw op.NotYetImplementedException();

                    case Python3Parser.DIV:
                        expr = new ArithmeticDivide(expr, secondExpr);
                        break;

                    case Python3Parser.MOD:
                        expr = new ArithmeticModulus(expr, secondExpr);
                        break;

                    case Python3Parser.IDIV:
                        expr = new ArithmeticFloor(expr, secondExpr);
                        break;

                    default:
                        throw context.UnexpectedChildType(op);
                }
            }

            return expr;
        }

        public override SyntaxNode VisitFactor(Python3Parser.FactorContext context)
        {
            // factor: ('+' | '-' | '~') factor | power
            var first = context.GetChildOrThrow<IParseTree>(0);

            switch (first)
            {
                case Python3Parser.PowerContext _
                    when context.ChildCount > 1:
                    throw context.UnexpectedChildType(context.GetChild(1));

                case Python3Parser.PowerContext power:
                    return VisitPower(power);

                case ITerminalNode _
                    when context.ChildCount > 2:
                    throw context.UnexpectedChildType(context.GetChild(2));

                case ITerminalNode term:
                    switch (term.Symbol.Type)
                    {
                        case Python3Parser.ADD:
                        {
                            ExpressionNode innerExpr = GetInnerExpr();
                            return new ArithmeticPositive(context.GetSourceReference(), innerExpr);
                        }
                        case Python3Parser.MINUS:
                        {
                            ExpressionNode innerExpr = GetInnerExpr();
                            return new ArithmeticNegative(context.GetSourceReference(), innerExpr);
                        }
                        case Python3Parser.NOT_OP:
                        {
                            ExpressionNode innerExpr = GetInnerExpr();
                            return new BinaryNot(context.GetSourceReference(), innerExpr);
                        }
                        default:
                            throw context.UnexpectedChildType(term);
                    }

                default:
                    throw context.UnexpectedChildType(first);
            }

            ExpressionNode GetInnerExpr()
            {
                var innerFactor = context.GetChildOrThrow<Python3Parser.FactorContext>(1);
                var innerExpr = VisitFactor(innerFactor)
                    .AsTypeOrThrow<ExpressionNode>();

                return innerExpr;
            }
        }

        public override SyntaxNode VisitPower(Python3Parser.PowerContext context)
        {
            // power: atom_expr ['**' factor]
            var first = context.GetChildOrThrow<Python3Parser.Atom_exprContext>(0);
            var expr = VisitAtom_expr(first)
                .AsTypeOrThrow<ExpressionNode>();

            if (context.ChildCount == 1)
                return expr;

            context.GetChildOrThrow(1, Python3Parser.POWER);
            var factor = context.GetChildOrThrow<Python3Parser.FactorContext>(2);

            if (context.ChildCount > 3)
                throw context.UnexpectedChildType(context.GetChild(3));

            var factorExpr = VisitFactor(factor)
                .AsTypeOrThrow<ExpressionNode>();

            return new ArithmeticPower(expr, factorExpr);
        }

        public override SyntaxNode VisitAtom_expr(Python3Parser.Atom_exprContext context)
        {
            // atom_expr: ['await'] atom trailer*
            var first = context.GetChildOrThrow<IParseTree>(0);

            switch (first)
            {
                case ITerminalNode node
                    when node.Symbol.Type == Python3Parser.AWAIT && context.ChildCount == 1:
                    throw context.ExpectedChild();

                case ITerminalNode node
                    when node.Symbol.Type == Python3Parser.AWAIT:
                    throw node.NotYetImplementedException();

                case ITerminalNode node:
                    throw context.UnexpectedChildType(node);

                case Python3Parser.AtomContext atom
                    when context.ChildCount == 1:
                    return VisitAtom(atom).AsTypeOrThrow<ExpressionNode>();

                case Python3Parser.AtomContext atom:
                    var trailer = context.GetChildOrThrow<Python3Parser.TrailerContext>(1);
                    throw trailer.NotYetImplementedException();

                default:
                    throw context.UnexpectedChildType(first);
            }
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