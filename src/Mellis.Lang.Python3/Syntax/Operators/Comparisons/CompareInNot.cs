using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareInNot : Comparison
    {
        public override ComparisonType Type => ComparisonType.InNot;

        public override OperatorCode OpCode => OperatorCode.CNIn;

        public CompareInNot(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}