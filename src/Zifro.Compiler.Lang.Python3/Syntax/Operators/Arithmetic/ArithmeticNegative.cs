using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetic
{
    public class ArithmeticNegative : UnaryOperator
    {
        public ArithmeticNegative(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}