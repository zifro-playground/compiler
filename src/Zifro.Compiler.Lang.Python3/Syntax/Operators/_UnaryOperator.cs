using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Common for negating operators such as the "not" (!value), "binary not" (~value)
    /// </summary>
    public abstract class UnaryOperator : ExpressionNode
    {
        protected UnaryOperator(SourceReference source, string sourceText, ExpressionNode operand)
            : base(source, sourceText)
        {
            Operand = operand;
        }

        public ExpressionNode Operand { get; set; }
    }
}