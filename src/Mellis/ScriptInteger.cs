using System;
using System.Globalization;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    /// <summary>
    /// Basic functionality of an integer value.
    /// </summary>
    public abstract class ScriptInteger : ScriptType
    {
        public int Value { get; }

        protected ScriptInteger(IProcessor processor, int value)
            : base(processor)
        {
            Value = value;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Int_Name;
        }

        public override bool IsTruthy()
        {
            return !Value.Equals(0);
        }

        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value + i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value + d.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticAddReverse(IScriptType lhs)
        {
            // Commutative
            return ArithmeticAdd(lhs);
        }

        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value - i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value - d.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticSubtractReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value - Value);
            case ScriptDouble d:
                return Processor.Factory.Create(d.Value - Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value * i.Value);

            case ScriptDouble d:
                return Processor.Factory.Create(Value * d.Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticMultiplyReverse(IScriptType lhs)
        {
            // Commutative
            return ArithmeticMultiply(lhs);
        }

        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i when i.Value.Equals(0):
            case ScriptDouble d when d.Value.Equals(0):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case ScriptInteger i:
                return Processor.Factory.Create(Value / (double)i.Value);

            case ScriptDouble d:
                return Processor.Factory.Create(Value / d.Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticDivideReverse(IScriptType lhs)
        {
            if (Value.Equals(0))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value / (double)Value);

            case ScriptDouble d:
                return Processor.Factory.Create(d.Value / Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulus(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value % i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value % d.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulusReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value % Value);
            case ScriptDouble d:
                return Processor.Factory.Create(d.Value % Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponent(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Math.Pow(Value, i.Value));
            case ScriptDouble d:
                return Processor.Factory.Create(Math.Pow(Value, d.Value));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponentReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Math.Pow(i.Value, Value));
            case ScriptDouble d:
                return Processor.Factory.Create(Math.Pow(d.Value, Value));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i when i.Value.Equals(0):
            case ScriptDouble d when d.Value.Equals(0d):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case ScriptInteger i:
                return Processor.Factory.Create(Value / i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value / (int)d.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticFloorDivideReverse(IScriptType lhs)
        {
            if (Value.Equals(0))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value / Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value / (int)d.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticUnaryPositive()
        {
            return this;
        }

        public override IScriptType ArithmeticUnaryNegative()
        {
            return Processor.Factory.Create(-Value);
        }

        public override IScriptType BinaryNot()
        {
            return Processor.Factory.Create(~Value);
        }

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value == i.Value);
            case ScriptDouble d:
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return Processor.Factory.Create(Value == d.Value);
            default:
                return Processor.Factory.False;
            }
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value != i.Value);
            case ScriptDouble d:
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return Processor.Factory.Create(Value != d.Value);
            default:
                return Processor.Factory.True;
            }
        }

        public override IScriptType CompareGreaterThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value > i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value > d.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value >= i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value >= d.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value < i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value < d.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value <= i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value <= d.Value);
            default:
                return null;
            }
        }

        public override IScriptType BinaryAnd(IScriptType rhs)
        {
            if (rhs is ScriptInteger i)
            {
                return Processor.Factory.Create(Value & i.Value);
            }

            return null;
        }

        public override IScriptType BinaryAndReverse(IScriptType lhs)
        {
            // Commutative
            return BinaryAnd(lhs);
        }

        public override IScriptType BinaryOr(IScriptType rhs)
        {
            if (rhs is ScriptInteger i)
            {
                return Processor.Factory.Create(Value | i.Value);
            }

            return null;
        }

        public override IScriptType BinaryOrReverse(IScriptType lhs)
        {
            // Commutative
            return BinaryOr(lhs);
        }

        public override IScriptType BinaryXor(IScriptType rhs)
        {
            if (rhs is ScriptInteger i)
            {
                return Processor.Factory.Create(Value ^ i.Value);
            }

            return null;
        }

        public override IScriptType BinaryXorReverse(IScriptType lhs)
        {
            // Commutative
            return BinaryXor(lhs);
        }

        public override IScriptType BinaryLeftShift(IScriptType rhs)
        {
            if (rhs is ScriptInteger i)
            {
                return Processor.Factory.Create(Value << i.Value);
            }

            return null;
        }

        public override IScriptType BinaryLeftShiftReverse(IScriptType lhs)
        {
            if (lhs is ScriptInteger i)
            {
                return Processor.Factory.Create(i.Value << Value);
            }

            return null;
        }

        public override IScriptType BinaryRightShift(IScriptType rhs)
        {
            if (rhs is ScriptInteger i)
            {
                return Processor.Factory.Create(Value >> i.Value);
            }

            return null;
        }

        public override IScriptType BinaryRightShiftReverse(IScriptType lhs)
        {
            if (lhs is ScriptInteger i)
            {
                return Processor.Factory.Create(i.Value >> Value);
            }

            return null;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.CurrentCulture);
        }
    }
}