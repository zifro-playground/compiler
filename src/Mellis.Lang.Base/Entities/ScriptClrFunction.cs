using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptClrFunction : ScriptBaseType, IClrFunction
    {
        public ScriptClrFunction(
            IProcessor processor,
            string functionName)
            : base(processor)
        {
            FunctionName = functionName;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
        }
        
        IProcessor IEmbeddedType.Processor
        {
            set => Processor = value;
        }

        public string FunctionName { get; }

        public abstract IScriptType Invoke(params IScriptType[] arguments);
    }
}