using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    public abstract class BaseVisitTestClass<T> : BaseVisitClass<T> where T : ParserRuleContext
    {
        [TestMethod]
        public void Visit_InvalidToken_Test()
        {
            // Arrange
            ITerminalNode unexpectedNode = GetTerminal(Python3Parser.ASYNC);
            contextMock.SetupChildren(
                unexpectedNode
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidRule_Test()
        {
            // Arrange
            var unexpectedRule = GetMockRule<Python3Parser.File_inputContext>();

            unexpectedRule.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                unexpectedRule.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedRule.Object);
            contextMock.VerifyLoopedChildren(1);

            unexpectedRule.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_NoChildren_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}