using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareGreaterThan : Comparison
    {
        public override ComparisonType Type => ComparisonType.GreaterThan;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CGt;

        public CompareGreaterThan(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}