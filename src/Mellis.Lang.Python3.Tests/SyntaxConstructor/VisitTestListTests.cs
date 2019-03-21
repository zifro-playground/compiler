using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitTestListTests : BaseVisitClass<Python3Parser.TestlistContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitTestlist(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SingleTest()
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

            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTests()
        {
            // Arrange
            var testMock1 = GetMockRule<Python3Parser.TestContext>();
            var testMock2 = GetMockRule<Python3Parser.TestContext>();
            var testMock3 = GetMockRule<Python3Parser.TestContext>();

            contextMock.SetupChildren(
                testMock1.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock2.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock3.Object
            );

            ctorMock.Setup(o => o.VisitTest(testMock1.Object))
                .Returns(GetExpressionMock).Verifiable();

            testMock2.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock);

            testMock2.Verify();
        }

        [TestMethod]
        public void Visit_MultipleTestsTooManyCommas()
        {
            // Arrange
            var testMock1 = GetMockRule<Python3Parser.TestContext>();
            var testMock2 = GetMockRule<Python3Parser.TestContext>();
            var testMock3 = GetMockRule<Python3Parser.TestContext>();

            var unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                testMock1.Object,
                GetTerminal(Python3Parser.COMMA),
                unexpected,
                testMock2.Object,
                GetTerminal(Python3Parser.COMMA),
                testMock3.Object
            );

            ctorMock.Setup(o => o.VisitTest(testMock1.Object))
                .Returns(GetExpressionMock).Verifiable();

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
        }

        [TestMethod]
        public void Visit_MultipleTestsNoCommas()
        {
            // Arrange
            var testMock1 = GetMockRule<Python3Parser.TestContext>();
            var testMock2 = GetMockRule<Python3Parser.TestContext>();
            var testMock3 = GetMockRule<Python3Parser.TestContext>();

            var unexpected = GetTerminal(Python3Parser.COMMA);
            contextMock.SetupChildren(
                testMock1.Object,
                testMock2.Object, // expecting comma, got test
                testMock3.Object
            );

            ctorMock.Setup(o => o.VisitTest(testMock1.Object))
                .Returns(GetExpressionMock).Verifiable();

            testMock2.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            // Assert
            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, testMock2.Object);
            contextMock.VerifyLoopedChildren(2);
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
            contextMock.VerifyLoopedChildren(1);
        }
    }
}