using System.Collections.Generic;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Functions
{
    public class Next : ClrFunction
    {
        public Next() : base("next")
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

            IScriptType value = arguments[0];

            if (!(value is IEnumerator<IScriptType> enumerator))
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Builtin_Next_Arg1_NotIterator),
                    Localized_Python3_Entities.Builtin_Next_Arg1_NotIterator,
                    value.GetTypeName()
                );
            }

            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            if (arguments.Length == 2)
            {
                return arguments[1];
            }

            throw new RuntimeException(
                nameof(Localized_Python3_Entities.Builtin_Next_StopIteration),
                Localized_Python3_Entities.Builtin_Next_StopIteration
            );
        }
    }
}