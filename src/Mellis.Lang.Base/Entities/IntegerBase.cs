using System;
using System.Globalization;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Tools.Extensions;

namespace Mellis.Lang.Base.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic functionality of an integer value.
    /// </summary>
    public abstract class IntegerBase : ScriptTypeBase
    {
        public int Value { get; }

        protected IntegerBase(IProcessor processor, int value)
            : base(processor)
        {
            Value = value;
        }

        /// <inheritdoc/>
        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Int_Name;
        }

        public override bool IsTruthy()
        {
            return !Value.Equals(0);
        }

        /// <inheritdoc/>
        public override IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_Invoke),
                Localized_Base_Entities.Ex_Int_Invoke,
                Value);
        }

        /// <inheritdoc/>
        public override IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_IndexGet),
                Localized_Base_Entities.Ex_Int_IndexGet,
                Value);
        }

        /// <inheritdoc/>
        public override IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_IndexSet),
                Localized_Base_Entities.Ex_Int_IndexSet,
                Value);
        }

        /// <inheritdoc/>
        public override IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_PropertyGet),
                Localized_Base_Entities.Ex_Int_PropertyGet,
                Value,
                property);
        }

        /// <inheritdoc/>
        public override IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_PropertySet),
                Localized_Base_Entities.Ex_Int_PropertySet,
                Value,
                property);
        }

        /// <inheritdoc/>
        public override bool TryConvert(Type type, out object value)
        {
            if (type == typeof(int))
            {
                value = Value;
                return true;
            }

            if (type == typeof(long))
            {
                value = (long) Value;
                return true;
            }

            if (type == typeof(double))
            {
                value = (double) Value;
                return true;
            }

            if (type == typeof(float))
            {
                value = (float) Value;
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

        /// <inheritdoc/>
        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInt:
                    return Processor.Factory.Create(Value + rhsInt.Value);
                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Int_AddInvalidType),
                        Localized_Base_Entities.Ex_Int_AddInvalidType,
                        Value, rhs?.GetTypeName() ?? "null");
            }
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInt:
                    return Processor.Factory.Create(Value - rhsInt.Value);
                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Int_SubtractInvalidType),
                        Localized_Base_Entities.Ex_Int_SubtractInvalidType,
                        Value, rhs?.GetTypeName() ?? "null");
            }
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInt:
                    return Processor.Factory.Create(Value * rhsInt.Value);

                case DoubleBase rhsDouble:
                    return Processor.Factory.CreateAppropriate(Value * rhsDouble.Value);

                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Int_MultiplyInvalidType),
                        Localized_Base_Entities.Ex_Int_MultiplyInvalidType,
                        Value, rhs?.GetTypeName() ?? "null");
            }
        }

        /// <inheritdoc/>
        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInt when rhsInt.Value.Equals(0):
                case DoubleBase rhsDouble when rhsDouble.Value.Equals(0):
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                        Localized_Base_Entities.Ex_Math_DivideByZero);

                case IntegerBase rhsInt:
                    return Processor.Factory.CreateAppropriate(Value / (double) rhsInt.Value);

                case DoubleBase rhsDouble:
                    return Processor.Factory.CreateAppropriate(Value / rhsDouble.Value);

                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Int_DivideInvalidType),
                        Localized_Base_Entities.Ex_Int_DivideInvalidType,
                        Value, rhs?.GetTypeName() ?? "null");
            }
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticModulus(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value % rhsInteger.Value);
                case DoubleBase rhsDouble:
                    return rhsDouble.ArithmeticModulus(this);
                default:
                    throw InvalidType(rhs, "%");
            }
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticExponent(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Math.Pow(Value, rhsInteger.Value));
                case DoubleBase rhsDouble:
                    return rhsDouble.ArithmeticExponent(this);
                default:
                    throw InvalidType(rhs, "**");
            }
        }

        /// <inheritdoc />
        public override IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value / rhsInteger.Value);
                case DoubleBase rhsDouble:
                    return rhsDouble.ArithmeticFloorDivide(this);
                default:
                    throw InvalidType(rhs, "//");
            }
        }

        /// <inheritdoc />
        public override IScriptType CompareEqual(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value.Equals(rhsInteger.Value));
                case DoubleBase rhsDouble:
                    return rhsDouble.CompareEqual(this);
                default:
                    return Processor.Factory.False;
            }
        }

        /// <inheritdoc />
        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(!Value.Equals(rhsInteger.Value));
                case DoubleBase rhsDouble:
                    return rhsDouble.CompareNotEqual(this);
                default:
                    return Processor.Factory.True;
            }
        }

        /// <inheritdoc />
        public override IScriptType CompareGreaterThan(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value > rhsInteger.Value);
                case DoubleBase rhsDouble:
                    return rhsDouble.CompareGreaterThan(this);
                default:
                    throw InvalidType(rhs, ">");
            }
        }

        /// <inheritdoc />
        public override IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value >= rhsInteger.Value);
                case DoubleBase rhsDouble:
                    return rhsDouble.CompareGreaterThanOrEqual(this);
                default:
                    throw InvalidType(rhs, ">=");
            }
        }

        /// <inheritdoc />
        public override IScriptType CompareLessThan(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value < rhsInteger.Value);
                case DoubleBase rhsDouble:
                    return rhsDouble.CompareLessThan(this);
                default:
                    throw InvalidType(rhs, "<");
            }
        }

        /// <inheritdoc />
        public override IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger:
                    return Processor.Factory.Create(Value <= rhsInteger.Value);
                case DoubleBase rhsDouble:
                    return rhsDouble.CompareLessThanOrEqual(this);
                default:
                    throw InvalidType(rhs, "<=");
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.CurrentCulture);
        }
    }
}