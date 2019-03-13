using System.Linq;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Exceptions;

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
            return new PyType<PyBoolean>(Processor, GetTypeName(), BooleanConstructor);
        }

        private IScriptType BooleanConstructor(IProcessor processor, IScriptType[] arguments)
        {
            if (arguments.Length == 0)
                return processor.Factory.True;

            if (arguments.Length > 1)
                throw new RuntimeTooManyArgumentsException(GetTypeName(), 1, arguments.Length);

            return arguments[0].IsTruthy()
                ? Processor.Factory.True
                : Processor.Factory.False;
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}