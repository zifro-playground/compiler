using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryNot : UnaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.BNot;

        public BinaryNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}