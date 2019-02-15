using System;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Base.Resources;
using Zifro.Compiler.Tools.Extensions;

namespace Zifro.Compiler.Lang.Base.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic functionality of an integer value.
    /// </summary>
    public abstract class IntegerBase : IScriptType
    {
        /// <inheritdoc/>
        public abstract IScriptType GetTypeDef();

        /// <inheritdoc/>
        public IProcessor Processor { get; }

        public int Value { get; }

        protected IntegerBase(IProcessor processor, int value)
        {
            Processor = processor;
            Value = value;
        }

        /// <inheritdoc/>
        public virtual string GetTypeName()
        {
            return Localized_Base_Entities.Type_Int_Name;
        }

        public virtual bool IsTruthy()
        {
            return !Value.Equals(0);
        }

        /// <inheritdoc/>
        public virtual IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_Invoke),
                Localized_Base_Entities.Ex_Int_Invoke,
                Value);
        }

        /// <inheritdoc/>
        public virtual IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_IndexGet),
                Localized_Base_Entities.Ex_Int_IndexGet,
                Value);
        }

        /// <inheritdoc/>
        public virtual IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_IndexSet),
                Localized_Base_Entities.Ex_Int_IndexSet,
                Value);
        }

        /// <inheritdoc/>
        public virtual IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_PropertyGet),
                Localized_Base_Entities.Ex_Int_PropertyGet,
                Value,
                property);
        }

        /// <inheritdoc/>
        public virtual IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Int_PropertySet),
                Localized_Base_Entities.Ex_Int_PropertySet,
                Value,
                property);
        }

        /// <inheritdoc/>
        public bool TryConvert<T>(out T value)
        {
            if (TryConvert(typeof(T), out object boxed))
            {
                value = (T) boxed;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public bool TryConvert(Type type, out object value)
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
        public IScriptType ArithmeticUnaryPositive()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticUnaryNegative()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticAdd(IScriptType rhs)
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
        public IScriptType ArithmeticSubtract(IScriptType rhs)
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
        public IScriptType ArithmeticMultiply(IScriptType rhs)
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
        public IScriptType ArithmeticDivide(IScriptType rhs)
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

        /// <inheritdoc/>
        public IScriptType ArithmeticModulus(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticExponent(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType CompareEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType CompareNotEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType CompareGreaterThan(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType CompareLessThan(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType BinaryNot()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType BinaryAnd(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType BinaryOr(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType BinaryXor(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType BinaryLeftShift(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType BinaryRightShift(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType MemberIn(IScriptType lhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType MemberNotIn(IScriptType lhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType IdentityIs(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType IdentityIsNot(IScriptType rhs)
        {
            throw new NotImplementedException();
        }
    }
}