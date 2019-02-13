using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryOr : BinaryOperator
    {
        public override OperatorCode OpCode => OperatorCode.BOr;

        public BinaryOr(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}