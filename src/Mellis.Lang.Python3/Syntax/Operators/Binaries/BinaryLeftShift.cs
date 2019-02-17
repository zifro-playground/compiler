using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryLeftShift : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.BLsh;

        public BinaryLeftShift(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}