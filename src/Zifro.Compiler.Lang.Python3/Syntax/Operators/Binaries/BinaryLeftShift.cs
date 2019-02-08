namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryLeftShift : BinaryOperator
    {
        public BinaryLeftShift(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}