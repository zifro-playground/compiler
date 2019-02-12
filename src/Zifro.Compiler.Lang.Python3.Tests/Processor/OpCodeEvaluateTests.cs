using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class OpCodeEvaluateTests
    {
        [TestMethod]
        public void EvaluateBinary_Add_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Add,
                o => o.ArithmeticAdd(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Sub_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Sub,
                o => o.ArithmeticSubtract(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Mul_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Mul,
                o => o.ArithmeticMultiply(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Div_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Div,
                o => o.ArithmeticDivide(It.IsAny<IScriptType>()));
        }


        [TestMethod]
        public void EvaluateBinary_Flr_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Flr,
                o => o.ArithmeticFloorDivide(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Mod_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Mod,
                o => o.ArithmeticModulus(It.IsAny<IScriptType>()));
        }

        [TestMethod]
        public void EvaluateBinary_Pow_Test()
        {
            EvaluateBinaryTestTemplate(OperatorCode.Pow,
                o => o.ArithmeticExponent(It.IsAny<IScriptType>()));
        }

        protected void EvaluateBinaryTestTemplate(OperatorCode opCode, Expression<Func<IScriptType, IScriptType>> method)
        {
            // Arrange
            var processor = new PyProcessor(
                new OpBinOpCode(SourceReference.ClrSource, opCode)
            );

            var valueMock = new Mock<IScriptType>();
            var resultMock = new Mock<IScriptType>();

            valueMock.Setup(method).Returns(resultMock.Object);

            processor.PushValue(valueMock.Object);
            processor.PushValue(valueMock.Object);

            // Act
            processor.WalkLine();
            var result = processor.PopValue<IScriptType>();
            int numOfValues = processor.ValueStackCount;

            // Assert
            Assert.AreEqual(1, numOfValues, "Did not absorb values.");
            Assert.AreSame(resultMock.Object, result);
            valueMock.Verify(method);
        }
    }
}