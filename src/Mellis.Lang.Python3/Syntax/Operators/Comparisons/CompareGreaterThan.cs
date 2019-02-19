using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareGreaterThan : Comparison
    {
        public override ComparisonType Type => ComparisonType.GreaterThan;

        public override OperatorCode OpCode => OperatorCode.CGt;

        public CompareGreaterThan(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}