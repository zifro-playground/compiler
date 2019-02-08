using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons
{
    public abstract class Comparison : BinaryOperator
    {
        public abstract ComparisonType Type { get; }

        protected Comparison(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}