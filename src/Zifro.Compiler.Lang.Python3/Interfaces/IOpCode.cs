using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Interfaces
{
    public interface IOpCode
    {
        SourceReference Source { get; }

        void Execute(PyProcessor processor);
    }
}