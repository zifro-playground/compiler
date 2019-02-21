using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitTrailerParenthesesTests : BaseVisitClass<
        Python3Parser.TrailerContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTrailer(contextMock.Object);
        }

        protected Mock<Python3Parser.ArglistContext> GetArglistMock()
        {
            return GetMockRule<Python3Parser.ArglistContext>();
        }

        protected void SetupForArglist(Mock<Python3Parser.ArglistContext> innerMock, params ExpressionNode[] arguments)
        {
            var args = new IParseTree[arguments.Length * 2];

            for (var i = 0; i < arguments.Length; i++)
            {
                ExpressionNode arg = arguments[i];
                var argMock = GetMockRule<Python3Parser.ArgumentContext>();

                ctorMock.Setup(o => o.VisitArgument(argMock.Object))
                    .Returns(arg).Verifiable();

                args[i * 2] = argMock.Object;
                args[i * 2 + 1] = GetTerminal(Python3Parser.COMMA);
            }

            innerMock.SetupChildren(
                args
            );
        }

        public ITerminalNode GetOpenTerminal()
        {
            return GetTerminal(Python3Parser.OPEN_PAREN);
        }

        public ITerminalNode GetCloseTerminal()
        {
            return GetTerminal(Python3Parser.CLOSE_PAREN);
        }

        [TestMethod]
        public void TestNonClosingParentheses()
        {
            // Arrange
            contextMock.SetupChildren(
                GetOpenTerminal()
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Parenthesis_NoClosing),
                startTokenMock.Object, stopTokenMock.Object,
                ")"
            );
        }

        [TestMethod]
        public void TestFunctionCallIsCreated()
        {
            // Arrange
            contextMock.SetupChildren(
                GetOpenTerminal(),
                GetCloseTerminal()
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(FunctionCall));
            Assert.AreEqual(0, ((FunctionCall) result).Arguments.Count);
        }

        [TestMethod]
        public void TestFunctionCallWithArguments()
        {
            // Arrange
            var arg1 = GetExpressionMock();
            var arg2 = GetExpressionMock();
            var arg3 = GetExpressionMock();

            var arglistMock = GetArglistMock();
            SetupForArglist(arglistMock,
                arg1, arg2, arg3
            );

            contextMock.SetupChildren(
                GetOpenTerminal(),
                arglistMock.Object,
                GetCloseTerminal()
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(FunctionCall));
            var call = ((FunctionCall) result);
            Assert.AreSame(arg1, call.Arguments[0]);
            Assert.AreSame(arg2, call.Arguments[1]);
            Assert.AreSame(arg3, call.Arguments[2]);
            Assert.AreEqual(3, call.Arguments.Count);
        }
    }
}