using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries
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