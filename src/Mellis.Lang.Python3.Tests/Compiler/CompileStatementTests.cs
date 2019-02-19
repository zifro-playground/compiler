using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Lang.Python3.Tests.TestingOps;

namespace Mellis.Lang.Python3.Tests.Compiler
{
    [TestClass]
    public class CompileStatementTests
    {
        [TestMethod]
        public void CompileAssignmentOnIdentifierTest()
        {
            // Arrange
            const string identifier = "foo";
            var compiler = new PyCompiler();

            var idLhsMock = new Mock<Identifier>(SourceReference.ClrSource, identifier);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: idLhsMock.Object,
                rightOperand: exprRhsMock.Object);

            // Act
            stmt.Compile(compiler);

            // Assert
            var setOpCode = Assert.That.IsOpCode<VarSet>(compiler, index: 1);
            Assert.AreEqual(identifier, setOpCode.Identifier);
            Assert.AreEqual(2, compiler.Count);
            Assert.AreSame(exprRhsOp, compiler[0], "compiler[0] was not exprRhsOp");

            idLhsMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Once);
        }

        [TestMethod]
        public void CompileAssignmentOnLiteralTest()
        {
            // Arrange
            var compiler = new PyCompiler();

            var source = new SourceReference(3,5,5,1);
            var intLhsMock = new Mock<LiteralInteger>(source, 5);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> exprRhsMock,
                out NopOp exprRhsOp);

            var stmt = new Assignment(SourceReference.ClrSource,
                leftOperand: intLhsMock.Object,
                rightOperand: exprRhsMock.Object);

            void Action()
            {
                stmt.Compile(compiler);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>((Action) Action);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, source);

            intLhsMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [TestMethod]
        public void Compile_If_Test()
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> testMock,
                out NopOp testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out NopOp suiteOp);

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
                out NopOp testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out NopOp suiteOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elseMock,
                out NopOp elseOp);

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
                out NopOp testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out NopOp suiteOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elifTestMock,
                out NopOp elifTestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elifSuiteMock,
                out NopOp elifSuiteOp);

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
                out NopOp testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out NopOp suiteOp);

            // elif 2
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elif2TestMock,
                out NopOp elif2TestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elif2SuiteMock,
                out NopOp elif2SuiteOp);

            var elif2 = new IfStatement(SourceReference.ClrSource,
                condition: elif2TestMock.Object,
                ifSuite: elif2SuiteMock.Object);

            // elif 1
            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elif1TestMock,
                out NopOp elif1TestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elif1SuiteMock,
                out NopOp elif1SuiteOp);

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
                out NopOp testOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out NopOp suiteOp);

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> elifTestMock,
                out NopOp elifTestOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elifSuiteMock,
                out NopOp elifSuiteOp);

            compiler.CreateAndSetup(
                out Mock<Statement> elseSuiteMock,
                out NopOp elseSuiteOp);

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