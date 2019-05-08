using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyClrFunctionType : PyType<ScriptClrFunction>
    {
        public PyClrFunctionType(
            IProcessor processor)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_ClrFunction_Name)
        {
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