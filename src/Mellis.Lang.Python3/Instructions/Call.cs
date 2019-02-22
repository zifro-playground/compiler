using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class Call : IOpCode
    {
        public int ArgumentCount { get; }

        public int ReturnAddress { get; }

        public SourceReference Source { get; }

        public Call(SourceReference source, int argumentCount, int returnAddress)
        {
            Source = source;
            ArgumentCount = argumentCount;
            ReturnAddress = returnAddress;
        }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }
    }
}