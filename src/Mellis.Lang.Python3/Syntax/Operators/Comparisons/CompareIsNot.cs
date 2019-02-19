using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareIsNot : Comparison
    {
        public override ComparisonType Type => ComparisonType.Equals;

        public override OperatorCode OpCode => OperatorCode.CEq;

        public CompareIsNot(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}