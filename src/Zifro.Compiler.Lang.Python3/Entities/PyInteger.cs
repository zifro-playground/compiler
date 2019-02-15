using System.Globalization;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Base.Entities;

namespace Zifro.Compiler.Lang.Python3.Entities
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