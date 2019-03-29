using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareEquals : Comparison
    {
        public override ComparisonType Type => ComparisonType.Equals;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CEq;

        public CompareEquals(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}