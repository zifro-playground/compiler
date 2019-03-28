using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Lang.Python3.Tests.TestingOps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Compiler.Statements
{
    [TestClass]
    public class WhileStatementTests
    {
        [TestMethod]
        public void Compile_While_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out NopOp testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out NopOp suiteOp);

            var stmt = new WhileStatement(SourceReference.ClrSource,
                testMock.Object, suiteMock.Object
            );

            // Act
            stmt.Compile(compiler);

            // Assert
            /*
             * Ops should be structured:
             * 0: jmp -> @2
             * 1: {suite}
             * 2: {test}
             * 3: jmp if true -> @1
             */

            var jumpToTest = Assert.That.IsOpCode<Jump>(compiler, 0);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 1, suiteOp);
            // test
            Assert.That.IsExpectedOpCode(compiler, 2, testOp);
            var jumpToSuite = Assert.That.IsOpCode<JumpIfTrue>(compiler, 3);
            // end

            // assert labels
            Assert.AreEqual(1, jumpToSuite.Target);
            Assert.AreEqual(2, jumpToTest.Target);

            Assert.AreEqual(4, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
        }
    }
}