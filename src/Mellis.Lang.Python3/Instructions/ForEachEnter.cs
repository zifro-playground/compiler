using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class ForEachEnter : IOpCode
    {
        public SourceReference Source { get; }

        public ForEachEnter(SourceReference source)
        {
            Source = source;
        }

        public void Execute(PyProcessor processor)
        {
            IScriptType value = processor.PopValue();

            if (!(value is IEnumerable<IScriptType> enumerable))
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Runtime.Ex_ForEach_NotIterable),
                    Localized_Python3_Runtime.Ex_ForEach_NotIterable,
                    value.GetTypeName()
                );
            }

            IEnumerator<IScriptType> enumerator = enumerable.GetEnumerator();

            if (!(enumerator is IScriptType enumeratorScriptType))
            {
                enumeratorScriptType = new PyEnumeratorWrapper(value.Processor, value, enumerator);
            }

            processor.PushValue(enumeratorScriptType);
        }

        public override string ToString()
        {
            return "iter->prep";
        }
    }
}