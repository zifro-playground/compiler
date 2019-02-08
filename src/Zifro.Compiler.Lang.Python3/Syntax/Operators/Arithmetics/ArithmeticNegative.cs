using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
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