using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareGreaterThanOrEqual : Comparison
    {
        public override ComparisonType Type => ComparisonType.GreaterThanOrEqual;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CGtEq;

        public CompareGreaterThanOrEqual(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}