using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    public abstract class ScriptClrYieldingFunctionProxy : ScriptClrYieldingFunction
    {
        public IClrYieldingFunction Definition { get; }

        protected ScriptClrYieldingFunctionProxy(
            IProcessor processor,
            IClrYieldingFunction definition)
            : base(processor, definition.FunctionName)
        {
            Definition = definition;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public override string ToString()
        {
            return string.Format(
                format: Localized_Base_Entities.Type_ClrFunction_ToString,
                arg0: Definition.FunctionName
            );
        }

        #region IClrYieldingFunction implementation

        public override void InvokeEnter(params IScriptType[] arguments)
        {
            Definition.InvokeEnter(arguments);
        }

        public override IScriptType InvokeExit(IScriptType[] arguments, IScriptType returnValue)
        {
            return Definition.InvokeExit(arguments, returnValue);
        }

        #endregion
    }
}