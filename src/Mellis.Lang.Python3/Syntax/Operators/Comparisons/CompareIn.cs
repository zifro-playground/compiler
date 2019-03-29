using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareIn : Comparison
    {
        public override ComparisonType Type => ComparisonType.In;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CIn;

        public CompareIn(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}