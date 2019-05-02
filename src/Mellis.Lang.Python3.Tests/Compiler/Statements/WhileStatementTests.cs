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
        private static PyCompiler ArrangeAndAct(
            out NopOp suiteOp,
            out NopOp testOp,
            out Mock<Statement> suiteMock,
            out Mock<ExpressionNode> testMock)
        {
            return ArrangeAndAct(CompilerSettings.DefaultSettings,
                out suiteOp, out testOp,
                out suiteMock, out testMock);
        }

        private static PyCompiler ArrangeAndAct(
            CompilerSettings settings,
            out NopOp suiteOp,
            out NopOp testOp,
            out Mock<Statement> suiteMock,
            out Mock<ExpressionNode> testMock
        )
        {
            // Arrange
            var compiler = new PyCompiler {
                Settings = settings
            };

            compiler.CreateAndSetup(
                out testMock,
                out testOp);

            compiler.CreateAndSetup(
                out suiteMock,
                out suiteOp);

            var stmt = new WhileStatement(SourceReference.ClrSource,
                testMock.Object, suiteMock.Object
            );

            // Act
            stmt.Compile(compiler);

            return compiler;
        }

        [TestMethod]
        public void Compile_While_Test()
        {
            // Arrange + act
            var compiler = ArrangeAndAct(
                out var suiteOp,
                out var testOp,
                out var suiteMock,
                out var testMock
            );

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

        [TestMethod]
        public void BreaksOn_LoopBlockEnd_Test()
        {
            var compiler = ArrangeAndAct(
                new CompilerSettings {
                    BreakOn = BreakCause.LoopBlockEnd
                },
                out var suiteOp,
                out var testOp,
                out _, out _
            );

            // Assert
            /*
             * Ops should be structured:
             * 0: jmp -> @2
             * 1: {suite}
             * 2: {test}
             * 3: break->LoopBlockEnd
             * 4: jmp if true -> @1
             */

            Assert.That.IsOpCode<Jump>(compiler, 0);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 1, suiteOp);
            // test
            Assert.That.IsExpectedOpCode(compiler, 2, testOp);
            var breakpoint = Assert.That.IsOpCode<Breakpoint>(compiler, 3);
            Assert.That.IsOpCode<JumpIfTrue>(compiler, 4);
            // end

            // assert op codes
            Assert.AreEqual(BreakCause.LoopBlockEnd, breakpoint.BreakCause);

            Assert.AreEqual(5, compiler.Count, "Too many op codes");
        }

        [TestMethod]
        public void BreaksOn_LoopEnter_Test()
        {
            var compiler = ArrangeAndAct(
                new CompilerSettings {
                    BreakOn = BreakCause.LoopEnter
                },
                out var suiteOp,
                out var testOp,
                out _, out _
            );

            // Assert
            /*
             * Ops should be structured:
             * 0: break->LoopEnter
             * 1: jmp -> @2
             * 2: {suite}
             * 3: {test}
             * 4: jmp if true -> @1
             */

            var breakpoint = Assert.That.IsOpCode<Breakpoint>(compiler, 0);
            Assert.That.IsOpCode<Jump>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            // test
            Assert.That.IsExpectedOpCode(compiler, 3, testOp);
            Assert.That.IsOpCode<JumpIfTrue>(compiler, 4);
            // end

            // assert op codes
            Assert.AreEqual(BreakCause.LoopEnter, breakpoint.BreakCause);

            Assert.AreEqual(5, compiler.Count, "Too many op codes");
        }

        [TestMethod]
        public void BreaksOn_LoopBoth_Test()
        {
            var compiler = ArrangeAndAct(
                new CompilerSettings {
                    BreakOn = BreakCause.LoopEnter |
                              BreakCause.LoopBlockEnd
                },
                out var suiteOp,
                out var testOp,
                out _, out _
            );

            // Assert
            /*
             * Ops should be structured:
             * 0: break->LoopEnter
             * 1: jmp -> @2
             * 2: {suite}
             * 3: {test}
             * 4: break->LoopBlockEnd
             * 5: jmp if true -> @1
             */

            var breakpoint1 = Assert.That.IsOpCode<Breakpoint>(compiler, 0);
            Assert.That.IsOpCode<Jump>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            // test
            Assert.That.IsExpectedOpCode(compiler, 3, testOp);
            var breakpoint2 = Assert.That.IsOpCode<Breakpoint>(compiler, 4);
            Assert.That.IsOpCode<JumpIfTrue>(compiler, 5);
            // end

            // assert op codes
            Assert.AreEqual(BreakCause.LoopEnter, breakpoint1.BreakCause);
            Assert.AreEqual(BreakCause.LoopBlockEnd, breakpoint2.BreakCause);

            Assert.AreEqual(6, compiler.Count, "Too many op codes");
        }
    }
}