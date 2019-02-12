using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;
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
            var expr1 = new LiteralInteger(SourceReference.ClrSource, 1);
            var expr2 = new LiteralInteger(SourceReference.ClrSource, 2);
            var opNode = (BinaryOperator)Activator.CreateInstance(operatorType, expr1, expr2);

            // Act
            opNode.Compile(compiler);

            // Assert
            Assert.That.IsPushLiteralOpCode<int>(1, compiler, index: 0);
            Assert.That.IsPushLiteralOpCode<int>(2, compiler, index: 1);
            Assert.That.IsBinaryOpCode(expectedType, compiler, index: 2);
            Assert.AreEqual(3, compiler.Count);
        }
    }
}