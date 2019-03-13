using System.Globalization;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities
{
    public class PyInteger : IntegerBase
    {
        public PyInteger(IProcessor processor, int value, string name = null)
            : base(processor, value, name)
        {
        }

        /// <inheritdoc />
        public override IScriptType Copy(string newName)
        {
            return new PyInteger(Processor, Value, newName);
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            return new PyType<PyInteger>(Processor, GetTypeName(), IntegerConstructor);
        }

        private IScriptType IntegerConstructor(IProcessor processor, IScriptType[] arguments)
        {
            if (arguments.Length > 2)
                throw new RuntimeTooManyArgumentsException(
                    GetTypeName(), 2, arguments.Length);

            if (arguments.Length == 0)
                return new PyInteger(Processor, 0);

            throw new SyntaxNotYetImplementedException(SourceReference.ClrSource);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}