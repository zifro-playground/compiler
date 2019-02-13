using System.Collections.Generic;

namespace Zifro.Compiler.Core.Interfaces
{
    public interface IScopeContext
    {
        IScopeContext ParentScope { get; }
        IReadOnlyDictionary<string, IScriptType> Variables { get; }
    }
}