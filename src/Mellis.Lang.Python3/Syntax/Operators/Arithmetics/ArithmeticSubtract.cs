using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticSubtract : BasicBinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.ASub;

        public ArithmeticSubtract(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}