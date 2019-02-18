using System.Collections.Generic;

namespace Mellis.Core.Interfaces
{
    public interface IScopeContext
    {
        IScopeContext ParentScope { get; }
        IReadOnlyDictionary<string, IScriptType> Variables { get; }
    }
}