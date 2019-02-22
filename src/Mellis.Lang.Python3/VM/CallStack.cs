using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.VM
{
    public class CallStack
    {
        public CallStack(
            SourceReference source,
            PyScope scope,
            int returnAddress)
        {
            Source = source;
            Scope = scope;
            ReturnAddress = returnAddress;
        }

        public string FunctionName { get; set; }
        public SourceReference Source { get; }
        public PyScope Scope { get; }
        public int ReturnAddress { get; }
    }
}