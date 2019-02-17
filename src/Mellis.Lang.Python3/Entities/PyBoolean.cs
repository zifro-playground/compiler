using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Python3.Entities
{
    public class PyBoolean : BooleanBase
    {
        public PyBoolean(PyProcessor processor, bool value)
            : base(processor, value)
        {
        }

        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}