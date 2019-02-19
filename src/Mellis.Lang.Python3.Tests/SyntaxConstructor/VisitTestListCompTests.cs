using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitTestListCompTests : BaseVisitClass<Python3Parser.Testlist_compContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTestlist_comp(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SingleTestTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object
            );

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            testMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleTestTrailingCommaTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object,
                GetTerminal(Python3Parser.COMMA)
            );

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(2);

            testMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleTestTooManyCommas()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();

            ITerminalNode unexpected = GetTerminal(Python3Parser.COMMA);

            contextMock.SetupChildren(
                unexpected,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                testMock.Object,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
            contextMock.VerifyLoopedChildren(1);

            testMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleStarTest()
        {
            // Arrange
            var starMock = GetMockRule<Python3Parser.Star_exprContext>();

            starMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                starMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            contextMock.VerifyLoopedChildren(1);

            starMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_NoTestsOnlyCommasTest()
        {
            // Arrange
            ITerminalNode unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                unexpected,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTestAndStarTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var starMock = GetMockRule<Python3Parser.Star_exprContext>();

            starMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(GetExpressionMock).Verifiable();

            contextMock.SetupChildren(
                testMock.Object,
                GetTerminal(Python3Parser.COMMA),
                starMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            // Should throw on 3rd, i.e. 2nd test
            contextMock.VerifyLoopedChildren(3);

            testMock.Verify();
            starMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleStarAndTestTest()
        {
            // Arrange
            var starMock = GetMockRule<Python3Parser.Star_exprContext>();
            var testMock = GetMockRule<Python3Parser.TestContext>();

            starMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                starMock.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            // Should throw on 1st
            contextMock.VerifyLoopedChildren(1);

            testMock.Verify();
            starMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTestsTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var secondTestMock = GetMockRule<Python3Parser.TestContext>();

            secondTestMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(GetExpressionMock).Verifiable();

            contextMock.SetupChildren(
                testMock.Object,
                GetTerminal(Python3Parser.COMMA),
                secondTestMock.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            // Should throw on 3rd, i.e. 2nd test
            contextMock.VerifyLoopedChildren(3);

            testMock.Verify();
            secondTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTestsNoTokensTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var secondTestMock = GetMockRule<Python3Parser.TestContext>();

            secondTestMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(GetExpressionMock).Verifiable();

            contextMock.SetupChildren(
                testMock.Object,
                secondTestMock.Object,
                testMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, secondTestMock.Object);
            contextMock.VerifyLoopedChildren(2);

            testMock.Verify();
            secondTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_CompForTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var compForMock = GetMockRule<Python3Parser.Comp_forContext>();

            compForMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            
            contextMock.SetupChildren(
                testMock.Object,
                compForMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "for");

            contextMock.VerifyLoopedChildren(2);

            testMock.Verify();
            compForMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_CompForTrailingCommaTest()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.TestContext>();
            var compForMock = GetMockRule<Python3Parser.Comp_forContext>();

            compForMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            var unexpected = GetTerminal(Python3Parser.COMMA);

            contextMock.SetupChildren(
                testMock.Object,
                compForMock.Object,
                unexpected
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
            contextMock.VerifyLoopedChildren(2);

            testMock.Verify();
            compForMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

    }
}