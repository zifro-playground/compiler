using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Compiler
{
    [TestClass]
    public class CompilerTests
    {
        [TestMethod]
        public void PushNullTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            void Action()
            {
                compiler.Push(null); 
            }

            // Act
            Assert.ThrowsException<ArgumentNullException>((Action) Action);
        }

        [TestMethod]
        public void PushOneTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            // Act
            compiler.Push(new NopOp());

            // Assert
            Assert.AreEqual(1, compiler.Count);
        }

        [TestMethod]
        public void PushManyTest()
        {
            // Arrange
            var compiler = new PyCompiler();
            var op1 = new NopOp();
            var op2 = new NopOp();
            var op3 = new NopOp();

            // Act
            compiler.Push(op1);
            compiler.Push(op2);
            compiler.Push(op3);

            // Assert
            Assert.AreEqual(3, compiler.Count);
            Assert.AreSame(op1, compiler[0]);
            Assert.AreSame(op2, compiler[1]);
            Assert.AreSame(op3, compiler[2]);
        }

        [TestMethod]
        public void PushRangeTest()
        {
            // Arrange
            var compiler = new PyCompiler();
            var op1 = new NopOp();
            var op2 = new NopOp();
            var op3 = new NopOp();

            // Act
            compiler.PushRange(new []
            {
                op1,
                op2,
                op3
            });

            // Assert
            Assert.AreEqual(3, compiler.Count);
            Assert.AreSame(op1, compiler[0]);
            Assert.AreSame(op2, compiler[1]);
            Assert.AreSame(op3, compiler[2]);
        }

        [TestMethod]
        public void PushNullRangeTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            // Act
            void Action()
            {
                compiler.PushRange(null);
            }

            // Act
            Assert.ThrowsException<ArgumentNullException>((Action)Action);
        }

        [TestMethod]
        public void PushRangeOneNullTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            // Act
            void Action()
            {
                compiler.PushRange(new []
                {
                    new NopOp(),
                    null,
                    new NopOp(), 
                });
            }

            // Act
            Assert.ThrowsException<ArgumentNullException>((Action)Action);
        }

        [TestMethod]
        public void IntegrationStringAssignmentTest()
        {
            // Arrange
            const string code = @"x = 5";
            var compiler = new PyCompiler();

            // Act
            compiler.Compile(code);

            // Assert
            Assert.AreNotEqual(0, compiler.Count, "Did not produce any op codes.");
            var literal = Assert.That.IsOpCode<PushLiteral<int>>(compiler, 0);
            Assert.AreEqual(5, literal.Literal.Value);
            var varSet = Assert.That.IsOpCode<VarSet>(compiler, 1);
            Assert.AreEqual("x", varSet.Identifier);
        }
    }
}