using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticModulus : BasicBinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.AMod;

        public ArithmeticModulus(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}