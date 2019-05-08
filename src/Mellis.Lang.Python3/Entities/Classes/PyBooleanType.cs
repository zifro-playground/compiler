using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyBooleanType : PyType<PyBoolean>
    {
        public PyBooleanType(
            IProcessor processor)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_Boolean_Name)
        {
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            if (arguments.Length == 0)
            {
                return Processor.Factory.False;
            }

            if (arguments.Length > 1)
            {
                throw new RuntimeTooManyArgumentsException(ClassName, 1, arguments.Length);
            }

            return arguments[0].IsTruthy()
                ? Processor.Factory.True
                : Processor.Factory.False;
        }
    }
}