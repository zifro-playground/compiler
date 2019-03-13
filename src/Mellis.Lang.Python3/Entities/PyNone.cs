using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyNone : NullBase
    {
        public PyNone(IProcessor processor, string name = null)
            : base(processor, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyNone(Processor, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyType<PyNone>(Processor, GetTypeName(), NoneConstructor);
        }

        private IScriptType NoneConstructor(IProcessor processor, IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Python3_Runtime.Ex_Type_CannotInstantiate),
                Localized_Python3_Runtime.Ex_Type_CannotInstantiate,
                GetTypeName());
        }

        public override string ToString()
        {
            return "None";
        }
    }
}