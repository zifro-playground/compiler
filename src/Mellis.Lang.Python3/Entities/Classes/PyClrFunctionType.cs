using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyClrFunctionType : PyType<ClrFunctionBase>
    {
        public PyClrFunctionType(
            IProcessor processor, string name = null)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_ClrFunction_Name,
                name: name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyClrFunctionType(Processor, newName);
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Python3_Runtime.Ex_Type_CannotInstantiate),
                Localized_Python3_Runtime.Ex_Type_CannotInstantiate,
                ClassName);
        }
    }
}