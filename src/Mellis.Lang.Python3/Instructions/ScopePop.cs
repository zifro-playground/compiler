using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class ScopePop : IOpCode
    {
        public ScopePop(SourceReference source)
        {
            Source = source;
        }

        public SourceReference Source { get; }

        public void Execute(PyProcessor processor)
        {
            processor.PopScope();
        }

        public override string ToString()
        {
            return "pop->$scope";
        }
    }
}