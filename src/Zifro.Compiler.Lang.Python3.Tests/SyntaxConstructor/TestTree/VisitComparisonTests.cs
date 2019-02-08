using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitComparisonTests
        : BaseVisitTestClass<Python3Parser.ComparisonContext, Python3Parser.ExprContext>
    {
        public override ITerminalNode GetTerminalForThisClass()
        {
            throw new NotSupportedException("Comparisons do not have terminal node. Comparison operator does however!");
        }

        public override void SetupForInnerMock(Mock<Python3Parser.ExprContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitExpr(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override SyntaxNode VisitContext()
        {
            return ctor.VisitComparison(contextMock.Object);
        }

        [TestMethod]
        public void Visit_SingleExpr_Test()
        {
            // Arrange
            var expected = GetExpressionMock();
            var innerContextMock = GetInnerMockWithSetup(expected);

            contextMock.SetupChildren(
                innerContextMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expected, result);
            contextMock.VerifyLoopedChildren(1);

            innerContextMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}