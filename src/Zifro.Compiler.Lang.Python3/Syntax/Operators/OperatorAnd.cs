using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    public class OperatorAnd : BinaryOperator
    {
        public OperatorAnd(SourceReference source, string sourceText,
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(source, sourceText, leftOperand, rightOperand)
        {
        }
    }
}