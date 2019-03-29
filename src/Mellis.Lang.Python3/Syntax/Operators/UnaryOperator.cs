using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Common for negating operators such as the "not" (!value), "binary not" (~value)
    /// </summary>
    public abstract class UnaryOperator : ExpressionNode
    {
        protected UnaryOperator(SourceReference source, ExpressionNode operand)
            : base(SourceReference.Merge(source, operand.Source))
        {
            Operand = operand;
        }

        public ExpressionNode Operand { get; }
    }
}