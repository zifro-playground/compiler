using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryXor : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.BXor;

        public BinaryXor(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}