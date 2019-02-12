using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareEquals : Comparison
    {
        public override ComparisonType Type => ComparisonType.Equals;

        public CompareEquals(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override OperatorCode OpCode => throw new System.NotImplementedException();
    }
}