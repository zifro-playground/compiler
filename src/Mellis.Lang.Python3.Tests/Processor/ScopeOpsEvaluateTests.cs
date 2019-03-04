using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class ScopeOpsEvaluateTests
    {
        [TestMethod]
        public void ScopePushTest()
        {
            // Arrange
            var processor = new VM.PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new NopOp()
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // push->$scope

            // Assert
            Assert.AreNotSame(processor.GlobalScope, processor.CurrentScope);
        }

        [TestMethod]
        public void ScopePush2Test()
        {
            // Arrange
            var processor = new VM.PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePush(SourceReference.ClrSource),
                new NopOp()
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // push->$scope
            var scope = processor.CurrentScope;
            processor.WalkInstruction(); // push->$scope

            // Assert
            Assert.AreNotSame(processor.CurrentScope, processor.GlobalScope);
            Assert.AreNotSame(processor.CurrentScope, scope);
        }

        [TestMethod]
        public void ScopePush2Pop1Test()
        {
            // Arrange
            var processor = new VM.PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePush(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource),
                new NopOp()
            );

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkInstruction(); // push->$scope
            var scope1 = processor.CurrentScope;
            processor.WalkInstruction(); // push->$scope
            var scope2 = processor.CurrentScope;
            processor.WalkInstruction(); // pop->$scope

            // Assert
            Assert.AreNotSame(processor.CurrentScope, processor.GlobalScope);
            Assert.AreSame(processor.CurrentScope, scope1);
            Assert.AreNotSame(processor.CurrentScope, scope2);
        }
    }
}