using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptEmbeddedClrYieldingFunction : ScriptClrYieldingFunction
    {
        public IClrYieldingFunction Definition { get; }

        protected ScriptEmbeddedClrYieldingFunction(
            IProcessor processor,
            IClrYieldingFunction definition,
            string name = null)
            : base(processor, definition.FunctionName, name)
        {
            Definition = definition;
        }

        /// <inheritdoc />
        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
        }

        /// <inheritdoc />
        public override bool IsTruthy()
        {
            return true;
        }

        /// <inheritdoc />
        public override bool TryCoerce(Type type, out object value)
        {
            value = default;
            return false;
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