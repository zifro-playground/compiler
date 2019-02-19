using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareIn : Comparison
    {
        public override ComparisonType Type => ComparisonType.Equals;

        public override OperatorCode OpCode => OperatorCode.CEq;

        public CompareIn(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}