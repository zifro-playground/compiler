using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticPositive : UnaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.APos;

        public ArithmeticPositive(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}