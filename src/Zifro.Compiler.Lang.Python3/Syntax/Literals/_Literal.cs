using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public abstract class Literal<T> : ExpressionNode
    {
        protected Literal(SourceReference source, string sourceText, T value)
            : base(source, sourceText)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}