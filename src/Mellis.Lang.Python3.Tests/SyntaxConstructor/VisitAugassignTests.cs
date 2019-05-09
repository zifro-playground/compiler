using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Statements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor
{
    [TestClass]
    public class VisitAugassignTests : BaseVisitClass<Python3Parser.AugassignContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitAugassign(contextMock.Object);
        }

        [DataTestMethod]
        [DataRow(Python3Parser.ADD_ASSIGN, BasicOperatorCode.AAdd, DisplayName = "ADD_ASSIGN += AAdd")]
        [DataRow(Python3Parser.SUB_ASSIGN, BasicOperatorCode.ASub, DisplayName = "SUB_ASSIGN -= ASub")]
        [DataRow(Python3Parser.MULT_ASSIGN, BasicOperatorCode.AMul, DisplayName = "MULT_ASSIGN *= AMul")]
        [DataRow(Python3Parser.DIV_ASSIGN, BasicOperatorCode.ADiv, DisplayName = "DIV_ASSIGN /= ADiv")]
        [DataRow(Python3Parser.MOD_ASSIGN, BasicOperatorCode.AMod, DisplayName = "MOD_ASSIGN %= AMod")]
        [DataRow(Python3Parser.AND_ASSIGN, BasicOperatorCode.BAnd, DisplayName = "AND_ASSIGN &= BAnd")]
        [DataRow(Python3Parser.OR_ASSIGN, BasicOperatorCode.BOr, DisplayName = "OR_ASSIGN |= BOr")]
        [DataRow(Python3Parser.XOR_ASSIGN, BasicOperatorCode.BXor, DisplayName = "XOR_ASSIGN ^= BXor")]
        [DataRow(Python3Parser.LEFT_SHIFT_ASSIGN, BasicOperatorCode.BLsh, DisplayName = "LEFT_SHIFT_ASSIGN <<= BLsh")]
        [DataRow(Python3Parser.RIGHT_SHIFT_ASSIGN, BasicOperatorCode.BRsh, DisplayName = "RIGHT_SHIFT_ASSIGN >>= BRsh")]
        [DataRow(Python3Parser.POWER_ASSIGN, BasicOperatorCode.APow, DisplayName = "POWER_ASSIGN **= APow")]
        [DataRow(Python3Parser.IDIV_ASSIGN, BasicOperatorCode.AFlr, DisplayName = "IDIV_ASSIGN //= AFlr")]
        public void Visit_AugmentedAssignment_ValidOp(int terminal, BasicOperatorCode expectedOpCode)
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(terminal)
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(InPlaceBinaryOperatorFactory));
            var factory = (InPlaceBinaryOperatorFactory)result;
            Assert.AreEqual(expectedOpCode, factory.OpCode);
        }

        [DataTestMethod]
        [DataRow(Python3Parser.AT_ASSIGN, "@=")]
        public void Visit_AugmentedAssignment_NotYetImplemented(int terminal, string expectedKeyword)
        {
            // Arrange
            var terminalNode = GetTerminal(terminal);
            contextMock.SetupChildren(
                terminalNode
            );

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, terminalNode, expectedKeyword);
        }
    }
}