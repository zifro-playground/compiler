using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalAnd : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.LAnd;

        public LogicalAnd(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}