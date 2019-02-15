using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class Jump : IOpCode
    {
        public Jump(SourceReference source, Label label)
        {
            Source = source;
            Label = label;
        }

        public SourceReference Source { get; }

        public Label Label { get; }

        public virtual void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }
    }
}