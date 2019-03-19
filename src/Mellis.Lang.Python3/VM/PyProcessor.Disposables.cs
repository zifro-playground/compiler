using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        private void DisposeAllDisposables()
        {
            var exceptions = new List<Exception>();

            foreach (IDisposable disposable in _disposables)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            _disposables.Clear();

            if (exceptions.Count > 1)
            {
                // Reverse due to stacks first-in-last-out
                throw new AggregateException(Enumerable.Reverse(exceptions));
            }

            if (exceptions.Count == 1)
            {
                throw exceptions[0];
            }
        }

        public int DisposablesCount => _disposables.Count;

        internal void PushDisposable(IDisposable disposable)
        {
            _disposables.Push(disposable);
        }

        internal IDisposable PopDisposable()
        {
            return _disposables.Pop();
        }
    }
}