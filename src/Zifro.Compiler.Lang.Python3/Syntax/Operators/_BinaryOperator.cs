using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    public abstract class BinaryOperator : ExpressionNode
    {
        protected BinaryOperator(SourceReference source, string sourceText, ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source, sourceText)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }
    }
}