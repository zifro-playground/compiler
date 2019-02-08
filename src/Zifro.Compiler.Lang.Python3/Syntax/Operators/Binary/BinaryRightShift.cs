namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binary
{
    public class BinaryRightShift : BinaryOperator
    {
        public BinaryRightShift(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}