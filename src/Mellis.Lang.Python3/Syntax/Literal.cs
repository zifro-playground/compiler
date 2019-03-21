using Mellis.Core.Entities;
using Mellis.Core.Interfaces;

namespace Mellis.Lang.Python3.Syntax
{
    public abstract class Literal : ExpressionNode
    {
        protected Literal(SourceReference source)
            : base(source)
        {
        }

        public abstract string GetTypeName();

        public abstract IScriptType ToScriptType(VM.PyProcessor processor);
    }

    public abstract class Literal<T> : Literal
    {
        protected Literal(SourceReference source, T value)
            : base(source)
        {
            Value = value;
        }

        public T Value { get; }
    }
}