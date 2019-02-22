using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitTrailerTests : BaseVisitClass<
        Python3Parser.TrailerContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTrailer(contextMock.Object);
        }

        [TestMethod]
        public void TestNonClosingParentheses()
        {
            // Arrange
            ITerminalNode opening = GetTerminal(Python3Parser.OPEN_PAREN);
            contextMock.SetupChildren(
                opening
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Parenthesis_NoClosing),
                opening,
                ")"
            );
        }

        [TestMethod]
        public void TestNonClosingBrackets()
        {
            // Arrange
            ITerminalNode opening = GetTerminal(Python3Parser.OPEN_BRACK);
            contextMock.SetupChildren(
                opening
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Parenthesis_NoClosing),
                opening,
                "]"
            );
        }

        [TestMethod]
        public void TestNoSubscriptionListInBrackets()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_BRACK),
                GetTerminal(Python3Parser.CLOSE_BRACK)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock,
                contextMock
            );
        }

        [TestMethod]
        public void TestNoNameAfterDot()
        {
            // Arrange
            ITerminalNode dot = GetTerminal(Python3Parser.DOT);
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                dot
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorExpectedChildFormatArgs(ex,
                startTokenMock, stopTokenMock,
                contextMock
            );
        }

        [TestMethod]
        public void TestFunctionCallIsCreated()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentsList));
            Assert.AreEqual(0, ((ArgumentsList) result).Count);
        }

        [TestMethod]
        public void TestFunctionCallWithArguments()
        {
            // Arrange

            var argListMock = GetMockRule<Python3Parser.ArglistContext>();
            var funcArgs = GetFunctionArgumentsMock();
            ctorMock.Setup(o => o.VisitArglist(argListMock.Object))
                .Returns(funcArgs).Verifiable();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                argListMock.Object,
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentsList));
            var resultArgs = (ArgumentsList) result;
            Assert.AreSame(funcArgs, resultArgs);
        }

        [TestMethod]
        public void TestIndexingNotYetImplemented()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_BRACK),
                GetMockRule<Python3Parser.SubscriptlistContext>().Object,
                GetTerminal(Python3Parser.CLOSE_BRACK)
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex,
                startTokenMock, stopTokenMock,
                "[]"
            );
        }

        [TestMethod]
        public void TestPropertyNotYetImplemented()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.DOT),
                GetTerminal(Python3Parser.NAME, "foo")
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex,
                startTokenMock, stopTokenMock,
                "."
            );
        }
    }
}