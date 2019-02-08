namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetic
{
    public class ArithmeticDivide : BinaryOperator
    {
        public ArithmeticDivide(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}