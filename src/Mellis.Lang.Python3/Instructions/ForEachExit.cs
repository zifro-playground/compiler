using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class ForEachExit : IOpCode
    {
        public SourceReference Source { get; }

        public ForEachExit(SourceReference source)
        {
            Source = source;
        }

        public void Execute(PyProcessor processor)
        {
            IScriptType value = processor.PopValue();

            if (!(value is IEnumerator<IScriptType> enumerator))
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_ForEach_ExitNotEnumerator),
                    Localized_Python3_Interpreter.Ex_ForEach_ExitNotEnumerator
                );
            }

            if (!ReferenceEquals(processor.PeekDisposable(), enumerator))
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_ForEach_ExitNotSameDisposable),
                    Localized_Python3_Interpreter.Ex_ForEach_ExitNotSameDisposable
                );
            }

            processor.PopDisposable();
            enumerator.Dispose();
        }

        public override string ToString()
        {
            return "iter->dispose";
        }
    }
}