namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetic
{
    public class ArithmeticMultiply : BinaryOperator
    {
        public ArithmeticMultiply(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}