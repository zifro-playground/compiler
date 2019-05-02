using System;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Instructions
{
    [TestClass]
    public class BreakpointShouldBreakTests
    {
        [DataTestMethod]
        [DataRow(BreakCause.LoopBlockEnd)]
        [DataRow(BreakCause.LoopEnter)]
        public void AlwaysBreaks(BreakCause breakCause)
        {
            // Arrange
            var processor = new PyProcessor();
            var breakpoint = new Breakpoint(breakCause);

            // Act
            bool result = breakpoint.ShouldBreak(processor, null);

            // Assert
            Assert.IsTrue(result);
        }

        private static Call PushCallArgsStack(PyProcessor processor, int argumentCount)
        {
            var call = new Call(SourceReference.ClrSource, argumentCount, returnAddress: 0);

            var arg = Mock.Of<IScriptType>();
            for (int i = 0; i < argumentCount; i++)
            {
                processor.PushValue(arg);
            }

            return call;
        }

        [DataTestMethod]
        // Cannot test if user function yet due to it not being implemented.
        [DataRow(BreakCause.FunctionAnyCall, 0, true)]
        [DataRow(BreakCause.FunctionAnyCall, 3, true)]
        [DataRow(BreakCause.FunctionClrCall, 0, true)]
        [DataRow(BreakCause.FunctionClrCall, 3, true)]
        [DataRow(BreakCause.FunctionUserCall, 0, false)] // assuming function cant be both Clr and User
        [DataRow(BreakCause.FunctionUserCall, 3, false)] // assuming function cant be both Clr and User
        public void FunctionBreaksOnClr(BreakCause breakCause, int args, bool expected)
        {
            // Arrange
            var valueMock = new Mock<IScriptType>();
            valueMock.As<IClrFunction>();

            var processor = new PyProcessor();
            processor.PushValue(valueMock.Object);

            var call = PushCallArgsStack(processor, args);
            var breakpoint = new Breakpoint(breakCause);

            // Act
            bool result = breakpoint.ShouldBreak(processor, call);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(3)]
        public void FunctionClrNotBreaksOnNonClr(int args)
        {
            // Arrange
            var valueMock = new Mock<IScriptType>();

            var processor = new PyProcessor();
            processor.PushValue(valueMock.Object);

            var call = PushCallArgsStack(processor, args);
            var breakpoint = new Breakpoint(BreakCause.FunctionClrCall);

            // Act
            bool result = breakpoint.ShouldBreak(processor, call);

            // Assert
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(BreakCause.FunctionAnyCall)]
        [DataRow(BreakCause.FunctionAnyCall)]
        [DataRow(BreakCause.FunctionClrCall)]
        [DataRow(BreakCause.FunctionUserCall)]
        public void FunctionNotBreaksOnNotCall(BreakCause breakCause)
        {
            // Arrange
            var valueMock = new Mock<IScriptType>();

            var processor = new PyProcessor();
            processor.PushValue(valueMock.Object);

            var nextOp = Mock.Of<IOpCode>();
            var breakpoint = new Breakpoint(breakCause);

            // Act
            bool result = breakpoint.ShouldBreak(processor, nextOp);

            // Assert
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(3)]
        public void FunctionUserBreaks(int args)
        {
            // Arrange
            var valueMock = new Mock<IScriptType>();

            var processor = new PyProcessor();
            processor.PushValue(valueMock.Object);

            var call = PushCallArgsStack(processor, args);
            var breakpoint = new Breakpoint(BreakCause.FunctionUserCall);

            // Act

            // Checking User functions are not yet possible
            Assert.ThrowsException<NotImplementedException>(delegate
            {
                bool _ = breakpoint.ShouldBreak(processor, call);
            });
        }
    }
}