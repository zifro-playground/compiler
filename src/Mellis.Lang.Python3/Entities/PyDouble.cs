using System.Globalization;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Exceptions;

namespace Mellis.Lang.Python3.Entities
{
    public class PyDouble : DoubleBase
    {
        public PyDouble(IProcessor processor, double value, string name = null)
            : base(processor, value, name)
        {
        }

        /// <inheritdoc />
        public override IScriptType Copy(string newName)
        {
            return new PyDouble(Processor, Value, newName);
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            return new PyType<PyDouble>(Processor, GetTypeName(), DoubleConstructor);
        }

        private IScriptType DoubleConstructor(IProcessor processor, IScriptType[] arguments)
        {
            if (arguments.Length > 1)
                throw new RuntimeTooManyArgumentsException(
                    GetTypeName(), 1, arguments.Length);

            if (arguments.Length == 0)
                return new PyDouble(Processor, 0);

            throw new SyntaxNotYetImplementedException(SourceReference.ClrSource);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}