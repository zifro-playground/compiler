using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    /// <summary>
    /// Basic functionality of a string value.
    /// </summary>
    public abstract class ScriptString : ScriptType
    {
        public string Value { get; }

        protected ScriptString(IProcessor processor, string value)
            : base(processor)
        {
            Value = value ?? string.Empty;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_String_Name;
        }

        public override bool IsTruthy()
        {
            return !string.IsNullOrEmpty(Value);
        }

        protected object[] GetErrorArgs(params object[] additional)
        {
            return GetErrorArgs().Concat(additional).ToArray();
        }

        protected object[] GetErrorArgs()
        {
            return new object[] {Value, Value.Length};
        }

        public override IScriptType GetIndex(IScriptType index)
        {
            switch (index)
            {
            case ScriptInteger indexInteger when indexInteger.Value >= 0 &&
                                                 indexInteger.Value < Value.Length:
                return Processor.Factory.Create(Value[indexInteger.Value]);

            case ScriptInteger indexInteger:
                throw new RuntimeException(
                    nameof(Localized_Base_Entities.Ex_String_IndexGet_OutOfRange),
                    Localized_Base_Entities.Ex_String_IndexGet_OutOfRange,
                    formatArgs: GetErrorArgs(indexInteger.Value));

            default:
                throw new RuntimeException(
                    nameof(Localized_Base_Entities.Ex_String_IndexGet_InvalidType),
                    Localized_Base_Entities.Ex_String_IndexGet_InvalidType,
                    formatArgs: GetErrorArgs(index.GetTypeName()));
            }
        }

        public override IScriptType ArithmeticAdd(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptString rhsString:
                return Processor.Factory.Create(Value + rhsString.Value);
            default:
                return null;
            }
        }

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            bool equals = rhs is ScriptString str && str.Value.Equals(Value);
            return Processor.Factory.Create(equals);
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            bool notEquals = !(rhs is ScriptString str) || !str.Value.Equals(Value);
            return Processor.Factory.Create(notEquals);
        }
    }
}