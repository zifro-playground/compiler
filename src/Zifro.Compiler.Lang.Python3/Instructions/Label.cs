using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class Label : IOpCode
    {
        public Label(SourceReference source)
        {
            Source = source;
            OpCodeIndex = -1;
        }

        public SourceReference Source { get; }

        public int OpCodeIndex { get; internal set; }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }
    }
}