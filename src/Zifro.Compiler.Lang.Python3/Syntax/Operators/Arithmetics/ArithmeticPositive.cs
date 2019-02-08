using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticPositive : UnaryOperator
    {
        public ArithmeticPositive(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}