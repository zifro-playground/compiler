using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyType : PyType<object>
    {
        public PyType(
            IProcessor processor,
            string name = null)
            : base(processor, Localized_Python3_Entities.Type_Type_Name, BaseTypeConstructor, name)
        {
        }

        private static IScriptType BaseTypeConstructor(IProcessor processor, IScriptType[] arguments)
        {
            if (arguments.Length > 1)
                throw new RuntimeTooManyArgumentsException(
                    Localized_Python3_Entities.Type_Type_Name,
                    1, arguments.Length);

            if (arguments.Length < 1)
                throw new RuntimeTooFewArgumentsException(
                    Localized_Python3_Entities.Type_Type_Name,
                    1, arguments.Length);

            return arguments[0].GetTypeDef();
        }
    }
}