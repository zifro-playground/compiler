using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticPower : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.APow;

        public ArithmeticPower(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}