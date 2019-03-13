using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Python3.Entities
{
    public class PyNone : NullBase
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
            return new PyType<PyNone>(Processor, GetTypeName());
        }

        public override string ToString()
        {
            return "None";
        }
    }
}