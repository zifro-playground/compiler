using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareIsNot : Comparison
    {
        public override ComparisonType Type => ComparisonType.IsNot;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CIsN;

        public CompareIsNot(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}