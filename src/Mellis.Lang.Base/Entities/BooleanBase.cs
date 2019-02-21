using System;
using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Basic functionality of a double value.
    /// </summary>
    public abstract class BooleanBase : ScriptTypeBase
    {
        public bool Value { get; }

        protected BooleanBase(IProcessor processor, bool value, string name = null)
            : base(processor, name)
        {
            Value = value;
        }

        /// <inheritdoc />
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
            return new object[]
            {
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

        /// <inheritdoc />
        public override bool IsTruthy()
        {
            return Value;
        }

        /// <inheritdoc />
        public override IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_Invoke),
                Localized_Base_Entities.Ex_Boolean_Invoke,
                formatArgs: GetErrorArgs());
        }

        /// <inheritdoc />
        public override IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_IndexGet),
                Localized_Base_Entities.Ex_Boolean_IndexGet,
                formatArgs: GetErrorArgs());
        }

        /// <inheritdoc />
        public override IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_IndexSet),
                Localized_Base_Entities.Ex_Boolean_IndexSet,
                formatArgs: GetErrorArgs());
        }

        /// <inheritdoc />
        public override IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_PropertyGet),
                Localized_Base_Entities.Ex_Boolean_PropertyGet,
                formatArgs: GetErrorArgs(property));
        }

        /// <inheritdoc />
        public override IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_PropertySet),
                Localized_Base_Entities.Ex_Boolean_PropertySet,
                formatArgs: GetErrorArgs(property));
        }

        /// <inheritdoc />
        public override bool TryConvert(Type type, out object value)
        {
            if (type == typeof(bool))
            {
                value = Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_AddInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_AddInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public override IScriptType CompareEqual(IScriptType rhs)
        {
            if (rhs is BooleanBase b && b.Value == Value)
                return Processor.Factory.True;

            return Processor.Factory.False;
        }

        /// <inheritdoc />
        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            if (rhs is BooleanBase b && b.Value == Value)
                return Processor.Factory.False;

            return Processor.Factory.True;
        }

        public override string ToString()
        {
            return Value ? bool.TrueString : bool.FalseString;
        }
    }
}