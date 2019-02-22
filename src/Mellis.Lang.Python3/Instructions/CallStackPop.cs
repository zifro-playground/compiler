using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class CallStackPop : IOpCode
    {
        public SourceReference Source { get; }

        public CallStackPop(SourceReference source)
        {
            Source = source;
        }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }
    }
}