using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class VarSet : IOpCode
    {
        public VarSet(SourceReference source)
        {
            Source = source;
        }

        public SourceReference Source { get; }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }
    }
}