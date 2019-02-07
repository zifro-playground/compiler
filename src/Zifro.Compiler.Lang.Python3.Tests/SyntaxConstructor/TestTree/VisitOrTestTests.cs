using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitOrTestTests : BaseVisitTestClass<Python3Parser.Or_testContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitOr_test(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SingleAnd_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.And_testContext>();
            var expected = GetExpressionMock();

            contextMock.SetupChildren(
                innerRuleMock.Object
            );

            ctorMock.Setup(o => o.VisitAnd_test(innerRuleMock.Object))
                .Returns(expected).Verifiable();

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
        public void Visit_MultipleAndSingleOrToken_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.And_testContext>();
            
            ITerminalNode orNode = GetTerminal(Python3Parser.OR);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                orNode,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, orNode, "or");
            // Suppose error directly on IF token
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidOrMissingRhs_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.And_testContext>();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                GetTerminal(Python3Parser.OR)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);
            contextMock.VerifyLoopedChildren(2);

            innerRuleMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidOrInvalidTokens_Test()
        {
            // Arrange
            var innerRuleMock = GetMockRule<Python3Parser.And_testContext>();

            ITerminalNode unexpectedNode = GetTerminal(Python3Parser.ASYNC);

            contextMock.SetupChildren(
                innerRuleMock.Object,
                unexpectedNode,
                innerRuleMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedNode);
            // Suppose error directly on token
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}