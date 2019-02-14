using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalNot : UnaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.LNot;

        public LogicalNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}