using Antlr4.Runtime.Tree;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitArgumentTests : BaseVisitTestClass<
        Python3Parser.ArgumentContext,
        Python3Parser.TestContext
    >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitArgument(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.TestContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitTest(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.ASSIGN);
        }

        [TestMethod]
        public void Visit_SingleTest_Test()
        {
            // Arrange
            var expr = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(expr);

            contextMock.SetupChildren(
                innerMock.Object    
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expr, result);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_CompForTestNyi_Test()
        {
            // Arrange
            var innerMock = GetInnerMock();
            var unexpectedMock = GetMockRule<Python3Parser.Comp_forContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                unexpectedMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex,
                startTokenMock, stopTokenMock,
                "for"
            );

            unexpectedMock.Verify();
            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidMultipleTests_Test()
        {
            // Arrange
            var innerMock = GetInnerMock();
            var unexpectedMock = GetInnerMock();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object,
                unexpectedMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex,
                startTokenMock, stopTokenMock,
                contextMock, unexpectedMock.Object);

            unexpectedMock.Verify();
            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_KeywordArgTestNyi_Test()
        {
            // Arrange
            var expr = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(expr);
            ITerminalNode unexpected = GetTerminal(Python3Parser.ASSIGN);

            contextMock.SetupChildren(
                innerMock.Object,
                unexpected,
                innerMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, "=");

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_KeywordUnpackingNyi_Test()
        {
            // Arrange
            var expr = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(expr);
            ITerminalNode unexpected = GetTerminal(Python3Parser.POWER);

            contextMock.SetupChildren(
                unexpected,
                innerMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, "**");

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_IterableUnpackingNyi_Test()
        {
            // Arrange
            var expr = GetExpressionMock();
            var innerMock = GetInnerMockWithSetup(expr);
            ITerminalNode unexpected = GetTerminal(Python3Parser.STAR);

            contextMock.SetupChildren(
                unexpected,
                innerMock.Object
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, unexpected, "*");

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}