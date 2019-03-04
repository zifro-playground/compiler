using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        public void PushScope()
        {
            var newScope = new PyScope(CurrentScope);

            _scopesStack.Add(newScope);
            OnScopeChanged(newScope);
        }

        public void PopScope()
        {
            if (_scopesStack.Count == 0)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_Scope_PopGlobal),
                    Localized_Python3_Interpreter.Ex_Scope_PopGlobal);
            }

            _scopesStack.RemoveAt(_scopesStack.Count - 1);
            OnScopeChanged(CurrentScope);
        }

        public void SetGlobalVariable(string key, IScriptType value)
        {
            _globalScope.SetVariable(key, value);
        }

        public void SetVariable(string key, IScriptType value)
        {
            var scope = (PyScope)CurrentScope;

            scope.SetVariable(key, value);
        }

        public IScriptType GetVariable(string key)
        {
            PyScope scope = GetScopeWithVariableOrNull(key);

            if (scope == null)
                throw new RuntimeException(
                    nameof(Localized_Python3_Runtime.Ex_Variable_NotDefined),
                    Localized_Python3_Runtime.Ex_Variable_NotDefined,
                    key);

            return scope.GetVariable(key);
        }

        private PyScope GetScopeWithVariableOrNull(string key)
        {
            PyScope scope = _scopesStack.LastOrDefault()
                            ?? _globalScope;

            while (scope != null)
            {
                if (scope.HasVariable(key))
                    return scope;

                scope = (PyScope)scope.ParentScope;
            }

            return null;
        }
    }
}