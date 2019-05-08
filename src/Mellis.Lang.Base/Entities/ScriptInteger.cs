using System;
using System.Globalization;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Tools.Extensions;

namespace Mellis.Lang.Base.Entities
{
    /// <summary>
    /// Basic functionality of an integer value.
    /// </summary>
    public abstract class ScriptInteger : ScriptBaseType
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
            case ScriptInteger rhsInt:
                return Processor.Factory.Create(Value + rhsInt.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInt:
                return Processor.Factory.Create(Value - rhsInt.Value);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInt:
                return Processor.Factory.Create(Value * rhsInt.Value);

            case ScriptDouble rhsDouble:
                return Processor.Factory.CreateAppropriate(Value * rhsDouble.Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInt when rhsInt.Value.Equals(0):
            case ScriptDouble rhsDouble when rhsDouble.Value.Equals(0):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case ScriptInteger rhsInt:
                return Processor.Factory.CreateAppropriate(Value / (double)rhsInt.Value);

            case ScriptDouble rhsDouble:
                return Processor.Factory.CreateAppropriate(Value / rhsDouble.Value);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulus(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value % rhsInteger.Value);
            case ScriptDouble rhsDouble:
                return rhsDouble.ArithmeticModulus(this);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponent(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Math.Pow(Value, rhsInteger.Value));
            case ScriptDouble rhsDouble:
                return rhsDouble.ArithmeticExponent(this);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticFloorDivide(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value / rhsInteger.Value);
            case ScriptDouble rhsDouble:
                return rhsDouble.ArithmeticFloorDivide(this);
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
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value.Equals(rhsInteger.Value));
            case ScriptDouble rhsDouble:
                return rhsDouble.CompareEqual(this);
            default:
                return Processor.Factory.False;
            }
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(!Value.Equals(rhsInteger.Value));
            case ScriptDouble rhsDouble:
                return rhsDouble.CompareNotEqual(this);
            default:
                return Processor.Factory.True;
            }
        }

        public override IScriptType CompareGreaterThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value > rhsInteger.Value);
            case ScriptDouble rhsDouble:
                return rhsDouble.CompareGreaterThan(this);
            default:
                return null;
            }
        }

        public override IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value >= rhsInteger.Value);
            case ScriptDouble rhsDouble:
                return rhsDouble.CompareGreaterThanOrEqual(this);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value < rhsInteger.Value);
            case ScriptDouble rhsDouble:
                return rhsDouble.CompareLessThan(this);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger rhsInteger:
                return Processor.Factory.Create(Value <= rhsInteger.Value);
            case ScriptDouble rhsDouble:
                return rhsDouble.CompareLessThanOrEqual(this);
            default:
                return null;
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.CurrentCulture);
        }
    }
}