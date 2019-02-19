using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareIs : Comparison
    {
        public override ComparisonType Type => ComparisonType.Is;

        public override OperatorCode OpCode => OperatorCode.CIs;

        public CompareIs(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}