using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticFloor : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.Flr;

        public ArithmeticFloor(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}