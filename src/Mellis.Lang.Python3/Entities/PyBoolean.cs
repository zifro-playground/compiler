using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyBoolean : ScriptBoolean
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
            return new PyBooleanType(Processor);
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}