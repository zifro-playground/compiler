using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticDivide : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.Div;

        public ArithmeticDivide(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}