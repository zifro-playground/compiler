namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binary
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