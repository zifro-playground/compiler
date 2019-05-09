using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    public abstract class ScriptClrFunctionProxy : ScriptClrFunction
    {
        public IClrFunction Definition { get; }

        protected ScriptClrFunctionProxy(
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

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            return Processor.Factory.Create(rhs == this ||
                                            (rhs is ScriptClrFunctionProxy proxy &&
                                             proxy.Definition == Definition));
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            return Processor.Factory.Create(rhs != this &&
                                            (!(rhs is ScriptClrFunctionProxy proxy) ||
                                             proxy.Definition != Definition));
        }
    }
}