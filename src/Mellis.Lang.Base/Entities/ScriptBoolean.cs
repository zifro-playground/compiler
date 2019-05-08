using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    /// <summary>
    /// Basic functionality of a double value.
    /// </summary>
    public abstract class ScriptBoolean : ScriptBaseType
    {
        public bool Value { get; }

        protected ScriptBoolean(IProcessor processor, bool value)
            : base(processor)
        {
            Value = value;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Boolean_Name;
        }

        public string GetLocalizedString()
        {
            return Value
                ? Localized_Base_Entities.Type_Boolean_True
                : Localized_Base_Entities.Type_Boolean_False;
        }

        public override bool IsTruthy()
        {
            return Value;
        }

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            if (rhs is ScriptBoolean b && b.Value == Value)
            {
                return Processor.Factory.True;
            }

            return Processor.Factory.False;
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            if (rhs is ScriptBoolean b && b.Value == Value)
            {
                return Processor.Factory.False;
            }

            return Processor.Factory.True;
        }

        public override string ToString()
        {
            return Value ? bool.TrueString : bool.FalseString;
        }
    }
}