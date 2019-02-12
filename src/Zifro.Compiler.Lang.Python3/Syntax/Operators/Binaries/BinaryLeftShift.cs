using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binaries
{
    public class BinaryLeftShift : BinaryOperator
    {
        public BinaryLeftShift(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override OperatorCode OpCode => throw new System.NotImplementedException();
    }
}