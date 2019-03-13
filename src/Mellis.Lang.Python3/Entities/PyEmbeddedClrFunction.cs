using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEmbeddedClrFunction : EmbeddedClrFunctionBase
    {
        public PyEmbeddedClrFunction(
            IProcessor processor,
            IClrFunction definition,
            string name = null)
            : base(processor, definition, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyEmbeddedClrFunction(Processor, Definition, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyType<PyEmbeddedClrFunction>(Processor, GetTypeName(), ClrFunctionConstructor);
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