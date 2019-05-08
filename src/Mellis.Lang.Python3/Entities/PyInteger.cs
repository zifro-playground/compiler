using System.Globalization;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyInteger : ScriptInteger
    {
        public PyInteger(IProcessor processor, int value)
            : base(processor, value)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyIntegerType(Processor);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}