using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyBooleanClassTests
    {
        private static PyProcessor ProcessorWithCall(int numArgs = 0)
        {
            return new PyProcessor(
                new Call(SourceReference.ClrSource, numArgs, 1)
            );
        }

        [TestMethod]
        public void CtorEmptyArgsTest()
        {
            // Arrange
            var processor = ProcessorWithCall();
            processor.PushValue(new PyBooleanType(processor));

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.That.ScriptTypeEqual(expected: false, actual: result);
        }

        [TestMethod]
        public void CtorTooManyArgs()
        {
            // Arrange
            var processor = ProcessorWithCall(numArgs: 2);
            processor.PushValue(new PyBooleanType(processor));
            processor.PushMockValue(); // arg 1
            processor.PushMockValue(); // arg 2

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<RuntimeTooManyArgumentsException>((Action) processor.WalkInstruction);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                /* func name */ Localized_Base_Entities.Type_Boolean_Name,
                /* maximum */ 1,
                /* actual */ 2);
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "arg0.isTruthy=>true")]
        [DataRow(false, DisplayName = "arg0.isTruthy=>false")]
        public void CtorOneTruthy(bool truthy)
        {
            // Arrange
            var argMock = new Mock<IScriptType>();
            argMock.Setup(o => o.IsTruthy())
                .Returns(truthy).Verifiable();

            var processor = ProcessorWithCall(numArgs: 1);
            processor.PushValue(new PyBooleanType(processor));
            processor.PushValue(argMock.Object); // arg 1

            // Act
            processor.WalkInstruction(); // warmup
            processor.WalkInstruction();

            // Assert
            var result = processor.PopValue();
            Assert.That.ScriptTypeEqual(expected: truthy, actual: result);
        }
    }
}