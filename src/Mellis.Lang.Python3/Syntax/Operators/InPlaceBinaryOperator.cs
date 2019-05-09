using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    public abstract class InPlaceBinaryOperator : BasicBinaryOperator
    {
        protected InPlaceBinaryOperator(SourceReference source, ExpressionNode leftOperand, ExpressionNode rightOperand) :
            base(source, leftOperand, rightOperand)
        {
        }

        protected InPlaceBinaryOperator(ExpressionNode leftOperand, ExpressionNode rightOperand) : base(leftOperand,
            rightOperand)
        {
        }
    }
}