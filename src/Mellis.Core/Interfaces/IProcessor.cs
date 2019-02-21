using Mellis.Core.Entities;
using Mellis.Core.Exceptions;

namespace Mellis.Core.Interfaces
{
    public interface IProcessor
    {
        IScriptTypeFactory Factory { get; }
        IScopeContext GlobalScope { get; }
        IScopeContext CurrentScope { get; }
        ProcessState State { get; }
        SourceReference CurrentSource { get; }

        InterpreterException LastError { get; }

        void ContinueYieldedValue(IScriptType value);
        void WalkLine();

        void AddBuiltin(params IClrFunction[] builtinList);
    }
}