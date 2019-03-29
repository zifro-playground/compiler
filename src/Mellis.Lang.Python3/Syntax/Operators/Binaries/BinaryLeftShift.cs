using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryLeftShift : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.BLsh;

        public BinaryLeftShift(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}