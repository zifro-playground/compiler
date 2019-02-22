using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class VarPop : IOpCode
    {
        public SourceReference Source { get; }

        public VarPop(SourceReference source)
        {
            Source = source;
        }

        public void Execute(PyProcessor processor)
        {
            processor.PopValue();
        }

        public override string ToString()
        {
            return "pop";
        }
    }
}