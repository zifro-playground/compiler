namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryOr : BinaryOperator
    {
        public BinaryOr(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}