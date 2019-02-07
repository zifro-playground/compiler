using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Common two part operators, such as "and" (a && b), "or" (a || b), "xor" (a ^ b)
    /// </summary>
    public abstract class BinaryOperator : ExpressionNode
    {
        protected BinaryOperator(SourceReference source, ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }
    }
}