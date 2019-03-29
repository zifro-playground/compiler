using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryRightShift : BasicBinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.BRsh;

        public BinaryRightShift(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}