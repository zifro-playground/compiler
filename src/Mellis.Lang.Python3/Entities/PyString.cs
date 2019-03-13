using System.Text;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities
{
    public class PyString : StringBase
    {
        public PyString(IProcessor processor, string value, string name = null)
            : base(processor, value, name)
        {
        }

        /// <inheritdoc />
        public override IScriptType Copy(string newName)
        {
            return new PyString(Processor, Value, newName);
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            return new PyType<PyString>(Processor, GetTypeName(), StringConstructor);
        }

        private IScriptType StringConstructor(IProcessor processor, IScriptType[] arguments)
        {
            if (arguments.Length > 1)
                throw new RuntimeTooManyArgumentsException(
                    GetTypeName(), 1, arguments.Length);

            if (arguments.Length == 0)
                return Processor.Factory.Create(string.Empty);

            return new PyString(Processor, arguments[0].ToString());
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger when rhsInteger.Value <= 0:
                    return Processor.Factory.Create(string.Empty);

                case IntegerBase rhsInteger:
                    var builder = new StringBuilder(Value.Length * rhsInteger.Value);
                    for (var i = 0; i < rhsInteger.Value; i++)
                    {
                        builder.Append(Value);
                    }

                    return Processor.Factory.Create(builder.ToString());

                default:
                    throw InvalidType(rhs, "*");
            }
        }

        public override string ToString()
        {
            return LiteralString.Escape(Value);
        }
    }
}