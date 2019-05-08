using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptClrYieldingFunction : ScriptBaseType, IClrYieldingFunction
    {
        public ScriptClrYieldingFunction(
            IProcessor processor, string functionName)
            : base(processor)
        {
            FunctionName = functionName;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
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