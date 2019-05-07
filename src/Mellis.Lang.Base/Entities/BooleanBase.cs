using System;
using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    /// <summary>
    /// Basic functionality of a double value.
    /// </summary>
    public abstract class BooleanBase : ScriptTypeBase
    {
        public bool Value { get; }

        protected BooleanBase(IProcessor processor, bool value, string name = null)
            : base(processor, name)
        {
            Value = value;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Boolean_Name;
        }

        protected object[] GetErrorArgs(params object[] additional)
        {
            return GetErrorArgs().Concat(additional).ToArray();
        }

        protected object[] GetErrorArgs()
        {
            return new object[] {
                Value,
                GetLocalizedString()
            };
        }

        public string GetLocalizedString()
        {
            return Value
                ? Localized_Base_Entities.Type_Boolean_True
                : Localized_Base_Entities.Type_Boolean_False;
        }

        public override bool IsTruthy()
        {
            return Value;
        }

        public override bool TryCoerce(Type type, out object value)
        {
            switch (Type.GetTypeCode(type))
            {
            case TypeCode.Boolean:
                value = Value;
                return true;

            case TypeCode.Byte:
                value = (byte)(Value ? 1 : 0);
                return true;

            case TypeCode.Int16:
                value = (short)(Value ? 1 : 0);
                return true;

            case TypeCode.Int32:
                value = Value ? 1 : 0;
                return true;

            case TypeCode.Int64:
                value = Value ? 1L : 0L;
                return true;

            case TypeCode.SByte:
                value = (sbyte)(Value ? 1 : 0);
                return true;

            case TypeCode.UInt16:
                value = (ushort)(Value ? 1 : 0);
                return true;

            case TypeCode.UInt32:
                value = Value ? 1u : 0u;
                return true;

            case TypeCode.UInt64:
                value = Value ? 1Lu : 0Lu;
                return true;

            case TypeCode.Single:
                value = Value ? 1f : 0f;
                return true;

            case TypeCode.Double:
                value = Value ? 1d : 0d;
                return true;

            case TypeCode.Decimal:
                value = Value ? 1m : 0m;
                return true;

            case TypeCode.Char:
                value = Value ? '\x0' : '\x1';
                return true;

            case TypeCode.Object when typeof(BooleanBase).IsAssignableFrom(type):
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
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_AddInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_AddInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticSubtract(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_SubtractInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_MultiplyInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType ArithmeticDivide(IScriptType rhs)
        {
            throw new RuntimeException(nameof(Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation),
                Localized_Base_Entities.Ex_Boolean_DivideInvalidOperation,
                formatArgs: GetErrorArgs(rhs.GetTypeName()));
        }

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            if (rhs is BooleanBase b && b.Value == Value)
            {
                return Processor.Factory.True;
            }

            return Processor.Factory.False;
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            if (rhs is BooleanBase b && b.Value == Value)
            {
                return Processor.Factory.False;
            }

            return Processor.Factory.True;
        }

        public override string ToString()
        {
            return Value ? bool.TrueString : bool.FalseString;
        }
    }
}