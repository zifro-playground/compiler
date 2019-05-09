using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Resources;
using Mellis.Tools;

namespace Mellis.Lang.Python3.Entities
{
    public class PyBoolean : ScriptBoolean
    {
        private int Value01 => Value ? 1 : 0;

        public PyBoolean(IProcessor processor, bool value)
            : base(processor, value)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyBooleanType(Processor);
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }

        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 + i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 + d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 + b.Value01);
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
                return Processor.Factory.Create(Value01 - i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 - d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 - b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticSubtractReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value - Value01);
            case ScriptDouble d:
                return Processor.Factory.Create(d.Value - Value01);
            case PyBoolean b:
                return Processor.Factory.Create(b.Value01 - Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 * i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 * d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 * b.Value01);

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
            case PyBoolean b when b.Value01.Equals(0):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case ScriptInteger i:
                return Processor.Factory.Create(Value01 / (double)i.Value);

            case ScriptDouble d:
                return Processor.Factory.Create(Value01 / d.Value);

            case PyBoolean b:
                return Processor.Factory.Create(Value01 / b.Value01);

            default:
                return null;
            }
        }

        public override IScriptType ArithmeticDivideReverse(IScriptType lhs)
        {
            if (Value01.Equals(0))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value / (double)Value01);
            case ScriptDouble d:
                return Processor.Factory.Create(d.Value / Value01);
            case PyBoolean b:
                return Processor.Factory.Create(b.Value01 / Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulus(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 % i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 % d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 % b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticModulusReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value % Value01);
            case ScriptDouble d:
                return Processor.Factory.Create(d.Value % Value01);
            case PyBoolean b:
                return Processor.Factory.Create(b.Value01 % Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponent(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i when i.Value >= 0:
                // Try integer exponentiation
                return Processor.Factory.Create(MathUtilities.Pow(Value01, i.Value));
            case ScriptInteger i:
                return Processor.Factory.Create(Math.Pow(Value01, i.Value));
            case ScriptDouble d:
                return Processor.Factory.Create(Math.Pow(Value01, d.Value));
            case PyBoolean b:
                return Processor.Factory.Create(MathUtilities.Pow(Value01, b.Value01));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponentReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(MathUtilities.Pow(i.Value, Value01));
            case ScriptDouble d:
                return Processor.Factory.Create(Math.Pow(d.Value, Value01));
            case PyBoolean b:
                return Processor.Factory.Create(MathUtilities.Pow(b.Value01, Value01));
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
            case PyBoolean b when b.Value01.Equals(0):
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);

            case ScriptInteger i:
                return Processor.Factory.Create(Value01 / i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 / (int)d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 / b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticFloorDivideReverse(IScriptType lhs)
        {
            if (Value01.Equals(0))
            {
                throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Math_DivideByZero),
                    Localized_Base_Entities.Ex_Math_DivideByZero);
            }

            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value / Value01);
            case ScriptDouble d:
                return Processor.Factory.Create((int)d.Value / Value01);
            case PyBoolean b:
                return Processor.Factory.Create(b.Value01 / Value01);
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticUnaryPositive()
        {
            return Processor.Factory.Create(Value01);
        }

        public override IScriptType ArithmeticUnaryNegative()
        {
            return Processor.Factory.Create(-Value01);
        }

        public override IScriptType BinaryNot()
        {
            return Processor.Factory.Create(~Value01);
        }

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 == i.Value);
            case ScriptDouble d:
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return Processor.Factory.Create(Value01 == d.Value);
            case ScriptBoolean b:
                return Processor.Factory.Create(Value == b.Value);
            default:
                return Processor.Factory.False;
            }
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 != i.Value);
            case ScriptDouble d:
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                return Processor.Factory.Create(Value01 != d.Value);
            case ScriptBoolean b:
                return Processor.Factory.Create(Value != b.Value);
            default:
                return Processor.Factory.True;
            }
        }

        public override IScriptType CompareGreaterThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 > i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 > d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 > b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType CompareGreaterThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 >= i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 >= d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 >= b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThan(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 < i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 < d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 < b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType CompareLessThanOrEqual(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 <= i.Value);
            case ScriptDouble d:
                return Processor.Factory.Create(Value01 <= d.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 <= b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryAnd(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 & i.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 & b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryAndReverse(IScriptType lhs)
        {
            // Commutative
            return BinaryAnd(lhs);
        }

        public override IScriptType BinaryOr(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 | i.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 | b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryOrReverse(IScriptType lhs)
        {
            // Commutative
            return BinaryOr(lhs);
        }

        public override IScriptType BinaryXor(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 ^ i.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 ^ b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryXorReverse(IScriptType lhs)
        {
            // Commutative
            return BinaryXor(lhs);
        }

        public override IScriptType BinaryLeftShift(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 << i.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 << b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryLeftShiftReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value << Value01);
            case PyBoolean b:
                return Processor.Factory.Create(b.Value01 << Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryRightShift(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(Value01 >> i.Value);
            case PyBoolean b:
                return Processor.Factory.Create(Value01 >> b.Value01);
            default:
                return null;
            }
        }

        public override IScriptType BinaryRightShiftReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i:
                return Processor.Factory.Create(i.Value >> Value01);
            case PyBoolean b:
                return Processor.Factory.Create(b.Value01 >> Value01);
            default:
                return null;
            }
        }
    }
}