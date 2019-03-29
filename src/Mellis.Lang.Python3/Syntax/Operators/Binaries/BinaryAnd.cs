using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryAnd : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.BAnd;

        public BinaryAnd(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}