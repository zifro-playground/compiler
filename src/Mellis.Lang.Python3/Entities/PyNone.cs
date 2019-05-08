using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyNone : ScriptNull
    {
        public PyNone(IProcessor processor, string name = null)
            : base(processor, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyNone(Processor, newName);
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