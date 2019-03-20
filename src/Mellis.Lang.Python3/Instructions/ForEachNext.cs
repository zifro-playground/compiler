using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class ForEachNext : IOpCode
    {
        public SourceReference Source { get; }

        public int JumpTarget { get; }

        public ForEachNext(SourceReference source, int jumpTarget)
        {
            Source = source;
            JumpTarget = jumpTarget;
        }

        public void Execute(PyProcessor processor)
        {
            IScriptType scriptType = processor.PeekValue();

            if (!(scriptType is IEnumerator<IScriptType> enumerator))
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_ForEach_NextNotEnumerator),
                    Localized_Python3_Interpreter.Ex_ForEach_NextNotEnumerator
                );
            }

            bool next = enumerator.MoveNext();

            if (next)
            {
                processor.PushValue(enumerator.Current ?? processor.Factory.Null);
            }
            else
            {
                processor.JumpToInstruction(JumpTarget);
            }
        }

        public override string ToString()
        {
            return $"iter->next@{JumpTarget}";
        }
    }
}