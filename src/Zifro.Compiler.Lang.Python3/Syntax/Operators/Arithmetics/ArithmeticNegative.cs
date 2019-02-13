using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
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