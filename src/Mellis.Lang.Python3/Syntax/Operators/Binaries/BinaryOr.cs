using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryOr : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.BOr;

        public BinaryOr(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}