using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Resources;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyStringType : PyType<PyString>
    {
        public PyStringType(
            IProcessor processor)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_String_Name)
        {
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            if (arguments.Length > 1)
            {
                throw new RuntimeTooManyArgumentsException(
                    ClassName, 1, arguments.Length);
            }

            if (arguments.Length == 0)
            {
                return Processor.Factory.Create(string.Empty);
            }

            return new PyString(Processor, arguments[0].ToString());
        }
    }
}