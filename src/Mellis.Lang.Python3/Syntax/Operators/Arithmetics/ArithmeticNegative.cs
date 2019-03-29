using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticNegative : UnaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.ANeg;

        public ArithmeticNegative(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}