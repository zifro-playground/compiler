using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareLessThanOrEqual : Comparison
    {
        public override ComparisonType Type => ComparisonType.LessThanOrEqual;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CLtEq;

        public CompareLessThanOrEqual(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}