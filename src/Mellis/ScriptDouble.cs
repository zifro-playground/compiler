using System;
using System.Globalization;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    /// <summary>
    /// Basic functionality of a double value.
    /// </summary>
    public abstract class ScriptDouble : ScriptType, IScriptDouble
    {
        public double Value { get; }

        protected ScriptDouble(IProcessor processor, double value)
            : base(processor)
        {
            Value = value;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Double_Name;
        }

        public override bool IsTruthy()
        {
            return !Value.Equals(0d);
        }

        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptDouble d:
                return Processor.Factory.Create(Value + d.Value);

            case IScriptInteger i:
                return Processor.Factory.Create(Value + i.Value);

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
            case IScriptDouble d:
                return Processor.Factory.Create(Value - d.Value);

            case IScriptInteger i:
                return Processor.Factory.Create(Value - i.Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticSubtractReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case IScriptDouble d:
                return Processor.Factory.Create(d.Value - Value);

            case IScriptInteger i:
                return Processor.Factory.Create(i.Value - Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(Value * i.Value);

            case IScriptDouble d:
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
            case IScriptInteger i when i.Value.Equals(0):
            case IScriptDouble d when d.Value.Equals(0d):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case IScriptDouble d:
                return Processor.Factory.Create(Value / d.Value);

            case IScriptInteger i:
                return Processor.Factory.Create(Value / i.Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticDivideReverse(IScriptType lhs)
        {
            if (Value.Equals(0d))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case IScriptDouble d:
                return Processor.Factory.Create(d.Value / Value);

            case IScriptInteger i:
                return Processor.Factory.Create(i.Value / Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulus(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i when i.Value.Equals(0):
            case IScriptDouble d when d.Value.Equals(0d):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case IScriptInteger i:
                return Processor.Factory.Create(Value % i.Value);
            case IScriptDouble d:
                return Processor.Factory.Create(Value % d.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulusReverse(IScriptType lhs)
        {
            if (Value.Equals(0d))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(i.Value % Value);
            case IScriptDouble d:
                return Processor.Factory.Create(d.Value % Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponent(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(Math.Pow(Value, i.Value));
            case IScriptDouble d:
                return Processor.Factory.Create(Math.Pow(Value, d.Value));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponentReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(Math.Pow(i.Value, Value));
            case IScriptDouble d:
                return Processor.Factory.Create(Math.Pow(d.Value, Value));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i when i.Value.Equals(0):
            case IScriptDouble d when d.Value.Equals(0d):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case IScriptInteger i:
                return Processor.Factory.Create((int)Math.Floor(Value / i.Value));
            case IScriptDouble d:
                return Processor.Factory.Create((int)Math.Floor(Value / d.Value));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticFloorDivideReverse(IScriptType lhs)
        {
            if (Value.Equals(0d))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create((int)Math.Floor(i.Value / Value));
            case IScriptDouble d:
                return Processor.Factory.Create((int)Math.Floor(d.Value / Value));
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

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i:
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return Processor.Factory.Create(Value == i.Value);
            case IScriptDouble d:
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
            case IScriptInteger i:
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return Processor.Factory.Create(Value != i.Value);
            case IScriptDouble d:
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
            case IScriptInteger i:
                return Processor.Factory.Create(Value > i.Value);
            case IScriptDouble d:
                return Processor.Factory.Create(Value > d.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(Value >= i.Value);
            case IScriptDouble d:
                return Processor.Factory.Create(Value >= d.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(Value < i.Value);
            case IScriptDouble d:
                return Processor.Factory.Create(Value < d.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case IScriptInteger i:
                return Processor.Factory.Create(Value <= i.Value);
            case IScriptDouble d:
                return Processor.Factory.Create(Value <= d.Value);
            default:
                return null;
            }
        }

        public override string ToString()
        {
            switch (Value)
            {
            case double.PositiveInfinity:
                return Localized_Base_Entities.Type_Double_PosInfinity;
            case double.NegativeInfinity:
                return Localized_Base_Entities.Type_Double_NegInfinity;
            case double.NaN:
                return Localized_Base_Entities.Type_Double_NaN;

            default:
                return Value.ToString(CultureInfo.InvariantCulture).ToLower();
            }
        }
    }
}