using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ClrYieldingFunctionBase : ScriptTypeBase, IClrYieldingFunction
    {
        public ClrYieldingFunctionBase(
            IProcessor processor, string functionName, string name = null)
            : base(processor, name)
        {
            FunctionName = functionName;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public override bool TryCoerce(Type type, out object value)
        {
            value = default;
            return false;
        }

        IProcessor IEmbeddedType.Processor
        {
            set => Processor = value;
        }

        public string FunctionName { get; }

        public abstract void InvokeEnter(params IScriptType[] arguments);

        public abstract IScriptType InvokeExit(IScriptType[] arguments, IScriptType returnValue);
    }
}