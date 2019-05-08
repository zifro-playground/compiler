using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptBaseType : IScriptType
    {
        public IProcessor Processor { get; internal set; }

        protected ScriptBaseType(IProcessor processor)
        {
            Processor = processor;
        }

        public abstract IScriptType GetTypeDef();

        public abstract string GetTypeName();

        public virtual bool IsTruthy()
        {
            return true;
        }

        public virtual IScriptType GetIndex(IScriptType index)
        {
            return null;
        }

        public virtual IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            return null;
        }

        public virtual IScriptType GetProperty(string property)
        {
            return null;
        }

        public virtual IScriptType SetProperty(string property, IScriptType value)
        {
            return null;
        }

        public bool TryCoerce<T>(out T value)
        {
            if (TryCoerce(typeof(T), out object boxed))
            {
                value = (T)boxed;
                return true;
            }

            value = default;
            return false;
        }

        public abstract bool TryCoerce(Type type, out object value);

        public virtual IScriptType ArithmeticUnaryPositive()
        {
            return null;
        }

        public virtual IScriptType ArithmeticUnaryNegative()
        {
            return null;
        }

        public virtual IScriptType ArithmeticAdd(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticAddReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticSubtractReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticMultiplyReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticDivide(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticDivideReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticModulus(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticModulusReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticExponent(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticExponentReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType ArithmeticFloorDivideReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType CompareEqual(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType CompareNotEqual(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType CompareGreaterThan(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType CompareLessThan(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType BinaryNot()
        {
            return null;
        }

        public virtual IScriptType BinaryAnd(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType BinaryAndReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType BinaryOr(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType BinaryOrReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType BinaryXor(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType BinaryXorReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType BinaryLeftShift(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType BinaryLeftShiftReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType BinaryRightShift(IScriptType rhs)
        {
            return null;
        }

        public virtual IScriptType BinaryRightShiftReverse(IScriptType lhs)
        {
            return null;
        }

        public virtual IScriptType MemberIn(IScriptType lhs)
        {
            return null;
        }
    }
}