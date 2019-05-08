using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyRangeType : PyType<PyRange>
    {
        public PyRangeType(IProcessor processor)
            : base(processor, Localized_Python3_Entities.Type_Range_Name)
        {
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            if (arguments.Length == 0)
            {
                throw new RuntimeTooFewArgumentsException(FunctionName, 1, arguments.Length);
            }

            int from = 0;
            int to;
            int step = 1;
            switch (arguments.Length)
            {
            case 1:
                to = GetIntegerArg(0);
                break;
            case 2:
                from = GetIntegerArg(0);
                to = GetIntegerArg(1);
                break;
            case 3:
                from = GetIntegerArg(0);
                to = GetIntegerArg(1);
                step = GetIntegerArg(2);

                if (step == 0)
                {
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg3_Zero),
                        Localized_Python3_Entities.Ex_RangeType_Ctor_Arg3_Zero
                    );
                }

                break;

            default:
                throw new RuntimeTooManyArgumentsException(FunctionName, 3, arguments.Length);
            }

            return new PyRange(Processor, from, to, step);

            int GetIntegerArg(int index)
            {
                if (!(arguments[index] is ScriptInteger intVal))
                {
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg_NotInteger),
                        Localized_Python3_Entities.Ex_RangeType_Ctor_Arg_NotInteger,
                        arguments[index].GetTypeName()
                    );
                }

                return intVal.Value;
            }
        }
    }
}