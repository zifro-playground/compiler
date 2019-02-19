using System;
using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic functionality of a string value.
    /// </summary>
    public abstract class StringBase : ScriptTypeBase
    {
        public string Value { get; }

        protected StringBase(IProcessor processor, string value)
            : base(processor)
        {
            Value = value ?? string.Empty;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_Invoke),
                Localized_Base_Entities.Ex_String_Invoke,
                formatArgs: GetErrorArgs());
        }

        /// <inheritdoc/>
        public override IScriptType GetIndex(IScriptType index)
        {
            switch (index)
            {
                case IntegerBase indexInteger when indexInteger.Value >= 0 &&
                                                   indexInteger.Value < Value.Length:
                    return Processor.Factory.Create(Value[indexInteger.Value]);

                case IntegerBase indexInteger:
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

        /// <inheritdoc/>
        public override IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_IndexSet),
                Localized_Base_Entities.Ex_String_IndexSet,
                formatArgs: GetErrorArgs());
        }

        /// <inheritdoc/>
        public override IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_PropertyGet),
                Localized_Base_Entities.Ex_String_PropertyGet,
                formatArgs: GetErrorArgs(property));
        }

        /// <inheritdoc/>
        public override IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_PropertySet),
                Localized_Base_Entities.Ex_String_PropertySet,
                formatArgs: GetErrorArgs(property));
        }

        /// <inheritdoc/>
        public override bool TryConvert(Type type, out object value)
        {
            if (type == typeof(string))
            {
                value = Value;
                return true;
            }

            if (type == typeof(char) && Value?.Length >= 1)
            {
                value = Value[0];
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
                case StringBase rhsString:
                    return Processor.Factory.Create(Value + rhsString.Value);
                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_AddInvalidType),
                        Localized_Base_Entities.Ex_String_AddInvalidType,
                        formatArgs: GetErrorArgs(rhs.GetTypeName()));
            }
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_String_SubtractInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_String_MultiplyInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation),
                Localized_Base_Entities.Ex_String_DivideInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }
    }
}