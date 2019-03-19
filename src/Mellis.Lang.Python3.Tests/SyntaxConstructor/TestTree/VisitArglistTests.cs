using System.Linq;
using Antlr4.Runtime.Tree;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitArglistTests : BaseVisitTestClass<
        Python3Parser.ArglistContext,
        Python3Parser.ArgumentContext
    >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitArglist(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.ArgumentContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitArgument(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.COMMA);
        }

        protected Mock<Python3Parser.ArgumentContext>[] SetupCommaSeparatedChildren(
            bool trailingComma,
            params ExpressionNode[] arguments)
        {
            int count = trailingComma
                ? arguments.Length * 2
                : arguments.Length * 2 - 1;

            var args = new IParseTree[count];
            var argMocks = new Mock<Python3Parser.ArgumentContext>[arguments.Length];

            for (var i = 0; i < arguments.Length; i++)
            {
                ExpressionNode arg = arguments[i];
                var argMock = GetInnerMockWithSetup(arg);

                args[i * 2] = argMock.Object;

                if (i < arguments.Length - 1 || trailingComma)
                {
                    args[i * 2 + 1] = GetTerminal(Python3Parser.COMMA);
                }

                argMocks[i] = argMock;
            }

            contextMock.SetupChildren(
                args
            );

            return argMocks;
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "With trailing comma")]
        [DataRow(false, DisplayName = "Without trailing comma")]
        public void Visit_ArgumentList_Test(bool trailingComma)
        {
            // Arrange
            var expr1 = GetExpressionMock();
            var expr2 = GetExpressionMock();
            var expr3 = GetExpressionMock();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            var innerMocks = SetupCommaSeparatedChildren(
                trailingComma,
                expr1,
                expr2,
                expr3
            ).Cast<Mock>().ToArray();

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentsList));
            var resultArgs = (ArgumentsList) result;
            Assert.AreEqual(3, resultArgs.Count);
            Assert.AreSame(expr1, resultArgs[0]);
            Assert.AreSame(expr2, resultArgs[1]);
            Assert.AreSame(expr3, resultArgs[2]);

            contextMock.Verify();
            Mock.VerifyAll(innerMocks);
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "With trailing comma")]
        [DataRow(false, DisplayName = "Without trailing comma")]
        public void Visit_SingleArgument_Test(bool trailingComma)
        {
            // Arrange
            var expr = GetExpressionMock();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            var innerMocks = SetupCommaSeparatedChildren(
                trailingComma,
                expr
            ).Cast<Mock>().ToArray();

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ArgumentsList));
            var resultArgs = (ArgumentsList)result;
            Assert.AreEqual(1, resultArgs.Count);
            Assert.AreSame(expr, resultArgs[0]);

            contextMock.Verify();
            Mock.VerifyAll(innerMocks);
        }

        [TestMethod]
        public void Visit_TooManyCommas_Test()
        {
            // Arrange
            var unexpected = GetTerminalForThisClass();
            var innerMock = GetInnerMockWithSetup(GetExpressionMock());

            contextMock.SetupChildren(
                innerMock.Object,
                GetTerminalForThisClass(),
                unexpected
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex,
                contextMock, unexpected);

            innerMock.Verify();
            contextMock.Verify();
        }
    }
}