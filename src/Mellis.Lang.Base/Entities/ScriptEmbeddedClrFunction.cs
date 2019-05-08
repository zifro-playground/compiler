using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptEmbeddedClrFunction : ScriptClrFunction
    {
        public IClrFunction Definition { get; }

        protected ScriptEmbeddedClrFunction(
            IProcessor processor,
            IClrFunction definition)
            : base(processor, definition.FunctionName)
        {
            Definition = definition;
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
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