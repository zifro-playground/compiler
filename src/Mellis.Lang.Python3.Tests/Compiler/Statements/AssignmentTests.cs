using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Lang.Python3.Tests.TestingOps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Compiler.Statements
{
    [TestClass]
    public class AssignmentTests
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

            var source = new SourceReference(3, 5, 5, 1);
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
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>((Action)Action);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, source);

            intLhsMock.Verify(o => o.Compile(compiler), Times.Never);
            exprRhsMock.Verify(o => o.Compile(compiler), Times.Never);
        }

    }
}