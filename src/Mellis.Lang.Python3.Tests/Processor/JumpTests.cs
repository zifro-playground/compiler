using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class JumpTests
    {
        [TestMethod]
        public void EvaluateJumpUpwardsTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new NopOp(),
                new Jump(SourceReference.ClrSource, 0)
            );
            
            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction();
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            Assert.AreEqual(0, processor.ProgramCounter);
        }
        
        [TestMethod]
        [Timeout(1000)]
        public void EvaluateJumpLoopTimesOutTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Jump(SourceReference.ClrSource, 0)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);

            Assert.AreEqual(0, processor.ProgramCounter);
        }

        [TestMethod]
        public void EvaluateJumpBeyondLastTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Jump(SourceReference.ClrSource, 5),
                new NopOp()
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }

        [TestMethod]
        public void EvaluateJumpBeyondFirstTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Jump(SourceReference.ClrSource, -5)
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump
            processor.WalkInstruction(); // walk back to first

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);
            Assert.AreEqual(0, processor.ProgramCounter);
        }

        [TestMethod]
        public void EvaluateJumpDownwardsTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new Jump(SourceReference.ClrSource, 5),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

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
        [DataRow(true, DisplayName = "bool true")]
        [DataRow(false, DisplayName = "bool false")]
        public void EvaluateJumpIfFalse_Pops_Test(object value)
        {
            // Arrange
            var processor = new PyProcessor(
                new JumpIfFalse(SourceReference.ClrSource, 5, peek: false),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(0, processor.ValueStackCount, "Did not pop.");
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "bool true")]
        [DataRow(false, DisplayName = "bool false")]
        public void EvaluateJumpIfFalse_Peeks_Test(object value)
        {
            // Arrange
            var processor = new PyProcessor(
                new JumpIfFalse(SourceReference.ClrSource, 5, peek: true),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            var scriptType = GetPyValue(value, processor);
            processor.PushValue(scriptType);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(1, processor.ValueStackCount, "Did not peek.");
            Assert.AreSame(scriptType, processor.PopValue());
        }

        [DataTestMethod]
        [DataRow(0, DisplayName = "int 0")]
        [DataRow(0d, DisplayName = "double 0")]
        [DataRow(false, DisplayName = "bool false")]
        [DataRow("", DisplayName = "string \"\"")]
        public void EvaluateJumpIfFalse_Falsy_Test(object value)
        {
            // Arrange
            var processor = new PyProcessor(
                new JumpIfFalse(SourceReference.ClrSource, 5),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

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

            var processor = new PyProcessor(
                new JumpIfFalse(SourceReference.ClrSource, 5),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // stepped over jump

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);
            Assert.AreEqual(1, processor.ProgramCounter);
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "bool true")]
        [DataRow(false, DisplayName = "bool false")]
        public void EvaluateJumpIfTrue_Pops_Test(object value)
        {
            // Arrange
            var processor = new PyProcessor(
                new JumpIfTrue(SourceReference.ClrSource, 5, peek: false),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(0, processor.ValueStackCount, "Did not pop.");
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "bool true")]
        [DataRow(false, DisplayName = "bool false")]
        public void EvaluateJumpIfTrue_Peeks_Test(object value)
        {
            // Arrange
            var processor = new PyProcessor(
                new JumpIfTrue(SourceReference.ClrSource, 5, peek: true),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            var scriptType = GetPyValue(value, processor);
            processor.PushValue(scriptType);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(1, processor.ValueStackCount, "Did not peek.");
            Assert.AreSame(scriptType, processor.PopValue());
        }


        [DataTestMethod]
        [DataRow(0, DisplayName = "int 0")]
        [DataRow(0d, DisplayName = "double 0")]
        [DataRow(false, DisplayName = "bool false")]
        [DataRow("", DisplayName = "string \"\"")]
        public void EvaluateJumpIfTrue_Falsy_Test(object value)
        {
            // Arrange
            var processor = new PyProcessor(
                new JumpIfTrue(SourceReference.ClrSource, 5),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // performed jump

            // Assert
            Assert.AreEqual(ProcessState.Running, processor.State);
            Assert.AreEqual(1, processor.ProgramCounter);
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
        public void EvaluateJumpIfTrue_Truthy_Test(object value)
        {
            // Arrange

            var processor = new PyProcessor(
                new JumpIfTrue(SourceReference.ClrSource, 5),
                new NopOp(),
                new NopOp(),
                new NopOp(),
                new NopOp()
            );

            processor.PushValue(GetPyValue(value, processor));

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // stepped over jump

            // Assert
            Assert.AreEqual(ProcessState.Ended, processor.State);
        }
    }
}