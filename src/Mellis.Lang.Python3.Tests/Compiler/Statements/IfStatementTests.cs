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
    public class IfStatementTests
    {
        [TestMethod]
        public void Compile_If_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out var testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            var stmt = new IfStatement(SourceReference.ClrSource,
                testMock.Object, suiteMock.Object
            );

            // Act
            stmt.Compile(compiler);

            // Assert
            // test
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);
            var jumpToEnd = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            // end

            // assert labels
            Assert.AreEqual(3, jumpToEnd.Target);

            Assert.AreEqual(3, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void Compile_If_Else_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out var testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elseMock,
                out var elseOp);

            var stmt = new IfStatement(SourceReference.ClrSource,
                condition: testMock.Object,
                ifSuite: suiteMock.Object,
                elseSuite: elseMock.Object
            );

            // Act
            stmt.Compile(compiler);

            // Assert
            // test
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);                  // 0 if test expr
            var jumpToElse = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);    // 1 jumpif to else
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);                 // 2 if suite
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);           // 3 jump to end
            // else
            Assert.That.IsExpectedOpCode(compiler, 4, elseOp);                  // 4 else suite
            // end

            // assert labels
            Assert.AreEqual(5, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreEqual(4, jumpToElse.Target, "jump from if test not match else label");

            Assert.AreEqual(5, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elseMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void Compile_If_Elif_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out var testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elifTestMock,
                out var elifTestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elifSuiteMock,
                out var elifSuiteOp);

            var elif = new IfStatement(SourceReference.ClrSource,
                condition: elifTestMock.Object,
                ifSuite: elifSuiteMock.Object);

            var stmt = new IfStatement(SourceReference.ClrSource,
                condition: testMock.Object,
                ifSuite: suiteMock.Object,
                elseSuite: elif
            );

            // Act
            stmt.Compile(compiler);

            // Assert
            // test
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);                  // 0 if test expr
            var jumpToElif = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);    // 1 jumpif to elif
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);                 // 2 if suite
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);           // 3 jump to end
            // elif
            Assert.That.IsExpectedOpCode(compiler, 4, elifTestOp);              // 4 elif test expr
            var jumpFromElif = Assert.That.IsOpCode<JumpIfFalse>(compiler, 5);  // 5 jumpif to end
            Assert.That.IsExpectedOpCode(compiler, 6, elifSuiteOp);             // 6 elif suite
            // end

            // assert labels
            Assert.AreEqual(4, jumpToElif.Target, "jump from if test not match elif label");
            Assert.AreEqual(7, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreEqual(7, jumpFromElif.Target, "jump from elif test not match end label");

            Assert.AreEqual(7, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elifTestMock.Verify(o => o.Compile(compiler), Times.Once);
            elifSuiteMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void Compile_If_Elif_Elif_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out var testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            // elif 2
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elif2TestMock,
                out var elif2TestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elif2SuiteMock,
                out var elif2SuiteOp);

            var elif2 = new IfStatement(SourceReference.ClrSource,
                condition: elif2TestMock.Object,
                ifSuite: elif2SuiteMock.Object);

            // elif 1
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elif1TestMock,
                out var elif1TestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elif1SuiteMock,
                out var elif1SuiteOp);

            var elif1 = new IfStatement(SourceReference.ClrSource,
                condition: elif1TestMock.Object,
                ifSuite: elif1SuiteMock.Object,
                elseSuite: elif2);

            // top if
            var stmt = new IfStatement(SourceReference.ClrSource,
                condition: testMock.Object,
                ifSuite: suiteMock.Object,
                elseSuite: elif1
            );

            // Act
            stmt.Compile(compiler);

            // Assert
            // test
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);                  // 0 if expr
            var jumpToElif1 = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);   // 1 jumpif to elif1
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);                 // 2 if suite
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);           // 3 jump to end
            // elif 1
            Assert.That.IsExpectedOpCode(compiler, 4, elif1TestOp);             // 4 elif1 expr
            var jumpToElif2 = Assert.That.IsOpCode<JumpIfFalse>(compiler, 5);   // 5 jumpif to elif2
            Assert.That.IsExpectedOpCode(compiler, 6, elif1SuiteOp);            // 6 elif1 suite
            var jumpFromElif1 = Assert.That.IsOpCode<Jump>(compiler, 7);        // 7 jump to end
            // elif 2
            Assert.That.IsExpectedOpCode(compiler, 8, elif2TestOp);             // 8 elif2 expr
            var jumpFromElif2 = Assert.That.IsOpCode<JumpIfFalse>(compiler, 9); // 9 jumpif to end
            Assert.That.IsExpectedOpCode(compiler, 10, elif2SuiteOp);           // 10 elif2 suite
            // no jumpFromElif2 because it's the last one

            // end
            //var labelEnd = Assert.That.IsOpCode<Label>(compiler, 13);

            // assert labels
            Assert.AreEqual(4, jumpToElif1.Target, "jump from if test not match elif1 label");
            Assert.AreEqual(11, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreEqual(8, jumpToElif2.Target, "jump from elif1 test not match elif2 label");
            Assert.AreEqual(11, jumpFromElif1.Target, "jump from elif1 suite not match end label");
            Assert.AreEqual(11, jumpFromElif2.Target, "jump from elif2 test not match end label");

            Assert.AreEqual(11, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elif1TestMock.Verify(o => o.Compile(compiler), Times.Once);
            elif1SuiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elif2TestMock.Verify(o => o.Compile(compiler), Times.Once);
            elif2SuiteMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void Compile_If_Elif_Else_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out var testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elifTestMock,
                out var elifTestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elifSuiteMock,
                out var elifSuiteOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elseSuiteMock,
                out var elseSuiteOp);

            var elif = new IfStatement(SourceReference.ClrSource,
                condition: elifTestMock.Object,
                ifSuite: elifSuiteMock.Object,
                elseSuite: elseSuiteMock.Object);

            var stmt = new IfStatement(SourceReference.ClrSource,
                condition: testMock.Object,
                ifSuite: suiteMock.Object,
                elseSuite: elif
            );

            // Act
            stmt.Compile(compiler);

            // Assert
            // test
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);                  // 0 if expr
            var jumpToElif = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);    // 1 jumpif to elif
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);                 // 2 if suite
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);           // 3 jump to end
            // elif
            Assert.That.IsExpectedOpCode(compiler, 4, elifTestOp);              // 4 elif expr
            var jumpToElse = Assert.That.IsOpCode<JumpIfFalse>(compiler, 5);    // 5 jumpif to else
            Assert.That.IsExpectedOpCode(compiler, 6, elifSuiteOp);             // 6 elif suite
            var jumpFromElif = Assert.That.IsOpCode<Jump>(compiler, 7);         // 7 jump to end
            // else
            Assert.That.IsExpectedOpCode(compiler, 8, elseSuiteOp);             // 8 else suite
            // end

            // assert labels
            Assert.AreEqual(4, jumpToElif.Target, "jump from if test not match elif label");
            Assert.AreEqual(9, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreEqual(8, jumpToElse.Target, "jump from elif test not match else label");
            Assert.AreEqual(9, jumpFromElif.Target, "jump from elif suite not match end label");

            Assert.AreEqual(9, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elifTestMock.Verify(o => o.Compile(compiler), Times.Once);
            elifSuiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elseSuiteMock.Verify(o => o.Compile(compiler), Times.Once);
        }
    }
}