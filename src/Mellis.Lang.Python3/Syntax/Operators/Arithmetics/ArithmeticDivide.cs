using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticDivide : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.ADiv;

        public ArithmeticDivide(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}