using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities
{
    public class PyString : StringBase
    {
        public PyString(PyProcessor processor, string value)
            : base(processor, value)
        {
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return LiteralString.Escape(Value);
        }
    }
}