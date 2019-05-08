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

        public override bool TryCoerce(Type type, out object value)
        {
            switch (Type.GetTypeCode(type))
            {
            case TypeCode.Boolean:
                value = Value != 0;
                return true;

            case TypeCode.Byte:
                value = (byte)Value;
                return true;

            case TypeCode.Int16:
                value = (short)Value;
                return true;

            case TypeCode.Int32:
                value = Value;
                return true;

            case TypeCode.Int64:
                value = (long)Value;
                return true;

            case TypeCode.SByte:
                value = (sbyte)Value;
                return true;

            case TypeCode.UInt16:
                value = (ushort)Value;
                return true;

            case TypeCode.UInt32:
                value = (uint)Value;
                return true;

            case TypeCode.UInt64:
                value = (ulong)Value;
                return true;

            case TypeCode.Single:
                value = (float)Value;
                return true;

            case TypeCode.Double:
                value = (double)Value;
                return true;

            case TypeCode.Decimal:
                value = (decimal)Value;
                return true;

            case TypeCode.Char:
                value = Value.Equals(0) ? '\x0' : '\x1';
                return true;

            case TypeCode.Object when typeof(ScriptInteger).IsAssignableFrom(type):
                value = this;
                return true;

            case TypeCode.String:
                value = ToString();
                return true;

            default:
                value = default;
                return false;
            }
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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