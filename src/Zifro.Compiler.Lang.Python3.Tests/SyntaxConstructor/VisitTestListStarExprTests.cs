using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitTestListStarExprTests : BaseVisitClass
    {

        [TestMethod]
        public void Visit_SingleTestTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var testMock = GetMockRule<Python3Parser.TestContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object
            );

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = ctor.VisitTestlist_star_expr(contextMock.Object);

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
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var testMock = GetMockRule<Python3Parser.TestContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object,
                GetTerminal(Python3Parser.COMMA)
            );

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = ctor.VisitTestlist_star_expr(contextMock.Object);

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
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var testMock = GetMockRule<Python3Parser.TestContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                testMock.Object,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA)
            );

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = ctor.VisitTestlist_star_expr(contextMock.Object);

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(8);

            testMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleStarTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var starMock = GetMockRule<Python3Parser.Star_exprContext>();

            starMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                starMock.Object
            );

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            contextMock.VerifyLoopedChildren(1);

            starMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_NoChildrenTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_NoTestsOnlyCommasTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA)
            );

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(3);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTestAndStarTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

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

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);

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
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var starMock = GetMockRule<Python3Parser.Star_exprContext>();
            var testMock = GetMockRule<Python3Parser.TestContext>();

            starMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                starMock.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock.Object
            );

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);

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
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

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

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);

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
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

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

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            contextMock.VerifyLoopedChildren(2);

            testMock.Verify();
            secondTestMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTestsTooManyCommasTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var testMock = GetMockRule<Python3Parser.TestContext>();
            var secondTestMock = GetMockRule<Python3Parser.TestContext>();

            secondTestMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(GetExpressionMock).Verifiable();

            contextMock.SetupChildren(
                testMock.Object,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                secondTestMock.Object,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA),
                testMock.Object,
                GetTerminal(Python3Parser.COMMA)
            );

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            // Should throw on 5th, i.e. 2nd test
            contextMock.VerifyLoopedChildren(5);

            testMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidTokenTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var testMock = GetMockRule<Python3Parser.TestContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object,
                // Should just ignore it
                GetTerminal(Python3Parser.ASYNC)
            );

            ctorMock.Setup(o => o.VisitTest(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            SyntaxNode result = ctor.VisitTestlist_star_expr(contextMock.Object);

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidRuleTest()
        {
            // Arrange
            var contextMock = GetMockRule<Python3Parser.Testlist_star_exprContext>();

            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                unexpectedMock.Object
            );

            Action action = delegate { ctor.VisitTestlist_star_expr(contextMock.Object); };

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(action);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            contextMock.VerifyLoopedChildren(1);

            unexpectedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

    }
}