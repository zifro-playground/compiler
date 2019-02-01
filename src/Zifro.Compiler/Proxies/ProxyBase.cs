using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Proxies
{
    public abstract class ProxyBase : IScriptType
    {
        protected IScriptType InnerValue;

        protected ProxyBase(IScriptType innerValue)
        {
            InnerValue = innerValue;
        }

        public IProcessor Processor
        {
            set => InnerValue.Processor = value;
        }

        public IScriptType GetTypeDef() => InnerValue.GetTypeDef();
        public string GetTypeName() => InnerValue.GetTypeName();
        public IScriptType Invoke(IScriptType[] arguments) => InnerValue.Invoke(arguments);
        public IScriptType GetIndex(IScriptType index) => InnerValue.GetIndex(index);
        public IScriptType SetIndex(IScriptType index, IScriptType value) => InnerValue.SetIndex(index, value);
        public IScriptType GetProperty(string property) => InnerValue.GetProperty(property);
        public IScriptType SetProperty(string property, IScriptType value) => InnerValue.SetProperty(property, value);
        public bool TryConvert<T>(out T value) => InnerValue.TryConvert(out value);
        public IScriptType ArithmeticUnaryPositive() => InnerValue.ArithmeticUnaryPositive();
        public IScriptType ArithmeticUnaryNegative() => InnerValue.ArithmeticUnaryNegative();
        public IScriptType ArithmeticAdd(IScriptType rhs) => InnerValue.ArithmeticAdd(rhs);
        public IScriptType ArithmeticSubtract(IScriptType rhs) => InnerValue.ArithmeticSubtract(rhs);
        public IScriptType ArithmeticMultiply(IScriptType rhs) => InnerValue.ArithmeticMultiply(rhs);
        public IScriptType ArithmeticDivide(IScriptType rhs) => InnerValue.ArithmeticDivide(rhs);
        public IScriptType ArithmeticModulus(IScriptType rhs) => InnerValue.ArithmeticModulus(rhs);
        public IScriptType ArithmeticExponent(IScriptType rhs) => InnerValue.ArithmeticExponent(rhs);
        public IScriptType ArithmeticFloorDivide(IScriptType rhs) => InnerValue.ArithmeticFloorDivide(rhs);
        public IScriptType CompareEqual(IScriptType rhs) => InnerValue.CompareEqual(rhs);
        public IScriptType CompareNotEqual(IScriptType rhs) => InnerValue.CompareNotEqual(rhs);
        public IScriptType CompareGreaterThan(IScriptType rhs) => InnerValue.CompareGreaterThan(rhs);
        public IScriptType CompareGreaterThanOrEqual(IScriptType rhs) => InnerValue.CompareGreaterThanOrEqual(rhs);
        public IScriptType CompareLessThan(IScriptType rhs) => InnerValue.CompareLessThan(rhs);
        public IScriptType CompareLessThanOrEqual(IScriptType rhs) => InnerValue.CompareLessThanOrEqual(rhs);
        public IScriptType BinaryNot() => InnerValue.BinaryNot();
        public IScriptType BinaryAnd(IScriptType rhs) => InnerValue.BinaryAnd(rhs);
        public IScriptType BinaryOr(IScriptType rhs) => InnerValue.BinaryOr(rhs);
        public IScriptType BinaryXor(IScriptType rhs) => InnerValue.BinaryXor(rhs);
        public IScriptType BinaryLeftShift(IScriptType rhs) => InnerValue.BinaryLeftShift(rhs);
        public IScriptType BinaryRightShift(IScriptType rhs) => InnerValue.BinaryRightShift(rhs);
        public IScriptType LogicalNot() => InnerValue.LogicalNot();
        public IScriptType LogicalAnd(IScriptType rhs) => InnerValue.LogicalAnd(rhs);
        public IScriptType LogicalOr(IScriptType rhs) => InnerValue.LogicalOr(rhs);
        public IScriptType MemberIn(IScriptType lhs) => InnerValue.MemberIn(lhs);
        public IScriptType MemberNotIn(IScriptType lhs) => InnerValue.MemberNotIn(lhs);
        public IScriptType IdentityIs(IScriptType rhs) => InnerValue.IdentityIs(rhs);
        public IScriptType IdentityIsNot(IScriptType rhs) => InnerValue.IdentityIsNot(rhs);
    }
}
