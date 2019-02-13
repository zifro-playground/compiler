using System.Collections.Generic;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyScope : IScopeContext
    {
        private readonly Dictionary<string, IScriptType> _variables;

        public PyScope(IScopeContext parent)
        {
            _variables = new Dictionary<string, IScriptType>();
            ParentScope = parent;
        }

        public IScopeContext ParentScope { get; }

        public IReadOnlyDictionary<string, IScriptType> Variables => _variables;

        internal void SetVariable(string key, IScriptType value)
        {
            _variables[key] = value;
        }

        internal IScriptType GetVariable(string key)
        {
            return _variables.TryGetValue(key, out IScriptType value)
                ? value
                : null;
        }
    }
}