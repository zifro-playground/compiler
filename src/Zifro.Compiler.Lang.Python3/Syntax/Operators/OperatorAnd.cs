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
    }
}