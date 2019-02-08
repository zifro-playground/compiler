namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetic
{
    public class ArithmeticModulus : BinaryOperator
    {
        public ArithmeticModulus(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}