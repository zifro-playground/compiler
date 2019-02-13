using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor
    {
        public event EventHandler<ProcessState> ProcessEnded;

        public event EventHandler<IScopeContext> ScopeChanged;

        protected virtual void OnProcessEnded(ProcessState e)
        {
            ProcessEnded?.Invoke(this, e);

            // Only check on clean end, ignore if ended with error
            if (e == ProcessState.Ended && GlobalScope != CurrentScope)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped),
                    Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped);
            }
        }

        protected virtual void OnScopeChanged(IScopeContext e)
        {
            ScopeChanged?.Invoke(this, e);
        }
    }
}