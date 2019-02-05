using System;
using System.Linq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Resources;

namespace Zifro.Compiler.Lang.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic functionality of a string value.
    /// </summary>
    public abstract class StringBase : IScriptType
    {
        /// <inheritdoc/>
        public abstract IScriptType GetTypeDef();

        /// <inheritdoc/>
        public IProcessor Processor { get; }

        public string Value { get; set; }

        protected StringBase(IProcessor processor)
        {
            Processor = processor;
        }

        /// <inheritdoc/>
        public virtual string GetTypeName()
        {
            return Localized_Base_Entities.Type_String_Name;
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
        public virtual IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_Invoke),
                Localized_Base_Entities.Ex_String_Invoke,
                values: GetErrorArgs());
        }

        /// <inheritdoc/>
        public virtual IScriptType GetIndex(IScriptType index)
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
                        values: GetErrorArgs(indexInteger.Value));

                default:
                    throw new RuntimeException(
                        nameof(Localized_Base_Entities.Ex_String_IndexGet_InvalidType),
                        Localized_Base_Entities.Ex_String_IndexGet_InvalidType,
                        values: GetErrorArgs(index.GetTypeName()));
            }
        }

        /// <inheritdoc/>
        public virtual IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_IndexSet),
                Localized_Base_Entities.Ex_String_IndexSet,
                values: GetErrorArgs());
        }

        /// <inheritdoc/>
        public virtual IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_PropertyGet),
                Localized_Base_Entities.Ex_String_PropertyGet,
                values: GetErrorArgs(property));
        }

        /// <inheritdoc/>
        public virtual IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_String_PropertySet),
                Localized_Base_Entities.Ex_String_PropertySet,
                values: GetErrorArgs(property));
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
        public virtual bool TryConvert(Type type, out object value)
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
                case StringBase rhsString:
                    return Processor.Factory.Create(Value + rhsString.Value);
                default:
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_AddInvalidType),
                        Localized_Base_Entities.Ex_String_AddInvalidType,
                        values: GetErrorArgs(rhs.GetTypeName()));
            }
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_String_SubtractInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_String_MultiplyInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
        }

        /// <inheritdoc/>
        public IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_String_DivideInvalidOperation),
                Localized_Base_Entities.Ex_String_DivideInvalidOperation,
                values: GetErrorArgs(rhs.GetTypeName()));
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
        public IScriptType LogicalNot()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType LogicalAnd(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IScriptType LogicalOr(IScriptType rhs)
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