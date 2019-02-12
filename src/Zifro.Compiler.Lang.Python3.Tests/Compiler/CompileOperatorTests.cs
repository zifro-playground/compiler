using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;
using Zifro.Compiler.Lang.Python3.Syntax.Operators;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics;

namespace Zifro.Compiler.Lang.Python3.Tests.Compiler
{
    [TestClass]
    public class CompileOperatorTests
    {
        [DataTestMethod]
        [DataRow(typeof(ArithmeticAdd), typeof(AddOp), DisplayName = "op +")]
        [DataRow(typeof(ArithmeticSubtract), typeof(SubOp), DisplayName = "op -")]
        [DataRow(typeof(ArithmeticMultiply), typeof(MultOp), DisplayName = "op *")]
        [DataRow(typeof(ArithmeticDivide), typeof(DivOp), DisplayName = "op /")]
        [DataRow(typeof(ArithmeticFloor), typeof(FloorOp), DisplayName = "op //")]
        [DataRow(typeof(ArithmeticModulus), typeof(ModOp), DisplayName = "op %")]
        [DataRow(typeof(ArithmeticPower), typeof(PowOp), DisplayName = "op **")]
        public void CompileBinaryTests(Type operatorType, Type expectedType)
        {
            // Arrange
            var compiler = new PyCompiler();
            var exprMock = new Mock<ExpressionNode>(SourceReference.ClrSource);
            var opNode = (BinaryOperator)Activator.CreateInstance(operatorType,
                exprMock.Object, exprMock.Object);

            exprMock.Setup(o => o.Compile(compiler))
                .Verifiable();

            // Act
            opNode.Compile(compiler);

            // Assert
            Assert.That.IsBinaryOpCode(expectedType, compiler, index: 0);
            Assert.AreEqual(1, compiler.Count);

            exprMock.Verify(o => o.Compile(compiler), Times.Exactly(2));
        }
    }
}