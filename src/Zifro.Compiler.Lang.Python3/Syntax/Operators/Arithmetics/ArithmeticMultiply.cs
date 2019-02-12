using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticMultiply : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.Mul;

        public ArithmeticMultiply(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}