using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyDoubleType : PyType<PyDouble>
    {
        public PyDoubleType(
            IProcessor processor,
            string name = null)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_Double_Name,
                name: name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyDoubleType(Processor, newName);
        }

        public override IScriptType Invoke(IScriptType[] arguments)
        {
            if (arguments.Length > 1)
                throw new RuntimeTooManyArgumentsException(
                    ClassName, 1, arguments.Length);

            if (arguments.Length == 0)
                return new PyDouble(Processor, 0);

            throw new SyntaxNotYetImplementedException(SourceReference.ClrSource);
        }
    }
}