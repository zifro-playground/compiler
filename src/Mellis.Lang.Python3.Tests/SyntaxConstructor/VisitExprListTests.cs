using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitExprListTests : BaseVisitClass<Python3Parser.ExprlistContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitExprlist(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SingleExpr()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.ExprContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object
            );

            ctorMock.Setup(o => o.VisitExpr(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            var result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleStarExpr()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.Star_exprContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                testMock.Object
            );

            ctorMock.Setup(o => o.VisitStar_expr(testMock.Object))
                .Returns(expected).Verifiable();

            testMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "*");
        }

        [TestMethod]
        public void Visit_SingleTest_TrailingComma_BecomesTuple()
        {
            // Arrange
            var testMock = GetMockRule<Python3Parser.ExprContext>();
            var expected = GetExpressionMock();

            var unexpected = GetTerminal(Python3Parser.COMMA);

            contextMock.SetupChildren(
                testMock.Object,
                unexpected
            );

            ctorMock.Setup(o => o.VisitExpr(testMock.Object))
                .Returns(expected).Verifiable();

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, ",");
        }

        [TestMethod]
        public void Visit_MultipleExpr()
        {
            // Arrange
            var testMock1 = GetMockRule<Python3Parser.ExprContext>();
            var testMock2 = GetMockRule<Python3Parser.ExprContext>();
            var testMock3 = GetMockRule<Python3Parser.ExprContext>();

            var unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                testMock1.Object,
                unexpected,
                testMock2.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock3.Object
            );

            ctorMock.Setup(o => o.VisitExpr(testMock1.Object))
                .Returns(GetExpressionMock).Verifiable();

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, ",");

            testMock2.Verify();
        }

        [TestMethod]
        public void Visit_MultipleExprStarredExpr()
        {
            // Arrange
            var mock1 = GetMockRule<Python3Parser.ExprContext>();
            var mock2 = GetMockRule<Python3Parser.Star_exprContext>();
            var mock3 = GetMockRule<Python3Parser.ExprContext>();

            var unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                mock1.Object,
                unexpected,
                mock2.Object,
                GetTerminal(Python3Parser.COMMA),
                mock3.Object
            );

            ctorMock.Setup(o => o.VisitExpr(mock1.Object))
                .Returns(GetExpressionMock).Verifiable();

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, ",");

            mock2.Verify();
        }

        //[TestMethod]
        //public void Visit_MultipleExprTooManyCommas()
        //{
        //    // Arrange
        //    var testMock1 = GetMockRule<Python3Parser.ExprContext>();
        //    var testMock2 = GetMockRule<Python3Parser.ExprContext>();
        //    var testMock3 = GetMockRule<Python3Parser.ExprContext>();

        //    var unexpected = GetTerminal(Python3Parser.COMMA);
        //    contextMock.SetupChildren(
        //        testMock1.Object,
        //        GetTerminal(Python3Parser.COMMA),
        //        unexpected,
        //        testMock2.Object,
        //        GetTerminal(Python3Parser.COMMA),
        //        testMock3.Object
        //    );

        //    ctorMock.Setup(o => o.VisitExpr(testMock1.Object))
        //        .Returns(GetExpressionMock).Verifiable();

        //    // Act
        //    var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

        //    // Assert
        //    Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
        //}

        [TestMethod]
        public void Visit_MultipleExprNoCommas()
        {
            // Arrange
            var testMock1 = GetMockRule<Python3Parser.ExprContext>();
            var testMock2 = GetMockRule<Python3Parser.ExprContext>();
            var testMock3 = GetMockRule<Python3Parser.ExprContext>();

            var unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                testMock1.Object,
                testMock2.Object, // expecting comma, got test
                testMock3.Object
            );

            ctorMock.Setup(o => o.VisitExpr(testMock1.Object))
                .Returns(GetExpressionMock).Verifiable();

            testMock2.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock,
                testMock2.Object);
        }

        [TestMethod]
        public void Visit_NoTestsOnlyCommas()
        {
            // Arrange
            var unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                unexpected,
                GetTerminal(Python3Parser.COMMA),
                GetTerminal(Python3Parser.COMMA)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
        }
    }
}