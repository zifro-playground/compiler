using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class EmbeddedClrYieldingFunctionBase : ScriptTypeBase, IClrYieldingFunction
    {
        public IClrYieldingFunction Definition { get; }

        protected EmbeddedClrYieldingFunctionBase(
            IProcessor processor,
            IClrYieldingFunction definition,
            string name = null)
            : base(processor, name)
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
        public override bool TryConvert(Type type, out object value)
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

        IProcessor IEmbeddedType.Processor {
            set => Processor = value;
        }

        public string FunctionName => Definition.FunctionName;

        public void InvokeEnter(params IScriptType[] arguments)
        {
            Definition.InvokeEnter(arguments);
        }

        public IScriptType InvokeExit(IScriptType[] arguments, IScriptType returnValue)
        {
            return Definition.InvokeExit(arguments, returnValue);
        }

        #endregion
    }
}