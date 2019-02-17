using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3
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
                var ex = new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped),
                    Localized_Python3_Interpreter.Ex_Scope_LastScopeNotPopped);

                State = ProcessState.Error;
                LastError = ex;

                throw ex;
            }
        }

        protected virtual void OnScopeChanged(IScopeContext e)
        {
            ScopeChanged?.Invoke(this, e);
        }
    }
}