using System;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Resources;

namespace Zifro.Compiler.Entities
{
    public abstract class DoubleBase : IScriptType
    {
        public abstract IProcessor Processor { get; set; }

        public abstract IScriptType GetTypeDef();
        public abstract string GetTypeName();

        public double Value { get; set; }

        public virtual IScriptType Invoke(IScriptType[] arguments)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_Invoke),
                Localized_Base_Entities.Ex_Double_Invoke,
                Value);
        }

        public virtual IScriptType GetIndex(IScriptType index)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_IndexGet),
                Localized_Base_Entities.Ex_Double_IndexGet,
                Value);
        }

        public virtual IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_IndexSet),
                Localized_Base_Entities.Ex_Double_IndexSet,
                Value);
        }

        public virtual IScriptType GetProperty(string property)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_PropertyGet),
                Localized_Base_Entities.Ex_Double_PropertyGet,
                Value,
                property);
        }

        public virtual IScriptType SetProperty(string property, IScriptType value)
        {
            throw new RuntimeException(
                nameof(Localized_Base_Entities.Ex_Double_PropertySet),
                Localized_Base_Entities.Ex_Double_PropertySet,
                Value,
                property);
        }

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

        public bool TryConvert(Type type, out object value)
        {
            if (type == typeof(int))
            {
                value = Value;
                return true;
            }

            if (type == typeof(long))
            {
                value = (long)Value;
                return true;
            }

            if (type == typeof(double))
            {
                value = (double)Value;
                return true;
            }

            if (type == typeof(float))
            {
                value = (float)Value;
                return true;
            }

            if (type == typeof(decimal))
            {
                value = (decimal)Value;
                return true;
            }

            value = default;
            return false;
        }

        public IScriptType ArithmeticUnaryPositive()
        {
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticUnaryNegative()
        {
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticAdd(IScriptType rhs)
        {
            if (rhs is DoubleBase rhsDouble)
            {
                return Processor.Factory.Create(Value + rhsDouble.Value);
            }
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            if (rhs is DoubleBase rhsDouble)
            {
                return Processor.Factory.Create(Value - rhsDouble.Value);
            }
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            if (rhs is DoubleBase rhsDouble)
            {
                return Processor.Factory.Create(Value * rhsDouble.Value);
            }
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticDivide(IScriptType rhs)
        {
            if (rhs is DoubleBase rhsDouble)
            {
                if (rhsDouble.Value.Equals(0d))
                {
                    throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                        Localized_Base_Entities.Ex_Math_DivideByZero);
                }

                return Processor.Factory.Create(Value / rhsDouble.Value);
            }
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticModulus(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticExponent(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType CompareEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType CompareNotEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType CompareGreaterThan(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType CompareLessThan(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType BinaryNot()
        {
            throw new NotImplementedException();
        }

        public IScriptType BinaryAnd(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType BinaryOr(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType BinaryXor(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType BinaryLeftShift(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType BinaryRightShift(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType LogicalNot()
        {
            throw new NotImplementedException();
        }

        public IScriptType LogicalAnd(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType LogicalOr(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType MemberIn(IScriptType lhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType MemberNotIn(IScriptType lhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType IdentityIs(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType IdentityIsNot(IScriptType rhs)
        {
            throw new NotImplementedException();
        }
    }
}