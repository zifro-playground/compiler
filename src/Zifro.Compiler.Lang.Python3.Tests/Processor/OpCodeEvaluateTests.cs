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

            var lhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var rhsMock = new Mock<IScriptType>(MockBehavior.Strict);
            var resultMock = new Mock<IScriptType>();

            lhsMock.Setup(method).Returns(resultMock.Object);

            processor.PushValue(lhsMock.Object);
            processor.PushValue(rhsMock.Object);

            // Act
            processor.WalkLine();
            int numOfValues = processor.ValueStackCount;
            var result = processor.PopValue<IScriptType>();

            // Assert
            Assert.IsNull(processor.LastError, "Last error <{0}>:{1}", processor.LastError?.GetType().Name, processor.LastError?.Message);

            Assert.AreEqual(1, numOfValues, "Did not absorb values.");
            Assert.AreSame(resultMock.Object, result, "Did not produce result.");
            lhsMock.Verify(method);
        }
    }
}