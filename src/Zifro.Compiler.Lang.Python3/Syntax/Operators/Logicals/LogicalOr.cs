using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalOr : BinaryOperator
    {
        public LogicalOr(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override OperatorCode OpCode => throw new System.NotImplementedException();
    }
}