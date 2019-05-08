using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyNone : ScriptNull
    {
        public PyNone(IProcessor processor)
            : base(processor)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyNoneType(Processor);
        }

        public override string ToString()
        {
            return "None";
        }
    }
}