using System;
using System.Linq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Base.Resources;

namespace Zifro.Compiler.Lang.Base.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Basic functionality of a double value.
    /// </summary>
    public abstract class BooleanBase : IScriptType
    {
        /// <inheritdoc />
        public abstract IScriptType GetTypeDef();

        /// <inheritdoc />
        public IProcessor Processor { get; }

        public bool Value { get; }

        protected BooleanBase(IProcessor processor, bool value)
        {
            Processor = processor;
            Value = value;
        }

        /// <inheritdoc />
        public virtual string GetTypeName()
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
        public virtual IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_Invoke),
                Localized_Base_Entities.Ex_Boolean_Invoke,
                values: GetErrorArgs());
        }

        /// <inheritdoc />
        public virtual IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_IndexGet),
                Localized_Base_Entities.Ex_Boolean_IndexGet,
                values: GetErrorArgs());
        }

        /// <inheritdoc />
        public virtual IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_IndexSet),
                Localized_Base_Entities.Ex_Boolean_IndexSet,
                values: GetErrorArgs());
        }

        /// <inheritdoc />
        public virtual IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_PropertyGet),
                Localized_Base_Entities.Ex_Boolean_PropertyGet,
                values: GetErrorArgs(property));
        }

        /// <inheritdoc />
        public virtual IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Boolean_PropertySet),
                Localized_Base_Entities.Ex_Boolean_PropertySet,
                values: GetErrorArgs(property));
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool TryConvert(Type type, out object value)
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
        public IScriptType ArithmeticUnaryPositive()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType ArithmeticUnaryNegative()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType ArithmeticAdd(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_AddInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_AddInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc />
        public IScriptType ArithmeticModulus(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType ArithmeticExponent(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType CompareEqual(IScriptType rhs)
        {
            if (rhs is BooleanBase b && b.Value == Value)
                return Processor.Factory.True;

            return Processor.Factory.False;
        }

        /// <inheritdoc />
        public IScriptType CompareNotEqual(IScriptType rhs)
        {
            if (rhs is BooleanBase b && b.Value == Value)
                return Processor.Factory.False;

            return Processor.Factory.True;
        }

        /// <inheritdoc />
        public IScriptType CompareGreaterThan(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType CompareLessThan(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType BinaryNot()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType BinaryAnd(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType BinaryOr(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType BinaryXor(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType BinaryLeftShift(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType BinaryRightShift(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType LogicalNot()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType LogicalAnd(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType LogicalOr(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType MemberIn(IScriptType lhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType MemberNotIn(IScriptType lhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType IdentityIs(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IScriptType IdentityIsNot(IScriptType rhs)
        {
            throw new NotImplementedException();
        }
    }
}