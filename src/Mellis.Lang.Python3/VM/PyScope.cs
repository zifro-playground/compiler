using System.Collections.Generic;
using System.Linq;
using Mellis.Core.Interfaces;

namespace Mellis.Lang.Python3.VM
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

        public string[] ListVariableNames()
        {
            return Variables.Keys.ToArray();
        }

        public string[] ListVariableNamesUpwards()
        {
            IEnumerable<string> names = Variables.Keys;
            IScopeContext parent = ParentScope;

            while (!(parent is null))
            {
                names = names.Concat(parent.Variables.Keys);
                parent = parent.ParentScope;
            }

            return names.Distinct().ToArray();
        }

        internal void SetVariableNoCopyUsingName(IScriptType value)
        {
            _variables[value.Name] = value;
        }

        internal void SetVariable(string key, IScriptType value)
        {
            _variables[key] = value.Copy(key);
        }

        internal IScriptType GetVariable(string key)
        {
            return _variables.TryGetValue(key, out IScriptType value)
                ? value
                : null;
        }

        internal bool HasVariable(string key)
        {
            return _variables.ContainsKey(key);
        }
    }
}