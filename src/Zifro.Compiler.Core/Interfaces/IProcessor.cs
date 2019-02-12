using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;

namespace Zifro.Compiler.Core.Interfaces
{
    public interface IProcessor
    {
        IScriptTypeFactory Factory { get; }
        IScopeContext GlobalScope { get; }
        IScopeContext CurrentScope { get; }
        ProcessState State { get; }

        InterpreterException LastError { get; }

        void ContinueYieldedValue(IScriptType value);
        void WalkLine();
    }
}