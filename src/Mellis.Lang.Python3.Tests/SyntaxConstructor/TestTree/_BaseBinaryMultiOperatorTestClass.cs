using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    public abstract class BaseBinaryMultiOperatorTestClass<TContext, TInnerContext>
        : BaseVisitTestClass<TContext, TInnerContext>
        where TContext : ParserRuleContext
        where TInnerContext : ParserRuleContext
    {
        public override void SetupForInnerMock(Mock<TInnerContext> innerMock, SyntaxNode returnValue)
        {
            RawSetupForInnerMock(innerMock)
                .Returns(returnValue).Verifiable();
        }

        public abstract ISetup<Grammar.SyntaxConstructor, SyntaxNode> RawSetupForInnerMock(
            Mock<TInnerContext> innerMock);

        public override ITerminalNode GetTerminalForThisClass()
        {
            throw new NotSupportedException("Use DataRow attributes instead.");
        }

        public static IEnumerable<(int token, Type expectedType)> OperatorsAndExpectedTypes;

        public static IEnumerable<object[]> DataTokenExpectedType
            => OperatorsAndExpectedTypes
                .Select(o => new object[] { o.token, o.expectedType });

        public static IEnumerable<object[]> DataDoubleDifferentTokenExpectedType
            => from data1 in OperatorsAndExpectedTypes
                join data2 in OperatorsAndExpectedTypes on 1 equals 1
                where data1.token != data2.token
                select new object[]
                {
                    data1.token,
                    data1.expectedType,
                    data2.token,
                    data2.expectedType
                };

        public static IEnumerable<object[]> DataTokens
            => OperatorsAndExpectedTypes
                .Select(o => new object[]
                {
                    o.token
                });

        public static string GetCustomDynamicDataDisplayName(MethodInfo method, object[] data)
        {
            string name = method.Name.Substring("Visit_".Length);
            name = name.Substring(0, name.Length - "_Test".Length);

            return string.Join(" · ", data.Select(o =>
            {
                switch (o)
                {
                    case Type t:
                        return t.Name;

                    case int i:
                        return Python3Parser.DefaultVocabulary.GetLiteralName(i);

                    default:
                        return o.ToString();
                }
            })) + $" ({name})";
        }

        [TestMethod]
        public virtual void Visit_SingleRule_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerRuleMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerRuleMock.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DynamicData(nameof(DataTokenExpectedType), DynamicDataDisplayName = nameof(GetCustomDynamicDataDisplayName))]
        public virtual void Visit_MultipleRuleSingleToken_Test(int token, Type expectedOperatorType)
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerRuleMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(token),
                innerRuleMock.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.That.IsBinaryOperator(expectedOperatorType, expected, expected, result);
            contextMock.VerifyLoopedChildren(3);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DynamicData(nameof(DataTokenExpectedType), DynamicDataDisplayName = nameof(GetCustomDynamicDataDisplayName))]
        public virtual void Visit_InvalidMissingRhs_Test(int token, Type expectedOperatorType)
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(token)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public virtual void Visit_MultipleRuleInvalidToken_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            var unexpectedNode = GetTerminal(Python3Parser.ASYNC);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                unexpectedNode,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            // ~~Suppose error directly on token~~
            // Allow any number of iterations through children
            //contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DynamicData(nameof(DataTokens), DynamicDataDisplayName = nameof(GetCustomDynamicDataDisplayName))]
        public virtual void Visit_MultipleRuleExcessNode_Test(int token)
        {
            // Arrange
            var innerRuleMock = GetInnerMockWithSetup(GetExpressionMock());

            var unexpectedNode = GetTerminal(token);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(token),
                innerRuleMock.Object,
                unexpectedNode
            );

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(4);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DynamicData(nameof(DataTokenExpectedType), DynamicDataDisplayName = nameof(GetCustomDynamicDataDisplayName))]
        public virtual void Visit_MultipleSameOpsOrder_Test(int token, Type expectedType)
        {
            // Arrange
            var expected1 = GetExpressionMock();
            var expected2 = GetExpressionMock();
            var expected3 = GetExpressionMock();

            var innerRuleMock1 = GetInnerMockWithSetup(expected1);
            var innerRuleMock2 = GetInnerMockWithSetup(expected2);
            var innerRuleMock3 = GetInnerMockWithSetup(expected3);

            contextMock.SetupChildren(
                innerRuleMock1.Object,
                GetTerminal(token),
                innerRuleMock2.Object,
                GetTerminal(token),
                innerRuleMock3.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            // Expect order ((1 op 2) op 3)
            // so it compiles left-to-right
            var lhs = Assert.That.IsBinaryOperatorGetLhs(
                expectedType: expectedType,
                expectedRhs: expected3, result);

            Assert.That.IsBinaryOperator(
                expectedType: expectedType,
                expectedLhs: expected1,
                expectedRhs: expected2, lhs);

            contextMock.VerifyLoopedChildren(5);

            innerRuleMock1.Verify();
            innerRuleMock2.Verify();
            innerRuleMock3.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DynamicData(nameof(DataDoubleDifferentTokenExpectedType), DynamicDataDisplayName = nameof(GetCustomDynamicDataDisplayName))]
        public virtual void Visit_MultipleDistinctOpsOrder_Test(int token1, Type expectedType1, int token2, Type expectedType2)
        {
            // Arrange
            var expected1 = GetExpressionMock();
            var expected2 = GetExpressionMock();
            var expected3 = GetExpressionMock();

            var innerRuleMock1 = GetInnerMockWithSetup(expected1);
            var innerRuleMock2 = GetInnerMockWithSetup(expected2);
            var innerRuleMock3 = GetInnerMockWithSetup(expected3);

            contextMock.SetupChildren(
                innerRuleMock1.Object,
                GetTerminal(token1),
                innerRuleMock2.Object,
                GetTerminal(token2),
                innerRuleMock3.Object
            );

            // Act
            var result = VisitContext();

            // Assert
            // Expect order ((1 op 2) op 3)
            // so it compiles left-to-right
            var lhs = Assert.That.IsBinaryOperatorGetLhs(
                expectedType: expectedType2,
                expectedRhs: expected3, result);

            Assert.That.IsBinaryOperator(
                expectedType: expectedType1,
                expectedLhs: expected1,
                expectedRhs: expected2, lhs);

            contextMock.VerifyLoopedChildren(5);

            innerRuleMock1.Verify();
            innerRuleMock2.Verify();
            innerRuleMock3.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        #region Inherited tests

        // This is here due to some testing tools only look
        // 1 in depth in hierarchy for tests

        [TestMethod]
        public override void Visit_InvalidRule_Test()
        {
            base.Visit_InvalidRule_Test();
        }

        [TestMethod]
        public override void Visit_InvalidToken_Test()
        {
            base.Visit_InvalidToken_Test();
        }

        [TestMethod]
        public override void Visit_NoChildren_Test()
        {
            base.Visit_NoChildren_Test();
        }

        #endregion
    }
}