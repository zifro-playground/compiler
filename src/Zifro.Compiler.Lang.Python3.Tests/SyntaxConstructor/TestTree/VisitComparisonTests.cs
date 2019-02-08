using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitComparisonTests
        : BaseVisitTestClass<Python3Parser.ComparisonContext, Python3Parser.ExprContext>
    {
        public override ITerminalNode GetTerminalForThisClass()
        {
            throw new NotSupportedException("Comparisons do not have terminal node. Comparison operator does however!");
        }

        public override void SetupForInnerMock(Mock<Python3Parser.ExprContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitExpr(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override SyntaxNode VisitContext()
        {
            return ctor.VisitComparison(contextMock.Object);
        }

        public Mock<Python3Parser.Comp_opContext> GetCompOpMockWithSetup(ComparisonType compType = ComparisonType.Equals)
        {
            var mock = GetCompOpMock();

            ctorMock.Setup(o => o.VisitComp_op(mock.Object))
                .Returns<Python3Parser.Comp_opContext>(o
                    => new ComparisonFactory(compType)).Verifiable();

            return mock;
        }

        public Mock<Python3Parser.Comp_opContext> GetCompOpMock()
        {
            return GetMockRule<Python3Parser.Comp_opContext>();
        }

        [TestMethod]
        public void Visit_SingleExpr_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerContextMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerContextMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleExpr_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerContextMock = GetInnerMockWithSetup(expected);
            var compMock = GetCompOpMockWithSetup();

            contextMock.SetupChildren(
                innerContextMock.Object,
                compMock.Object,
                innerContextMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.That.IsBinaryOperator<CompareEquals>(expected, expected, result);
            contextMock.VerifyLoopedChildren(3);

            compMock.Verify();
            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
        
        [TestMethod]
        public void Visit_MultipleNYIComparison_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerContextMock = GetInnerMockWithSetup(expected);
            var compMock = GetCompOpMockWithSetup(ComparisonType.NotEqualsABC);

            contextMock.SetupChildren(
                innerContextMock.Object,
                compMock.Object,
                innerContextMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource, "<>");
            contextMock.VerifyLoopedChildren(3);

            compMock.Verify();
            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleOpsOrder_Test()
        {
            // Arrange
            var expected1 = GetExpressionMock();
            var expected2 = GetExpressionMock();
            var expected3 = GetExpressionMock();

            var innerRuleMock = GetInnerMock();

            ctorMock.SetupSequence(o => o.VisitExpr(innerRuleMock.Object))
                .Returns(expected1)
                .Returns(expected2)
                .Returns(expected3);

            var compMock = GetCompOpMockWithSetup();

            contextMock.SetupChildren(
                innerRuleMock.Object,
                compMock.Object,
                innerRuleMock.Object,
                compMock.Object,
                innerRuleMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Expect order ((1 op 2) op 3)
            // so it compiles left-to-right
            var lhs = Assert.That.IsBinaryOperatorGetLhs<CompareEquals>(
                expectedRhs: expected3, result);

            Assert.That.IsBinaryOperator<CompareEquals>(
                expectedLhs: expected1,
                expectedRhs: expected2, lhs);

            contextMock.VerifyLoopedChildren(5);

            ctorMock.Verify(o => o.VisitExpr(innerRuleMock.Object), Times.Exactly(3));
            compMock.Verify();
            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleMissingLhs_Test()
        {
            // Arrange
            var innerContextMock = GetInnerMock();
            var compMock = GetCompOpMock();

            compMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                compMock.Object,
                innerContextMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock,
                compMock.Object);
            // Should error already on 1st
            contextMock.VerifyLoopedChildren(1);

            compMock.Verify();
            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleMissingRhs_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerContextMock = GetInnerMockWithSetup(expected);
            var compMock = GetCompOpMock();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerContextMock.Object,
                compMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(2);

            compMock.Verify();
            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleInvalidCompRule_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerContextMock = GetInnerMockWithSetup(expected);
            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerContextMock.Object,
                unexpectedMock.Object,
                innerContextMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock,
                unexpectedMock.Object);
            // Should error already on 2nd
            contextMock.VerifyLoopedChildren(2);

            unexpectedMock.Verify();
            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}