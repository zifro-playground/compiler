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
        [DataRow(Python3Parser.ADD_ASSIGN, BasicOperatorCode.IAAdd, DisplayName = "ADD_ASSIGN += IAAdd")]
        [DataRow(Python3Parser.SUB_ASSIGN, BasicOperatorCode.IASub, DisplayName = "SUB_ASSIGN -= IASub")]
        [DataRow(Python3Parser.MULT_ASSIGN, BasicOperatorCode.IAMul, DisplayName = "MULT_ASSIGN *= IAMul")]
        [DataRow(Python3Parser.DIV_ASSIGN, BasicOperatorCode.IADiv, DisplayName = "DIV_ASSIGN /= IADiv")]
        [DataRow(Python3Parser.MOD_ASSIGN, BasicOperatorCode.IAMod, DisplayName = "MOD_ASSIGN %= IAMod")]
        [DataRow(Python3Parser.AT_ASSIGN, BasicOperatorCode.IAMat, DisplayName = "MOD_ASSIGN @= IAMat")]
        [DataRow(Python3Parser.AND_ASSIGN, BasicOperatorCode.IBAnd, DisplayName = "AND_ASSIGN &= IBAnd")]
        [DataRow(Python3Parser.OR_ASSIGN, BasicOperatorCode.IBOr, DisplayName = "OR_ASSIGN |= IBOr")]
        [DataRow(Python3Parser.XOR_ASSIGN, BasicOperatorCode.IBXor, DisplayName = "XOR_ASSIGN ^= IBXor")]
        [DataRow(Python3Parser.LEFT_SHIFT_ASSIGN, BasicOperatorCode.IBLsh, DisplayName = "LEFT_SHIFT_ASSIGN <<= IBLsh")]
        [DataRow(Python3Parser.RIGHT_SHIFT_ASSIGN, BasicOperatorCode.IBRsh, DisplayName = "RIGHT_SHIFT_ASSIGN >>= IBRsh")]
        [DataRow(Python3Parser.POWER_ASSIGN, BasicOperatorCode.IAPow, DisplayName = "POWER_ASSIGN **= IAPow")]
        [DataRow(Python3Parser.IDIV_ASSIGN, BasicOperatorCode.IAFlr, DisplayName = "IDIV_ASSIGN //= IAFlr")]
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
    }
}