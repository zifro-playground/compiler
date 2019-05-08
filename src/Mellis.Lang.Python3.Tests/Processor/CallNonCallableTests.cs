using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Mellis.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class CallNonCallableTests
    {
        [TestMethod]
        public void CallNoValueTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1)
            );

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<InternalException>(delegate { processor.WalkInstruction(); });

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_ValueStack_PopEmpty)
            );
        }

        [TestMethod]
        public void CallIntegerTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Call(SourceReference.ClrSource, 0, 1)
            );

            var value = new PyInteger(processor, 0);

            processor.PushValue(value);

            // Act
            processor.WalkInstruction(); // warmup
            var ex = Assert.ThrowsException<RuntimeException>(delegate { processor.WalkInstruction(); });

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Base_Entities.Ex_Base_Invoke),
                value.GetTypeName()
            );
        }
    }
}