using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Tests.TestingOps;

namespace Zifro.Compiler.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class ScopeOpsEvaluateTests
    {
        [TestMethod]
        public void ScopePushTest()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new NopOp()
            );

            // Act
            processor.WalkInstruction();

            // Assert
            Assert.AreNotSame(processor.GlobalScope, processor.CurrentScope);
        }

        [TestMethod]
        public void ScopePush2Test()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePush(SourceReference.ClrSource),
                new NopOp()
            );

            // Act
            processor.WalkInstruction();
            var scope = processor.CurrentScope;
            processor.WalkInstruction();

            // Assert
            Assert.AreNotSame(processor.CurrentScope, processor.GlobalScope);
            Assert.AreNotSame(processor.CurrentScope, scope);
        }

        [TestMethod]
        public void ScopePush2Pop1Test()
        {
            // Arrange
            var processor = new PyProcessor(
                new ScopePush(SourceReference.ClrSource),
                new ScopePush(SourceReference.ClrSource),
                new ScopePop(SourceReference.ClrSource),
                new NopOp()
            );

            // Act
            processor.WalkInstruction();
            var scope1 = processor.CurrentScope;
            processor.WalkInstruction();
            var scope2 = processor.CurrentScope;
            processor.WalkInstruction();

            // Assert
            Assert.AreNotSame(processor.CurrentScope, processor.GlobalScope);
            Assert.AreSame(processor.CurrentScope, scope1);
            Assert.AreNotSame(processor.CurrentScope, scope2);
        }
    }
}