using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticMultiply : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.AMul;

        public ArithmeticMultiply(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}