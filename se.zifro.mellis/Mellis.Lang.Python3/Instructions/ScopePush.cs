using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class ScopePush : IOpCode
    {
        public ScopePush(SourceReference source)
        {
            Source = source;
        }

        public SourceReference Source { get; }

        public void Execute(PyProcessor processor)
        {
            processor.PushScope();
        }

        public override string ToString()
        {
            return "push->$scope";
        }
    }
}