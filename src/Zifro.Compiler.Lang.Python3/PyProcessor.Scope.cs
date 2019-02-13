using System;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor
    {
        public void PushScope()
        {
            _scopesStack.Add(new PyScope(CurrentScope));
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
        }

        public void SetGlobalVariable(string key, IScriptType value)
        {
            _globalScope.SetVariable(key, value);
        }

        public IScriptType GetGlobalVariable(string key)
        {
            throw new System.NotImplementedException();
        }

        public void SetVariable(string key, IScriptType value)
        {
        }

        public IScriptType GetVariable(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}