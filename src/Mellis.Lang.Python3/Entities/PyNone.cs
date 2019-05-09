using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Resources;

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
            return new PyType<PyNone>(Processor, Localized_Base_Entities.Type_Null_Name);
        }

        public override string ToString()
        {
            return "None";
        }
    }
}