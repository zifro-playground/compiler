using System.Collections.Generic;
using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Operators.Arithmetics;
using Mellis.Lang.Python3.Syntax.Operators.Binaries;
using Mellis.Lang.Python3.Syntax.Operators.Logicals;
using Mellis.Lang.Python3.Syntax.Statements;

#pragma warning disable IDE0008 // Use explicit type

namespace Mellis.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitExpr_stmt(Python3Parser.Expr_stmtContext context)
        {
            // expr_stmt:
            // testlist_star_expr
            // (
            //    annassign |
            //    augassign (yield_expr|testlist) |
            //    (
            //       '=' (yield_expr | testlist_star_expr)
            //    ) *
            // )
            var firstRule = context.GetChildOrThrow<Python3Parser.Testlist_star_exprContext>(0);
            var firstExpr = VisitTestlist_star_expr(firstRule)
                .AsTypeOrThrow<ExpressionNode>();

            if (context.ChildCount == 1)
            {
                return new ExpressionStatement(firstExpr);
            }

            IParseTree second = context.GetChild(1);
            switch (second)
            {
                // testlist_star_expr ( '=' (yield_expr | testlist_star_expr) ) *
                case ITerminalNode term when term.Symbol.Type == Python3Parser.ASSIGN:
                    var expressions = new List<ExpressionNode>
                    {
                        firstExpr
                    };

                    for (var i = 2; i < context.ChildCount; i += 2)
                    {
                        var rule = context.GetChildOrThrow<ParserRuleContext>(i);

                        switch (rule)
                        {
                            case Python3Parser.Yield_exprContext _:
                                throw rule.NotYetImplementedException("yield");

                            case Python3Parser.Testlist_star_exprContext testListStar:
                                var expr = VisitTestlist_star_expr(testListStar)
                                    .AsTypeOrThrow<ExpressionNode>();

                                expressions.Add(expr);
                                break;

                            default:
                                throw context.UnexpectedChildType(rule);
                        }

                        if (i + 1 < context.ChildCount)
                        {
                            // verify it's an assign token
                            ITerminalNode assign = context.GetChildOrThrow(i + 1, Python3Parser.ASSIGN);
                            // cant handle multiple assigns yet
                            throw assign.NotYetImplementedException("=");
                        }
                    }

                    return new Assignment(
                        context.GetSourceReference(),
                        expressions[0],
                        expressions[1]);

                // testlist_star_expr annassign
                case Python3Parser.AnnassignContext _ when context.ChildCount > 2:
                    throw context.UnexpectedChildType(context.GetChild(2));
                case Python3Parser.AnnassignContext annAssign:
                    throw annAssign.NotYetImplementedException(":");

                // testlist_star_expr augassign (yield_expr | testlist)
                case Python3Parser.AugassignContext _ when context.ChildCount > 3:
                    throw context.UnexpectedChildType(context.GetChild(3));
                case Python3Parser.AugassignContext augAssign:
                    string keyword = augAssign.GetChildOrThrow<ITerminalNode>(0).Symbol.Text;
                    throw augAssign.NotYetImplementedException(keyword);

                default:
                    throw context.UnexpectedChildType(second);
            }
        }

        public override SyntaxNode VisitTestlist_star_expr(Python3Parser.Testlist_star_exprContext context)
        {
            // testlist_star_expr: (test|star_expr) (',' (test|star_expr))* [',']

            ExpressionNode result = null;

            for (var i = 0; i < context.ChildCount; i += 2)
            {
                var rule = context.GetChildOrThrow<ParserRuleContext>(i);

                if (result == null)
                {
                    result = VisitTestOrStar(rule);
                }
                else
                {
                    throw rule.NotYetImplementedException();
                }

                if (i + 1 < context.ChildCount)
                {
                    context.GetChildOrThrow(i + 1, Python3Parser.COMMA);
                }
            }

            if (result == null)
            {
                throw context.ExpectedChild();
            }

            return result;

            ExpressionNode VisitTestOrStar(ParserRuleContext rule)
            {
                switch (rule)
                {
                    case Python3Parser.TestContext test:
                        return VisitTest(test)
                            .AsTypeOrThrow<ExpressionNode>();

                    case Python3Parser.Star_exprContext star:
                        throw star.NotYetImplementedException();

                    default:
                        throw context.UnexpectedChildType(rule);
                }
            }
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
            {
                throw context.NotYetImplementedException(child.Symbol.Text);
            }

            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTest(Python3Parser.TestContext context)
        {
            // test: or_test ['if' or_test 'else' test] | lambdef

            if (context.ChildCount == 0)
            {
                throw context.ExpectedChild();
            }

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
                {
                    return VisitOr_test(orTest);
                }

                if (context.ChildCount > 5)
                {
                    throw context.UnexpectedChildType(context.GetChild(5));
                }

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
                {
                    throw context.UnexpectedChildType(context.GetChild(2));
                }

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
            {
                throw context.ExpectedChild();
            }

            if (!(first is ITerminalNode term))
            {
                throw context.UnexpectedChildType(first);
            }

            ComparisonType type = GetType(term.Symbol.Type);

            if (type == ComparisonType.InNot ||
                type == ComparisonType.IsNot)
            {
                ThrowIfMoreThan(2);
            }
            else
            {
                ThrowIfMoreThan(1);
            }

            return new ComparisonFactory(type);

            void ThrowIfMoreThan(int count)
            {
                if (context.ChildCount > count)
                {
                    throw context.UnexpectedChildType(context.GetChild(count));
                }
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
            {
                return expr;
            }

            context.GetChildOrThrow(1, Python3Parser.POWER);
            var factor = context.GetChildOrThrow<Python3Parser.FactorContext>(2);

            if (context.ChildCount > 3)
            {
                throw context.UnexpectedChildType(context.GetChild(3));
            }

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

                case Python3Parser.AtomContext atom:
                    var expr = VisitAtom(atom)
                        .AsTypeOrThrow<ExpressionNode>();
                    SourceReference atomSource = atom.GetSourceReference();

                    for (var i = 1; i < context.ChildCount; i++)
                    {
                        var trailerRule = context.GetChildOrThrow<Python3Parser.TrailerContext>(i);
                        var trailerExpr = VisitTrailer(trailerRule);

                        switch (trailerExpr)
                        {
                            case ArgumentsList argList:
                                var source = SourceReference.Merge(
                                    atomSource,
                                    trailerRule.GetSourceReference()
                                );
                                // Nests the expression deeper
                                expr = new FunctionCall(source, expr, argList);
                                break;
                            default:
                                throw trailerExpr.WrongTypeException(
                                    typeof(ArgumentsList)
                                    // TODO: add indexing list type
                                    // TODO: add property get type
                                );
                        }
                    }

                    return expr;

                default:
                    throw context.UnexpectedChildType(first);
            }
        }

        public override SyntaxNode VisitAtom(Python3Parser.AtomContext context)
        {
            // atom:
            //    '(' [yield_expr|testlist_comp] ')' |
            //    '[' [testlist_comp] ']' |
            //    '{' [dictorsetmaker] '}' |
            //    NAME | NUMBER | STRING+ | '...' | 'None' | 'True' | 'False'
            var firstToken = context.GetChildOrThrow<ITerminalNode>(0);

            switch (firstToken.Symbol.Type)
            {
                case Python3Parser.NAME when context.ChildCount > 1:
                case Python3Parser.TRUE when context.ChildCount > 1:
                case Python3Parser.FALSE when context.ChildCount > 1:
                case Python3Parser.NUMBER when context.ChildCount > 1:
                case Python3Parser.ELLIPSIS when context.ChildCount > 1:
                case Python3Parser.NONE when context.ChildCount > 1:
                    throw context.UnexpectedChildType(context.GetChild(1));

                case Python3Parser.NAME:
                    return new Identifier(firstToken.GetSourceReference(), firstToken.Symbol.Text);

                case Python3Parser.TRUE:
                    return new LiteralBoolean(firstToken.GetSourceReference(), true);
                case Python3Parser.FALSE:
                    return new LiteralBoolean(firstToken.GetSourceReference(), false);

                case Python3Parser.NUMBER when firstToken.Symbol.Text.EndsWith("j", true, CultureInfo.InvariantCulture):
                    throw new SyntaxNotYetImplementedException(firstToken.GetSourceReference());

                case Python3Parser.NUMBER:
                    try
                    {
                        return LiteralInteger.Parse(firstToken.GetSourceReference(),
                            firstToken.Symbol.Text);
                    }
                    catch (SyntaxLiteralFormatException)
                    {
                        return LiteralDouble.Parse(firstToken.GetSourceReference(),
                            firstToken.Symbol.Text);
                    }

                case Python3Parser.STRING:
                    for (var i = 1; i < context.ChildCount; /* i++ */)
                    {
                        throw new SyntaxNotYetImplementedException(
                            context.GetChildOrThrow(i, Python3Parser.STRING)
                                .GetSourceReference());
                    }

                    return LiteralString.Parse(firstToken.GetSourceReference(),
                        firstToken.Symbol.Text);

                case Python3Parser.ELLIPSIS:
                case Python3Parser.NONE:
                    throw firstToken.NotYetImplementedException();

                case Python3Parser.OPEN_PAREN:
                    context.ExpectClosingParenthesis(firstToken, Python3Parser.CLOSE_PAREN);

                    if (context.ChildCount == 2)
                    {
                        // <(>, <missing rule>, <)>
                        throw context.ExpectedChild();
                    }
                    // <(>, <secondRule>, <unexpected>+, <)>
                    else if (context.ChildCount > 3)
                    {
                        throw context.UnexpectedChildType(context.GetChild(2));
                    }

                    // <(>, <secondRule>, <)>
                    var secondRule = context.GetChildOrThrow<ParserRuleContext>(1);
                    switch (secondRule)
                    {
                        case Python3Parser.Yield_exprContext yield:
                            throw yield.NotYetImplementedException("yield");
                        case Python3Parser.Testlist_compContext testListComp:
                            return VisitTestlist_comp(testListComp);
                        default:
                            throw context.UnexpectedChildType(secondRule);
                    }

                case Python3Parser.OPEN_BRACE:
                    context.ExpectClosingParenthesis(firstToken, Python3Parser.CLOSE_BRACE);
                    throw firstToken.NotYetImplementedException("{}");

                case Python3Parser.OPEN_BRACK:
                    context.ExpectClosingParenthesis(firstToken, Python3Parser.CLOSE_BRACK);
                    throw firstToken.NotYetImplementedException("[]");

                default:
                    throw context.UnexpectedChildType(firstToken);
            }
        }

        public override SyntaxNode VisitTestlist_comp(Python3Parser.Testlist_compContext context)
        {
            // testlist_comp: (test|star_expr)
            //      (
            //          comp_for |
            //          (',' (test|star_expr))* [',']
            //      )

            var firstRule = context.GetChildOrThrow<ParserRuleContext>(0);
            ExpressionNode result = VisitTestOrStar(firstRule);

            if (context.ChildCount == 1)
            {
                return result;
            }

            // Check 2nd, comp_for or comma?
            var second = context.GetChildOrThrow<IParseTree>(1);

            switch (second)
            {
                case Python3Parser.Comp_forContext compFor:
                    throw compFor.NotYetImplementedException("for");

                case ITerminalNode term
                    when term.Symbol.Type == Python3Parser.COMMA:
                    // Continue the code
                    break;

                default:
                    throw context.UnexpectedChildType(second);
            }

            // Start from the 3rd
            for (var i = 2; i < context.ChildCount; i += 2)
            {
                var rule = context.GetChildOrThrow<ParserRuleContext>(i);

                if (rule is Python3Parser.TestContext ||
                    rule is Python3Parser.Star_exprContext)
                {
                    throw rule.NotYetImplementedException();
                }
                else
                {
                    throw context.UnexpectedChildType(rule);
                }

#pragma warning disable 162
                if (i + 1 < context.ChildCount)
                {
                    context.GetChildOrThrow(i + 1, Python3Parser.COMMA);
                }
#pragma warning restore 162
            }

            if (result == null)
            {
                throw context.ExpectedChild();
            }

            return result;

            ExpressionNode VisitTestOrStar(ParserRuleContext rule)
            {
                switch (rule)
                {
                    case Python3Parser.TestContext test:
                        return VisitTest(test)
                            .AsTypeOrThrow<ExpressionNode>();

                    case Python3Parser.Star_exprContext star:
                        throw star.NotYetImplementedException();

                    default:
                        throw context.UnexpectedChildType(rule);
                }
            }
        }

        public override SyntaxNode VisitTrailer(Python3Parser.TrailerContext context)
        {
            // trailer: '(' [arglist] ')' | '[' subscriptlist ']' | '.' NAME

            var firstTerm = context.GetChildOrThrow<ITerminalNode>(0);

            switch (firstTerm.Symbol.Type)
            {
                // Throw if too many children
                case Python3Parser.OPEN_PAREN when context.ChildCount > 3:
                case Python3Parser.OPEN_BRACK when context.ChildCount > 3:
                    throw context.UnexpectedChildType(context.GetChild(3));

                // Function: No arguments
                case Python3Parser.OPEN_PAREN when context.ChildCount == 2:
                    context.ExpectClosingParenthesis(firstTerm, Python3Parser.CLOSE_PAREN);
                    return new ArgumentsList(context.GetSourceReference(), new List<ExpressionNode>());

                // Function: Arguments list
                case Python3Parser.OPEN_PAREN:
                    context.ExpectClosingParenthesis(firstTerm, Python3Parser.CLOSE_PAREN);

                    var argsRule = context.GetChildOrThrow<Python3Parser.ArglistContext>(1);

                    var argsNode = VisitArglist(argsRule)
                        .AsTypeOrThrow<ArgumentsList>();

                    return argsNode;

                // List indexing
                case Python3Parser.OPEN_BRACK:
                    context.ExpectClosingParenthesis(firstTerm, Python3Parser.CLOSE_BRACK);
                    // No inner subscription list
                    if (context.ChildCount == 2)
                {
                    throw context.ExpectedChild();
                }

                context.GetChildOrThrow<Python3Parser.SubscriptlistContext>(1);
                    throw context.NotYetImplementedException("[]");

                // Property accessing
                case Python3Parser.DOT when context.ChildCount > 2:
                    throw context.UnexpectedChildType(context.GetChild(2));
                case Python3Parser.DOT:
                    context.GetChildOrThrow(1, Python3Parser.NAME);
                    throw context.NotYetImplementedException(".");

                // Who are you?
                default:
                    throw context.UnexpectedChildType(firstTerm);
            }
        }

        public override SyntaxNode VisitArglist(Python3Parser.ArglistContext context)
        {
            // arglist: argument (',' argument)*  [',']

            var exprList = new List<ExpressionNode>();

            if (context.ChildCount == 0)
            {
                throw context.ExpectedChild();
            }

            for (var i = 0; i < context.ChildCount; i += 2)
            {
                var argRule = context.GetChildOrThrow<Python3Parser.ArgumentContext>(i);
                if (i + 1 < context.ChildCount)
                {
                    context.GetChildOrThrow(i + 1, Python3Parser.COMMA);
                }

                var argExpr = VisitArgument(argRule)
                    .AsTypeOrThrow<ExpressionNode>();

                exprList.Add(argExpr);
            }

            return new ArgumentsList(context.GetSourceReference(), exprList);
        }

        public override SyntaxNode VisitArgument(Python3Parser.ArgumentContext context)
        {
            // argument: (
            //      test [comp_for] |
            //      test '=' test |
            //      '**' test |
            //      '*' test
            // )

            var first = context.GetChildOrThrow<IParseTree>(0);
            switch (first)
            {
                // test
                case Python3Parser.TestContext test when context.ChildCount == 1:
                {
                    var expr = VisitTest(test)
                        .AsTypeOrThrow<ExpressionNode>();

                    return expr;
                }

                // test comp_for
                case Python3Parser.TestContext test when context.ChildCount == 2:
                {
                    var compForRule = context.GetChildOrThrow<Python3Parser.Comp_forContext>(1);
                    throw compForRule.NotYetImplementedException("for");
                }

                // test '=' test
                case Python3Parser.TestContext _ when context.ChildCount > 3:
                    throw context.UnexpectedChildType(context.GetChild(3));
                case Python3Parser.TestContext test:
                {
                    var assign = context.GetChildOrThrow(1, Python3Parser.ASSIGN);
                    context.GetChildOrThrow<Python3Parser.TestContext>(2);
                    throw assign.NotYetImplementedException("=");
                }

                case ITerminalNode term:
                {
                    switch (term.Symbol.Type)
                    {
                        // '**' test
                        case Python3Parser.POWER when context.ChildCount > 2:
                            throw context.UnexpectedChildType(context.GetChild(2));
                        case Python3Parser.POWER:
                        {
                            context.GetChildOrThrow<Python3Parser.TestContext>(1);
                            throw term.NotYetImplementedException("**");
                        }

                        // '*' test
                        case Python3Parser.STAR when context.ChildCount > 2:
                            throw context.UnexpectedChildType(context.GetChild(2));
                        case Python3Parser.STAR:
                        {
                            context.GetChildOrThrow<Python3Parser.TestContext>(1);
                            throw term.NotYetImplementedException("*");
                        }

                        default:
                            throw context.UnexpectedChildType(term);
                    }
                }

                default:
                    throw context.UnexpectedChildType(first);
            }
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
            // testlist: test (',' test)* [',']
            var rule = context.GetChildOrThrow<Python3Parser.TestContext>(0);

            var expr = VisitTest(rule)
                .AsTypeOrThrow<ExpressionNode>();

            for (var i = 1; i < context.ChildCount; i += 2)
            {
                context.GetChildOrThrow(i, Python3Parser.COMMA);
                if (i == context.ChildCount - 1)
                {
                    break;
                }

                var secondRule = context.GetChildOrThrow<Python3Parser.TestContext>(i + 1);

                throw secondRule.NotYetImplementedException();
                // TODO:
                //var secondExpr = VisitTest(secondRule)
                //    .AsTypeOrThrow<ExpressionNode>();
            }

            return expr;
        }

        public override SyntaxNode VisitDictorsetmaker(Python3Parser.DictorsetmakerContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }
    }
}