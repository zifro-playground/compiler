using System.Collections.Generic;

namespace Zifro.Compiler.Core.Interfaces
{
    public interface IScopeContext
    {
        IScopeContext ParentScope { get; }
        IReadOnlyCollection<IValueType> Variables { get; }
    }
}