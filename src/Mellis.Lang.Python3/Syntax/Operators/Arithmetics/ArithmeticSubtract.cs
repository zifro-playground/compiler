using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticSubtract : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.ASub;

        public ArithmeticSubtract(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}