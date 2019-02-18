using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticNegative : UnaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.ANeg;

        public ArithmeticNegative(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}