using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Proxies
{
    public abstract class ValueProxyBase : IValueType
    {
        protected IValueType _innerValue;

        protected ValueProxyBase(IValueType innerValue)
        {
            _innerValue = innerValue;
        }

        public IProcessor Processor
        {
            set => _innerValue.Processor = value;
        }

        public IValueType GetTypeDef() => _innerValue.GetTypeDef();

        public IValueType Invoke(IValueType[] arguments) => _innerValue.Invoke(arguments);

        public IValueType GetIndex(IValueType index) => _innerValue.GetIndex(index);

        public IValueType SetIndex(IValueType index, IValueType value) => _innerValue.SetIndex(index, value);

        public IValueType GetProperty(string property) => _innerValue.GetProperty(property);

        public IValueType SetProperty(string property, IValueType value) => _innerValue.SetProperty(property, value);

        public bool TryConvert<T>(out T value) => _innerValue.TryConvert(out value);

        public IValueType ArithmeticUnaryPositive() => _innerValue.ArithmeticUnaryPositive();

        public IValueType ArithmeticUnaryNegative() => _innerValue.ArithmeticUnaryNegative();

        public IValueType ArithmeticAdd(IValueType rhs) => _innerValue.ArithmeticAdd(rhs);

        public IValueType ArithmeticSubtract(IValueType rhs) => _innerValue.ArithmeticSubtract(rhs);

        public IValueType ArithmeticMultiply(IValueType rhs) => _innerValue.ArithmeticMultiply(rhs);

        public IValueType ArithmeticDivide(IValueType rhs) => _innerValue.ArithmeticDivide(rhs);

        public IValueType ArithmeticModulus(IValueType rhs) => _innerValue.ArithmeticModulus(rhs);

        public IValueType ArithmeticExponent(IValueType rhs) => _innerValue.ArithmeticExponent(rhs);

        public IValueType ArithmeticFloorDivide(IValueType rhs) => _innerValue.ArithmeticFloorDivide(rhs);

        public IValueType CompareEqual(IValueType rhs) => _innerValue.CompareEqual(rhs);

        public IValueType CompareNotEqual(IValueType rhs) => _innerValue.CompareNotEqual(rhs);

        public IValueType CompareGreaterThan(IValueType rhs) => _innerValue.CompareGreaterThan(rhs);

        public IValueType CompareGreaterThanOrEqual(IValueType rhs) => _innerValue.CompareGreaterThanOrEqual(rhs);

        public IValueType CompareLessThan(IValueType rhs) => _innerValue.CompareLessThan(rhs);

        public IValueType CompareLessThanOrEqual(IValueType rhs) => _innerValue.CompareLessThanOrEqual(rhs);

        public IValueType BinaryNot() => _innerValue.BinaryNot();

        public IValueType BinaryAnd(IValueType rhs) => _innerValue.BinaryAnd(rhs);

        public IValueType BinaryOr(IValueType rhs) => _innerValue.BinaryOr(rhs);

        public IValueType BinaryXor(IValueType rhs) => _innerValue.BinaryXor(rhs);

        public IValueType BinaryLeftShift(IValueType rhs) => _innerValue.BinaryLeftShift(rhs);

        public IValueType BinaryRightShift(IValueType rhs) => _innerValue.BinaryRightShift(rhs);

        public IValueType LogicalNot() => _innerValue.LogicalNot();

        public IValueType LogicalAnd(IValueType rhs) => _innerValue.LogicalAnd(rhs);

        public IValueType LogicalOr(IValueType rhs) => _innerValue.LogicalOr(rhs);

        public IValueType MemberIn(IValueType lhs) => _innerValue.MemberIn(lhs);

        public IValueType MemberNotIn(IValueType lhs) => _innerValue.MemberNotIn(lhs);

        public IValueType IdentityIs(IValueType rhs) => _innerValue.IdentityIs(rhs);

        public IValueType IdentityIsNot(IValueType rhs) => _innerValue.IdentityIsNot(rhs);
    }
}
