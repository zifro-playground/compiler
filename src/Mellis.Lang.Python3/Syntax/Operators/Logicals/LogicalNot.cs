using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalNot : UnaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.LNot;

        public LogicalNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}