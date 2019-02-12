namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries
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