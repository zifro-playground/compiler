using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    public class OperatorNot : UnaryOperator
    {
        public OperatorNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}