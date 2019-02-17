namespace Mellis.Lang.Python3.Syntax.Operators
{
    public abstract class Comparison : BinaryOperator
    {
        public abstract ComparisonType Type { get; }

        protected Comparison(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}