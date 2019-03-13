using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Python3.Entities
{
    public class PyBoolean : BooleanBase
    {
        public PyBoolean(IProcessor processor, bool value, string name = null)
            : base(processor, value, name)
        {
        }

        /// <inheritdoc />
        public override IScriptType Copy(string newName)
        {
            return new PyBoolean(Processor, Value, newName);
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            return new PyType<PyBoolean>(Processor, GetTypeName());
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}