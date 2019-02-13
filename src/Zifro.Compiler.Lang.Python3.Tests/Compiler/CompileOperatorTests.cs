using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
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
        [DataRow(typeof(ArithmeticAdd), OperatorCode.AAdd, DisplayName = "comp op +")]
        [DataRow(typeof(ArithmeticSubtract), OperatorCode.ASub, DisplayName = "comp op -")]
        [DataRow(typeof(ArithmeticMultiply), OperatorCode.AMul, DisplayName = "comp op *")]
        [DataRow(typeof(ArithmeticDivide), OperatorCode.ADiv, DisplayName = "comp op /")]
        [DataRow(typeof(ArithmeticFloor), OperatorCode.AFlr, DisplayName = "comp op //")]
        [DataRow(typeof(ArithmeticModulus), OperatorCode.AMod, DisplayName = "comp op %")]
        [DataRow(typeof(ArithmeticPower), OperatorCode.APow, DisplayName = "comp op **")]
        public void CompileBinaryTests(Type operatorType, OperatorCode expectedCode)
        {
            // Arrange
            var compiler = new PyCompiler();

            var exprMock = new Mock<ExpressionNode>(SourceReference.ClrSource);
            exprMock.Setup(o => o.Compile(compiler))
                .Verifiable();

            var opNode = (BinaryOperator)Activator.CreateInstance(operatorType,
                exprMock.Object, exprMock.Object);

            // Act
            opNode.Compile(compiler);

            // Assert
            Assert.That.IsBinaryOpCode(expectedCode, compiler, index: 0);
            Assert.AreEqual(1, compiler.Count);

            exprMock.Verify(o => o.Compile(compiler), Times.Exactly(2));
        }

    }
}