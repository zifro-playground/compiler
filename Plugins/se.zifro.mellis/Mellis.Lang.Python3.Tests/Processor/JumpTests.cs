using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class JumpTests
    {
        [TestMethod]
        public void EvaluateLabelsTest()
        {
            // Arrange
            var label1 = new Label(SourceReference.ClrSource);
            var label2 = new Label(SourceReference.ClrSource);
            var label3 = new Label(SourceReference.ClrSource);

            var processor = new PyProcessor(
                new NopOp(),    // 0
                label1,         // 1
                label2,         // 2
                new NopOp(),    // 3
                label3          // 4
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);

            Assert.AreEqual(1, label1.OpCodeIndex);
            Assert.AreEqual(2, label2.OpCodeIndex);
            Assert.AreEqual(4, label3.OpCodeIndex);
        }

        [TestMethod]
        public void EvaluateJumpUpwardsTest()
        {
            // Arrange
            var label = new Label(SourceReference.ClrSource);

            var processor = new PyProcessor(
                label,
                new NopOp(),
                new Jump(SourceReference.ClrSource, label)
            );
            
            // Act
            processor.WalkInstruction(); // stepped over label
            processor.WalkInstruction();
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            Assert.AreEqual(-1, processor.ProgramCounter);
        }
        
        [TestMethod]
        [Timeout(1000)]
        public void EvaluateJumpLoopTimesOutTest()
        {
            // Arrange
            var label = new Label(SourceReference.ClrSource);

            var processor = new PyProcessor(
                label,
                new Jump(SourceReference.ClrSource, label)
            );

            // Act
            processor.WalkLine();

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            Assert.AreEqual(-1, processor.ProgramCounter);
        }

        [TestMethod]
        public void EvaluateJumpDownwardsTest()
        {
            // Arrange
            var label = new Label(SourceReference.ClrSource);

            var processor = new PyProcessor(
                new Jump(SourceReference.ClrSource, label),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                label
            );

            // Act
            processor.WalkInstruction(); // performed jump
            processor.WalkInstruction(); // stepped over label

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        protected static IScriptType GetPyValue(object value, PyProcessor processor)
        {
            switch (value)
            {
                case bool b:
                    return new PyBoolean(processor, b);
                case int i:
                    return new PyInteger(processor, i);
                case double d:
                    return new PyDouble(processor, d);
                case string s:
                    return new PyString(processor, s);

                default:
                    throw new NotSupportedException();
            }
        }

        [DataTestMethod]
        [DataRow(0, DisplayName = "int 0")]
        [DataRow(0d, DisplayName = "double 0")]
        [DataRow(false, DisplayName = "bool false")]
        [DataRow("", DisplayName = "string \"\"")]
        public void EvaluateJumpIfFalse_Falsy_Test(object value)
        {
            // Arrange
            var label = new Label(SourceReference.ClrSource);

            var processor = new PyProcessor(
                new JumpIfFalse(SourceReference.ClrSource, label),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                label
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // performed jump
            processor.WalkInstruction(); // stepped over label

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [DataTestMethod]
        [DataRow(1, DisplayName = "int 1")]
        [DataRow(int.MinValue, DisplayName = "int -2147483648")]
        [DataRow(int.MaxValue, DisplayName = "int 2147483647")]
        [DataRow(1.0d, DisplayName = "double 1")]
        [DataRow(0.0001d, DisplayName = "double 0.0001")]
        [DataRow(double.NaN, DisplayName = "double NaN")]
        [DataRow(double.PositiveInfinity, DisplayName = "double +inf")]
        [DataRow(true, DisplayName = "bool true")]
        [DataRow("foo", DisplayName = "string \"foo\"")]
        public void EvaluateJumpIfFalse_Truthy_Test(object value)
        {
            // Arrange
            var label = new Label(SourceReference.ClrSource);

            var processor = new PyProcessor(
                new JumpIfFalse(SourceReference.ClrSource, label),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                label
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // stepped over jump
            processor.WalkInstruction();

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);
            Assert.AreEqual(1, processor.ProgramCounter);
        }
    }
}