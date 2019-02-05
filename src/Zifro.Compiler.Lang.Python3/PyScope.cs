using System.Collections.Generic;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyScope : IScopeContext
    {
        public IScopeContext ParentScope { get; }
        public IReadOnlyCollection<IScriptType> Variables { get; }
    }
}