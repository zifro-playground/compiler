using System;
using Mellis.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class CallStackTests
    {
        [TestMethod]
        public void ReachStackOverflowTest()
        {
            // Arrange
            var processor = new PyProcessor();
            var callStack = new CallStack(
                SourceReference.ClrSource,
                (PyScope) processor.CurrentScope,
                0
            );

            for (var i = 0; i < PyProcessor.CALL_STACK_LIMIT; i++)
            {
                processor.PushCallStack(callStack);
            }

            void Action()
            {
                processor.PushCallStack(callStack);
            }

            // Act + Assert
            Assert.ThrowsException<RuntimeStackOverflowException>((Action) Action);
        }

        [TestMethod]
        public void AddNullTest()
        {
            // Arrange
            var processor = new PyProcessor();

            void Action()
            {
                processor.PushCallStack(null);
            }

            // Act
            var ex = Assert.ThrowsException<InternalException>((Action) Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_CallStack_PushNull));
        }

        [TestMethod]
        public void PopEmptyTest()
        {
            // Arrange
            var processor = new PyProcessor();

            void Action()
            {
                processor.PopCallStack();
            }

            // Act
            var ex = Assert.ThrowsException<InternalException>((Action) Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Interpreter.Ex_CallStack_PopEmpty));
        }
    }
}