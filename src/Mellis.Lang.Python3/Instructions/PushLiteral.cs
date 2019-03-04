using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Instructions
{
    public class PushLiteral<TValue> : IOpCode
    {
        public SourceReference Source { get; }

        public Literal<TValue> Literal { get; }

        public PushLiteral(Literal<TValue> literal)
        {
            Literal = literal;
            Source = literal.Source;
        }

        public void Execute(VM.PyProcessor processor)
        {
            processor.PushValue(Literal.ToScriptType(processor));
        }

        public override string ToString()
        {
            return $"push->{{{Literal}}}";
        }
    }
}