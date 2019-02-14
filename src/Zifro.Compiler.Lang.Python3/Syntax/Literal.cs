using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public abstract class Literal<T> : ExpressionNode
    {
        protected Literal(SourceReference source, T value)
            : base(source)
        {
            Value = value;
        }

        public T Value { get; set; }

        public abstract IScriptType ToScriptType(PyProcessor processor);
    }
}