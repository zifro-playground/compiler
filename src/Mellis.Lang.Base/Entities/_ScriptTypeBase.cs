using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptTypeBase : IScriptType
    {
        /// <inheritdoc/>
        public IProcessor Processor { get; }

        /// <inheritdoc/>
        public abstract IScriptType GetTypeDef();

        protected ScriptTypeBase(IProcessor processor)
        {
            Processor = processor;
        }

        /// <inheritdoc/>
        public abstract string GetTypeName();

        protected RuntimeException NotImplemented(string keyword)
        {
            return new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_Operator),
                Localized_Base_Entities.Ex_Base_Operator,
                GetTypeName(), keyword);
        }

        protected RuntimeException InvalidType(IScriptType rhs, string keyword)
        {
            return new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_OperatorInvalidType),
                Localized_Base_Entities.Ex_Base_OperatorInvalidType,
                GetTypeName(), rhs.GetTypeName(), keyword);
        }

        /// <inheritdoc/>
        public abstract bool IsTruthy();

        /// <inheritdoc/>
        public virtual IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_Invoke),
                Localized_Base_Entities.Ex_Base_Invoke,
                GetTypeName());
        }

        /// <inheritdoc/>
        public virtual IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_IndexGet),
                Localized_Base_Entities.Ex_Base_IndexGet,
                GetTypeName());
        }

        /// <inheritdoc/>
        public virtual IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_IndexSet),
                Localized_Base_Entities.Ex_Base_IndexSet,
                GetTypeName());
        }

        /// <inheritdoc/>
        public virtual IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_PropertyGet),
                Localized_Base_Entities.Ex_Base_PropertyGet,
                GetTypeName(), property);
        }

        /// <inheritdoc/>
        public virtual IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_PropertySet),
                Localized_Base_Entities.Ex_Base_PropertySet,
                GetTypeName(), property);
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
        public abstract bool TryConvert(Type type, out object value);

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticUnaryPositive()
        {
            throw NotImplemented("+");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticUnaryNegative()
        {
            throw NotImplemented("-");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticAdd(IScriptType rhs)
        {
            throw NotImplemented("+");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw NotImplemented("-");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw NotImplemented("*");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw NotImplemented("/");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticModulus(IScriptType rhs)
        {
            throw NotImplemented("%");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticExponent(IScriptType rhs)
        {
            throw NotImplemented("**");
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            throw NotImplemented("//");
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareEqual(IScriptType rhs)
        {
            throw NotImplemented("==");
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareNotEqual(IScriptType rhs)
        {
            throw NotImplemented("!=");
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareGreaterThan(IScriptType rhs)
        {
            throw NotImplemented(">");
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            throw NotImplemented(">=");
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareLessThan(IScriptType rhs)
        {
            throw NotImplemented("<");
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            throw NotImplemented("<=");
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryNot()
        {
            throw NotImplemented("~");
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryAnd(IScriptType rhs)
        {
            throw NotImplemented("&");
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryOr(IScriptType rhs)
        {
            throw NotImplemented("|");
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryXor(IScriptType rhs)
        {
            throw NotImplemented("^");
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryLeftShift(IScriptType rhs)
        {
            throw NotImplemented("<<");
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryRightShift(IScriptType rhs)
        {
            throw NotImplemented(">>");
        }

        /// <inheritdoc/>
        public virtual IScriptType MemberIn(IScriptType lhs)
        {
            throw NotImplemented("in");
        }

        /// <inheritdoc/>
        public virtual IScriptType MemberNotIn(IScriptType lhs)
        {
            throw NotImplemented("not in");
        }

        /// <inheritdoc/>
        public virtual IScriptType IdentityIs(IScriptType rhs)
        {
            throw NotImplemented("is");
        }

        /// <inheritdoc/>
        public virtual IScriptType IdentityIsNot(IScriptType rhs)
        {
            throw NotImplemented("is not");
        }
    }
}