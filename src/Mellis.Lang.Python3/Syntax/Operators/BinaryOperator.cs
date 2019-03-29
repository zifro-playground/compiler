using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Common two part operators, such as "and" (a && b), "or" (a || b), "xor" (a ^ b)
    /// </summary>
    public abstract class BinaryOperator : ExpressionNode
    {
        protected BinaryOperator(SourceReference source,
            ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        // You can happily merge them without conflict
        // since the operator source will always be the
        // combined range from lhs to rhs
        protected BinaryOperator(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : this(SourceReference.Merge(leftOperand.Source, rightOperand.Source),
                leftOperand, rightOperand)
        {
        }

        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }
    }
}