using System.Globalization;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Python3.Entities
{
    public class PyInteger : IntegerBase
    {
        public PyInteger(PyProcessor processor, int value)
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
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}