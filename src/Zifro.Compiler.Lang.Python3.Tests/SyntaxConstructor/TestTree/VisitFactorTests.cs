using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitFactorTests : BaseVisitTestClass<Python3Parser.FactorContext, Python3Parser.PowerContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitFactor(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.PowerContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitPower(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            throw new NotSupportedException("Not applicable");
        }

        [DataTestMethod]
        [DataRow(Python3Parser.ADD, typeof(ArithmeticPositive), DisplayName = "valid factor +x")]
        [DataRow(Python3Parser.MINUS, typeof(ArithmeticNegative), DisplayName = "valid factor -x")]
        [DataRow(Python3Parser.NOT_OP, typeof(BinaryNot), DisplayName = "valid factor ~x")]
        public void Visit_ValidFactor_Test(int factorToken, Type expectedType)
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.FactorContext>();

            ExpressionNode expr = GetExpressionMock();
            ctorMock.Setup(o => o.VisitFactor(innerMock.Object))
                .Returns(expr).Verifiable();

            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(factorToken),
                innerMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, expectedType);
            Assert.That.IsUnaryOperator(expectedType, expr, result);
            
            contextMock.Verify();
            Assert.AreEqual(contextMock.Object.GetSourceReference(), result.Source);

            contextMock.VerifyLoopedChildren(2);

            innerMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.STAR, DisplayName = "invalid factor *x")]
        [DataRow(Python3Parser.AT, DisplayName = "invalid factor @x")]
        public void Visit_InvalidFactorType_Test(int factorToken)
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.FactorContext>();
            ITerminalNode unexpectedToken = GetTerminal(factorToken);

            contextMock.SetupChildren(
                unexpectedToken,
                innerMock.Object
            );

            // Act + Arrange
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedToken);

            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidFactorExcess_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.FactorContext>();
            ITerminalNode unexpectedToken = GetTerminal(Python3Parser.ADD);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.ADD),
                unexpectedToken,
                innerMock.Object
            );

            // Act + Arrange
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, unexpectedToken);

            // Should error on 2nd
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidFactorNoToken_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.FactorContext>();
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                innerMock.Object
            );

            // Act + Arrange
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, innerMock.Object);

            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidFactorPowerRule_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.PowerContext>();
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.ADD),
                innerMock.Object
            );

            // Act + Arrange
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, innerMock.Object);

            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidFactorTooManyRules_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.FactorContext>();
            var unexpectedMock = GetInnerMock();


            contextMock.SetupChildren(
                GetTerminal(Python3Parser.ADD),
                innerMock.Object,
                unexpectedMock.Object
            );

            // Act + Arrange
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            // Assume error without even looking at it since it's too many for the grammar
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_ValidPower_Test()
        {
            // Arrange
            var innerMock = GetInnerMock();

            ExpressionNode expr = GetExpressionMock();
            SetupForInnerMock(innerMock, expr);

            contextMock.SetupChildren(
                innerMock.Object
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.AreSame(expr, result);

            contextMock.VerifyLoopedChildren(1);
            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidPowerTooMany_Test()
        {
            // Arrange
            var innerMock = GetInnerMock();
            var unexpectedMock = GetInnerMock();


            contextMock.SetupChildren(
                innerMock.Object,
                unexpectedMock.Object
            );

            // Act + Arrange
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            innerMock.Verify();
            ctorMock.Verify();
        }
    }
}