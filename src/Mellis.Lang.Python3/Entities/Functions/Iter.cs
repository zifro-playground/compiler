using System.Collections.Generic;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Functions
{
    public class Iter : ClrFunction
    {
        public Iter() : base("iter")
        {
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            if (arguments.Length > 2)
            {
                throw new RuntimeTooManyArgumentsException(FunctionName, 2, arguments.Length);
            }
            if (arguments.Length == 0)
            {
                throw new RuntimeTooFewArgumentsException(FunctionName, 1, arguments.Length);
            }
            if (arguments.Length == 2)
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Builtin_Iter_Arg2_NotYetImplemented),
                    Localized_Python3_Entities.Builtin_Iter_Arg2_NotYetImplemented
                );
            }

            IScriptType value = arguments[0];

            if (!(value is IEnumerable<IScriptType> enumerable))
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Builtin_Iter_Arg1_NotIterable),
                    Localized_Python3_Entities.Builtin_Iter_Arg1_NotIterable,
                    value.GetTypeName()
                );
            }

            IEnumerator<IScriptType> enumerator = enumerable.GetEnumerator();

            if (!(enumerator is IScriptType enumeratorScriptType))
            {
                enumeratorScriptType = new PyEnumeratorProxy(value.Processor, value, enumerator);
            }

            return enumeratorScriptType;
        }
    }
}