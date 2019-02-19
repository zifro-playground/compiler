using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Tools.Extensions;

namespace Mellis.Lang.Base.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Basic functionality of a double value.
    /// </summary>
    public abstract class DoubleBase : ScriptTypeBase
    {
        public double Value { get; }

        protected DoubleBase(IProcessor processor, double value)
            : base(processor)
        {
            Value = value;
        }

        /// <inheritdoc />
        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Double_Name;
        }

        /// <inheritdoc />
        public override IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_Invoke),
                Localized_Base_Entities.Ex_Double_Invoke,
                Value);
        }

        /// <inheritdoc />
        public override IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_IndexGet),
                Localized_Base_Entities.Ex_Double_IndexGet,
                Value);
        }

        /// <inheritdoc />
        public override IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_IndexSet),
                Localized_Base_Entities.Ex_Double_IndexSet,
                Value);
        }

        /// <inheritdoc />
        public override IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_PropertyGet),
                Localized_Base_Entities.Ex_Double_PropertyGet,
                Value,
                property);
        }

        /// <inheritdoc />
        public override IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_PropertySet),
                Localized_Base_Entities.Ex_Double_PropertySet,
                Value,
                property);
        }

        /// <inheritdoc />
        public override bool TryConvert(Type type, out object value)
        {
            if (type == typeof(double))
            {
                value = (double) Value;
                return true;
            }

            if (type == typeof(decimal))
            {
                value = (decimal) Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public override bool IsTruthy()
        {
            return !Value.Equals(0d);
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
                case DoubleBase rhsDouble:
                    return Processor.Factory.CreateAppropriate(Value + rhsDouble.Value);

                case IntegerBase rhsInt:
                    return Processor.Factory.CreateAppropriate(Value + rhsInt.Value);

                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Double_AddInvalidType),
                        Localized_Base_Entities.Ex_Double_AddInvalidType,
                        Value, rhs.GetTypeName());
            }
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            switch (rhs)
            {
                case DoubleBase rhsDouble:
                    return Processor.Factory.CreateAppropriate(Value - rhsDouble.Value);

                case IntegerBase rhsInt:
                    return Processor.Factory.CreateAppropriate(Value - rhsInt.Value);

                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Double_SubtractInvalidType),
                        Localized_Base_Entities.Ex_Double_SubtractInvalidType,
                        Value, rhs.GetTypeName());
            }
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInt:
                    return Processor.Factory.CreateAppropriate(Value * rhsInt.Value);

                case DoubleBase rhsDouble:
                    return Processor.Factory.CreateAppropriate(Value * rhsDouble.Value);

                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Double_MultiplyInvalidType),
                        Localized_Base_Entities.Ex_Double_MultiplyInvalidType,
                        Value, rhs.GetTypeName());
            }
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger when rhsInteger.Value.Equals(0):
                case DoubleBase rhsDouble when rhsDouble.Value.Equals(0d):
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                        Localized_Base_Entities.Ex_Math_DivideByZero);

                case DoubleBase rhsDouble:
                    return Processor.Factory.CreateAppropriate(Value / rhsDouble.Value);

                case IntegerBase rhsInt:
                    return Processor.Factory.CreateAppropriate(Value / rhsInt.Value);

                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Double_DivideInvalidType),
                        Localized_Base_Entities.Ex_Double_DivideInvalidType,
                        Value, rhs.GetTypeName());
            }
        }
    }
}