using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticFloor : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.AFlr;

        public ArithmeticFloor(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}