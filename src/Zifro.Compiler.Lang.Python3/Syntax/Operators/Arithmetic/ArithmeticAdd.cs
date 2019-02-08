namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetic
{
    public class ArithmeticAdd : BinaryOperator
    {
        public ArithmeticAdd(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}