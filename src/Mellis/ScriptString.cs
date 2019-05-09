using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    /// <summary>
    /// Basic functionality of a string value.
    /// </summary>
    public abstract class ScriptString : ScriptType
    {
        public string Value { get; }

        protected ScriptString(IProcessor processor, string value)
            : base(processor)
        {
            Value = value ?? string.Empty;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_String_Name;
        }

        public override bool IsTruthy()
        {
            return !string.IsNullOrEmpty(Value);
        }

        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptString rhsString:
                return Processor.Factory.Create(Value + rhsString.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            bool equals = rhs is ScriptString str && str.Value.Equals(Value);
            return Processor.Factory.Create(equals);
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            bool notEquals = !(rhs is ScriptString str) || !str.Value.Equals(Value);
            return Processor.Factory.Create(notEquals);
        }
    }
}