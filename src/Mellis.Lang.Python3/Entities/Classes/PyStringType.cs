using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyStringType : PyType<PyString>
    {
        public PyStringType(
            IProcessor processor,
            string name = null)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_String_Name,
                name: name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyStringType(Processor, newName);
        }

        public override IScriptType Invoke(IScriptType[] arguments)
        {
            if (arguments.Length > 1)
                throw new RuntimeTooManyArgumentsException(
                    ClassName, 1, arguments.Length);

            if (arguments.Length == 0)
                return Processor.Factory.Create(string.Empty);

            return new PyString(Processor, arguments[0].ToString());
        }
    }
}