using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalAnd : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.LAnd;

        public LogicalAnd(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}