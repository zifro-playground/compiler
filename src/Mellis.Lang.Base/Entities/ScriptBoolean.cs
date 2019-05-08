using System;
using System.Linq;
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

        protected object[] GetErrorArgs(params object[] additional)
        {
            return GetErrorArgs().Concat(additional).ToArray();
        }

        protected object[] GetErrorArgs()
        {
            return new object[] {
                Value,
                GetLocalizedString()
            };
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
        
        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_AddInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_AddInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
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