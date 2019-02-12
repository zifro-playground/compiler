using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalNot : UnaryOperator
    {
        public LogicalNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}