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
            var labelEnd = Assert.That.IsOpCode<Label>(compiler, 3);

            // assert labels
            Assert.AreSame(labelEnd, jumpToEnd.Target);

            Assert.AreEqual(4, compiler.Count, "Too many op codes");

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
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);
            var jumpToElse = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);
            // else
            var labelElse = Assert.That.IsOpCode<Label>(compiler, 4);
            Assert.That.IsExpectedOpCode(compiler, 5, elseOp);
            // end
            var labelEnd = Assert.That.IsOpCode<Label>(compiler, 6);

            // assert labels
            Assert.AreSame(labelEnd, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreSame(labelElse, jumpToElse.Target, "jump from if test not match else label");

            Assert.AreEqual(7, compiler.Count, "Too many op codes");

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
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);
            var jumpToElif = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);
            // elif
            var labelElif = Assert.That.IsOpCode<Label>(compiler, 4);
            Assert.That.IsExpectedOpCode(compiler, 5, elifTestOp);
            var jumpFromElif = Assert.That.IsOpCode<JumpIfFalse>(compiler, 6);
            Assert.That.IsExpectedOpCode(compiler, 7, elifSuiteOp);
            // end
            var labelEnd = Assert.That.IsOpCode<Label>(compiler, 8);

            // assert labels
            Assert.AreSame(labelEnd, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreSame(labelElif, jumpToElif.Target, "jump from if test not match elif label");
            Assert.AreSame(labelEnd, jumpFromElif.Target, "jump from elif test not match end label");

            Assert.AreEqual(9, compiler.Count, "Too many op codes");

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
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);
            var jumpToElif1 = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);
            // elif 1
            var labelElif1 = Assert.That.IsOpCode<Label>(compiler, 4);
            Assert.That.IsExpectedOpCode(compiler, 5, elif1TestOp);
            var jumpToElif2 = Assert.That.IsOpCode<JumpIfFalse>(compiler, 6);
            Assert.That.IsExpectedOpCode(compiler, 7, elif1SuiteOp);
            var jumpFromElif1 = Assert.That.IsOpCode<Jump>(compiler, 8);
            // elif 2
            var labelElif2 = Assert.That.IsOpCode<Label>(compiler, 9);
            Assert.That.IsExpectedOpCode(compiler, 10, elif2TestOp);
            var jumpToElse = Assert.That.IsOpCode<JumpIfFalse>(compiler, 11);
            Assert.That.IsExpectedOpCode(compiler, 12, elif2SuiteOp);
            // no jumpFromElif2 because it's the last one

            // end
            var labelEnd = Assert.That.IsOpCode<Label>(compiler, 13);

            // assert labels
            Assert.AreSame(labelEnd, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreSame(labelEnd, jumpFromElif1.Target, "jump from elif1 suite not match end label");
            Assert.AreSame(labelElif1, jumpToElif1.Target, "jump from if test not match elif1 label");
            Assert.AreSame(labelElif2, jumpToElif2.Target, "jump from elif1 test not match elif2 label");
            Assert.AreSame(labelEnd, jumpToElse.Target, "jump from elif2 test not match end label");

            Assert.AreEqual(14, compiler.Count, "Too many op codes");

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
            Assert.That.IsExpectedOpCode(compiler, 0, testOp);
            var jumpToElif = Assert.That.IsOpCode<JumpIfFalse>(compiler, 1);
            // suite
            Assert.That.IsExpectedOpCode(compiler, 2, suiteOp);
            var jumpFromIf = Assert.That.IsOpCode<Jump>(compiler, 3);
            // elif
            var labelElif = Assert.That.IsOpCode<Label>(compiler, 4);
            Assert.That.IsExpectedOpCode(compiler, 5, elifTestOp);
            var jumpToElse = Assert.That.IsOpCode<JumpIfFalse>(compiler, 6);
            Assert.That.IsExpectedOpCode(compiler, 7, elifSuiteOp);
            var jumpFromElif = Assert.That.IsOpCode<Jump>(compiler, 8);
            // else
            var labelElse = Assert.That.IsOpCode<Label>(compiler, 9);
            Assert.That.IsExpectedOpCode(compiler, 10, elseSuiteOp);
            // end
            var labelEnd = Assert.That.IsOpCode<Label>(compiler, 11);

            // assert labels
            Assert.AreSame(labelEnd, jumpFromIf.Target, "jump from if suite not match end label");
            Assert.AreSame(labelEnd, jumpFromElif.Target, "jump from elif suite not match end label");
            Assert.AreSame(labelElif, jumpToElif.Target, "jump from if test not match elif label");
            Assert.AreSame(labelElse, jumpToElse.Target, "jump from elif test not match else label");

            Assert.AreEqual(12, compiler.Count, "Too many op codes");

            testMock.Verify(o => o.Compile(compiler), Times.Once);
            suiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elifTestMock.Verify(o => o.Compile(compiler), Times.Once);
            elifSuiteMock.Verify(o => o.Compile(compiler), Times.Once);
            elseSuiteMock.Verify(o => o.Compile(compiler), Times.Once);
        }
    }
}