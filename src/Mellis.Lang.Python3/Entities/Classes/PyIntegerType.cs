using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyIntegerType : PyType<PyInteger>
    {
        public PyIntegerType(
            IProcessor processor,
            string name = null)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_Int_Name,
                name: name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyIntegerType(Processor, newName);
        }

        public override IScriptType Invoke(IScriptType[] arguments)
        {
            if (arguments.Length > 2)
                throw new RuntimeTooManyArgumentsException(
                    ClassName, 2, arguments.Length);

            if (arguments.Length == 0)
                return new PyInteger(Processor, 0);

            throw new SyntaxNotYetImplementedException(SourceReference.ClrSource);
        }
    }
}