using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class VarGet : IOpCode
    {
        public VarGet(SourceReference source, string identifier)
        {
            Source = source;
            Identifier = identifier;
        }

        public SourceReference Source { get; }

        public string Identifier { get; }

        public void Execute(PyProcessor processor)
        {
            IScriptType value = processor.GetVariable(Identifier);
            processor.PushValue(value);
        }

        public override string ToString()
        {
            return $"push->${Identifier}";
        }
    }
}