using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    public class OperatorAnd : BinaryOperator
    {
        public OperatorAnd(SourceReference source,
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(source, leftOperand, rightOperand)
        {
        }

        public OperatorAnd(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : this(SourceReference.Merge(leftOperand.Source, rightOperand.Source),
                leftOperand, rightOperand)
        {
        }
    }
}