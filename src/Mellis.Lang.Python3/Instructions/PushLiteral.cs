using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Instructions
{
    public class PushLiteral : IOpCode
    {
        public SourceReference Source { get; }

        public Literal Literal { get; }

        public PushLiteral(Literal literal)
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