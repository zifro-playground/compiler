using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareEquals : Comparison
    {
        public override ComparisonType Type => ComparisonType.Equals;

        public override OperatorCode OpCode => OperatorCode.CEq;

        public CompareEquals(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}