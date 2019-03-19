using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class EmbeddedClrFunctionBase : ClrFunctionBase
    {
        public IClrFunction Definition { get; }

        protected EmbeddedClrFunctionBase(
            IProcessor processor,
            IClrFunction definition,
            string name = null)
            : base(processor, definition.FunctionName, name)
        {
            Definition = definition;
        }

        /// <inheritdoc />
        public override IScriptType Invoke(IScriptType[] arguments)
        {
            return Definition.Invoke(arguments);
        }

        public override string ToString()
        {
            return string.Format(
                format: Localized_Base_Entities.Type_ClrFunction_ToString,
                arg0: Definition.FunctionName
            );
        }
    }
}