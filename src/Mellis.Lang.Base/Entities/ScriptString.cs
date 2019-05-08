using System;
using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    /// <summary>
    /// Basic functionality of a string value.
    /// </summary>
    public abstract class ScriptString : ScriptBaseType
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

        protected object[] GetErrorArgs(params object[] additional)
        {
            return GetErrorArgs().Concat(additional).ToArray();
        }

        protected object[] GetErrorArgs()
        {
            return new object[] {Value, Value.Length};
        }

        public override IScriptType GetIndex(IScriptType index)
        {
            switch (index)
            {
            case ScriptInteger indexInteger when indexInteger.Value >= 0 &&
                                                 indexInteger.Value < Value.Length:
                return Processor.Factory.Create(Value[indexInteger.Value]);

            case ScriptInteger indexInteger:
                throw new RuntimeException(
                    nameof(Localized_Base_Entities.Ex_String_IndexGet_OutOfRange),
                    Localized_Base_Entities.Ex_String_IndexGet_OutOfRange,
                    formatArgs: GetErrorArgs(indexInteger.Value));

            default:
                throw new RuntimeException(
                    nameof(Localized_Base_Entities.Ex_String_IndexGet_InvalidType),
                    Localized_Base_Entities.Ex_String_IndexGet_InvalidType,
                    formatArgs: GetErrorArgs(index.GetTypeName()));
            }
        }

        public override IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_IndexSet),
                Localized_Base_Entities.Ex_String_IndexSet,
                formatArgs: GetErrorArgs());
        }

        public override IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_PropertyGet),
                Localized_Base_Entities.Ex_String_PropertyGet,
                formatArgs: GetErrorArgs(property));
        }

        public override IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_PropertySet),
                Localized_Base_Entities.Ex_String_PropertySet,
                formatArgs: GetErrorArgs(property));
        }

        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptString rhsString:
                return Processor.Factory.Create(Value + rhsString.Value);
            default:
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_AddInvalidType),
                    Localized_Base_Entities.Ex_String_AddInvalidType,
                    formatArgs: GetErrorArgs(rhs.GetTypeName()));
            }
        }

        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_String_SubtractInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_String_MultiplyInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation),
                Localized_Base_Entities.Ex_String_DivideInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }
    }
}