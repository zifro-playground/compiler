using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticMultiply : BinaryOperator
    {
        public ArithmeticMultiply(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override OperatorCode OpCode => throw new System.NotImplementedException();
    }
}