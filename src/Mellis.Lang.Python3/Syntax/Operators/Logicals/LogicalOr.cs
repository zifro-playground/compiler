using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalOr : BinaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.LOr;

        public LogicalOr(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}