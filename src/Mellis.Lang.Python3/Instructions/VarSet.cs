using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class VarSet : IOpCode
    {
        public VarSet(SourceReference source, string identifier)
        {
            Source = source;
            Identifier = identifier;
        }

        public SourceReference Source { get; }

        public string Identifier { get; }

        public void Execute(VM.PyProcessor processor)
        {
            var value = processor.PopValue();
            processor.SetVariable(Identifier, value);
        }

        public override string ToString()
        {
            return $"pop->{Identifier}";
        }
    }
}