using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticModulus : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.Mod;

        public ArithmeticModulus(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}