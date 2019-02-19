using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareNotEquals : Comparison
    {
        public override ComparisonType Type => ComparisonType.NotEquals;

        public override OperatorCode OpCode => OperatorCode.CNEq;

        public CompareNotEquals(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}