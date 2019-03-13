using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyClrFunction : ClrFunctionBase
    {
        public PyClrFunction(IProcessor processor, IClrFunction definition, string name = null)
            : base(processor, definition, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyClrFunction(Processor, Definition, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyType<PyClrFunction>(Processor, GetTypeName(), ClrFunctionConstructor);
        }

        private IScriptType ClrFunctionConstructor(IProcessor processor, IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Python3_Runtime.Ex_Type_CannotInstantiate),
                Localized_Python3_Runtime.Ex_Type_CannotInstantiate,
                GetTypeName());
        }
    }
}