using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Interfaces
{
    public interface IOpCode
    {
        SourceReference Source { get; }

        void Execute(VM.PyProcessor processor);
    }
}