using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public class Assignment : Statement
    {
        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }

        public Assignment(SourceReference source,
            ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }
    }
}