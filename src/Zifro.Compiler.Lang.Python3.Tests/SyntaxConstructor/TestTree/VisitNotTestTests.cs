using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitNotTestTests
        : BaseVisitTestClass<Python3Parser.Not_testContext, Python3Parser.ComparisonContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitNot_test(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.ComparisonContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitComparison(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public void SetupForNotMock(Mock<Python3Parser.Not_testContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitNot_test(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            return GetTerminal(Python3Parser.NOT);
        }

        [TestMethod]
        public void Visit_SingleComparision_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMock();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                innerRuleMock.Object
            );

            SetupForInnerMock(innerRuleMock, expected);

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_MultipleComparisions_Test()
        {
            // Arrange
            var innerRuleMock = GetInnerMock();
            var unexpectedMock = GetInnerMock();

            contextMock.SetupChildren(
                innerRuleMock.Object,
                unexpectedMock.Object
            );

            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock,
                unexpectedMock.Object);
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            unexpectedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_SingleNotTest_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.Not_testContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                GetTerminalForThisClass(),
                innerRuleMock.Object
            );

            SetupForNotMock(innerRuleMock, expected);
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.That.IsUnaryOperator<LogicalNot>(expected, result);
            
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidNotExcessLeadingToken_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.Not_testContext>();

            ITerminalNode unexpected = GetTerminalForThisClass();
            contextMock.SetupChildren(
                GetTerminalForThisClass(),
                unexpected,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
            // Should throw when reached 2nd
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidNotExcessTrailingToken_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.Not_testContext>();

            ITerminalNode unexpected = GetTerminalForThisClass();
            contextMock.SetupChildren(
                GetTerminalForThisClass(),
                innerRuleMock.Object,
                unexpected
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
            contextMock.VerifyLoopedChildren(3);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidNotExcessLeadingRule_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.Not_testContext>();
            var unexpectedMock = GetMockRule<Python3Parser.Not_testContext>();

            contextMock.SetupChildren(
                unexpectedMock.Object,
                GetTerminalForThisClass(),
                innerRuleMock.Object
            );

            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);
            contextMock.VerifyLoopedChildren(1);

            unexpectedMock.Verify();
            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidNotExcessTrailingRule_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.Not_testContext>();
            var unexpectedMock = GetMockRule<Python3Parser.Not_testContext>();

            contextMock.SetupChildren(
                GetTerminalForThisClass(),
                innerRuleMock.Object,
                unexpectedMock.Object
            );

            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);
            contextMock.VerifyLoopedChildren(3);

            unexpectedMock.Verify();
            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidNotWrongToken_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.Not_testContext>();

            ITerminalNode unexpected = GetTerminal(Python3Parser.ASYNC);
            contextMock.SetupChildren(
                unexpected,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpected);
            // Should throw when reached 1st
            contextMock.VerifyLoopedChildren(1);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidNotWrongRule_Test()
        {
            // Arrange
            var unexpectedMock = GetMockRule<Python3Parser.ComparisonContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminalForThisClass(),
                unexpectedMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);
            contextMock.VerifyLoopedChildren(2);

            unexpectedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}