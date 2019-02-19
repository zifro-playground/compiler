using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mellis.Core.Interfaces;
using Mellis.Tools.Extensions;

namespace Mellis.Tools.AutoObject
{
    public abstract class AutoValueBase : IScriptType
    {
        public IProcessor Processor { get; set; }

        public abstract IScriptType GetTypeDef();

        public abstract string GetTypeName();

        public abstract IScriptType Invoke(IScriptType[] arguments);

        public virtual IScriptType GetIndex(IScriptType index)
        {
            throw new NotImplementedException("Can't let you do that john.");
        }

        public IScriptType SetIndex(IScriptType index, IScriptType value)
        {
            throw new NotImplementedException("Can't let you do that john.");
        }

        public IScriptType GetProperty(string property)
        {
            foreach (MemberInfo memberInfo in GetMembers())
            {
                var attribute = memberInfo.GetCustomAttribute<ShowInScriptAttribute>(true);
                if (attribute == null) continue;
                if ((attribute.Name ?? memberInfo.Name) != property) continue;

                switch (memberInfo)
                {
                    // found it
                    case PropertyInfo prop when Processor.Factory.TryCreate(prop.GetValue(this), out IScriptType value):
                        return value;
                    case FieldInfo field when Processor.Factory.TryCreate(field.GetValue(this), out IScriptType value):
                        return value;

                    case FieldInfo field:
                        throw new NotImplementedException(
                            $"Type mismatch. Cannot get script version of {field.FieldType.Name}");
                    case PropertyInfo prop:
                        throw new NotImplementedException(
                            $"Type mismatch. Cannot get script version of {prop.PropertyType.Name}");
                }
            }

            return Processor.Factory.Null;
        }

        public IScriptType SetProperty(string property, IScriptType value)
        {
            foreach (MemberInfo memberInfo in GetMembers())
            {
                var attribute = memberInfo.GetCustomAttribute<ShowInScriptAttribute>(true);
                if (attribute == null) continue;
                if ((attribute.Name ?? memberInfo.Name) != property) continue;

                switch (memberInfo)
                {
                    // found it
                    case PropertyInfo prop when value.TryConvert(prop.PropertyType, out object clrValue):
                        prop.SetValue(this, clrValue);
                        return value;
                    case FieldInfo field when value.TryConvert(field.FieldType, out object clrValue):
                        field.SetValue(this, clrValue);
                        return value;

                    case FieldInfo field:
                        throw new NotImplementedException($"Type mismatch. Cannot assign to {field.FieldType.Name}");
                    case PropertyInfo prop:
                        throw new NotImplementedException($"Type mismatch. Cannot assign to {prop.PropertyType.Name}");
                }
            }

            throw new NotImplementedException($"Property {property} not found");
        }

        private IEnumerable<MemberInfo> GetMembers()
        {
            return GetType().GetMembers(BindingFlags.Instance |
                                        BindingFlags.FlattenHierarchy |
                                        BindingFlags.GetField |
                                        BindingFlags.GetProperty |
                                        BindingFlags.Public |
                                        BindingFlags.NonPublic
            );
        }

        public bool TryConvert<T>(out T value)
        {
            if (this is T v)
            {
                value = v;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryConvert(Type type, out object value)
        {
            if (GetType().IsInstanceOfType(type))
            {
                value = this;
                return true;
            }

            value = default;
            return false;
        }

        public bool IsTruthy()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new NotImplementedException();
        }

        public IScriptType ArithmeticDivide(IScriptType rhs)
        {
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