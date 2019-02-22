using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.VM
{
    public class CallStack
    {
        public CallStack(SourceReference source,
            string functionName,
            int returnAddress)
        {
            Source = source;
            ReturnAddress = returnAddress;
            FunctionName = functionName;
        }

        public string FunctionName { get; }
        public SourceReference Source { get; }
        public int ReturnAddress { get; }
    }
}