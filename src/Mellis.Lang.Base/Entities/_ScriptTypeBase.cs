using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptTypeBase : IScriptType
    {
        /// <inheritdoc/>
        public IProcessor Processor { get; internal set; }

        /// <inheritdoc/>
        public string Name { get; }

        protected ScriptTypeBase(IProcessor processor, string name = null)
        {
            Processor = processor;
            Name = name;
        }

        /// <inheritdoc/>
        public abstract IScriptType Copy(string newName);

        /// <inheritdoc/>
        public abstract IScriptType GetTypeDef();

        /// <inheritdoc/>
        public abstract string GetTypeName();

        protected RuntimeException InvalidType(IScriptType rhs, string keyword)
        {
            return new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Base_OperatorInvalidType),
                Localized_Base_Entities.Ex_Base_OperatorInvalidType,
                GetTypeName(), rhs.GetTypeName(), keyword);
        }

        /// <inheritdoc/>
        public virtual bool IsTruthy()
        {
            return true;
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
        public bool TryCoerce<T>(out T value)
        {
            if (TryCoerce(typeof(T), out object boxed))
            {
                value = (T) boxed;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public abstract bool TryCoerce(Type type, out object value);

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticUnaryPositive()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticUnaryNegative()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticAdd(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticAddReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticSubtractReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticMultiplyReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticDivide(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticDivideReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticModulus(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticModulusReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticExponent(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticExponentReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticFloorDivideReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareEqual(IScriptType rhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareNotEqual(IScriptType rhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareGreaterThan(IScriptType rhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareLessThan(IScriptType rhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryNot()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryAnd(IScriptType rhs)
        {
            return null;
        }

        public IScriptType BinaryAndReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryOr(IScriptType rhs)
        {
            return null;
        }

        public IScriptType BinaryOrReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryXor(IScriptType rhs)
        {
            return null;
        }

        public IScriptType BinaryXorReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryLeftShift(IScriptType rhs)
        {
            return null;
        }

        public IScriptType BinaryLeftShiftReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType BinaryRightShift(IScriptType rhs)
        {
            return null;
        }

        public IScriptType BinaryRightShiftReverse(IScriptType lhs)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IScriptType MemberIn(IScriptType lhs)
        {
            return null;
        }
    }
}