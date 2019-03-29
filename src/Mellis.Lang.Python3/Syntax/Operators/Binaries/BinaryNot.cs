using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryNot : UnaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.BNot;

        public BinaryNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}