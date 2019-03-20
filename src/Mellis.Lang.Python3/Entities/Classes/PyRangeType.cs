using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyRangeType : PyType<PyRange>
    {
        public PyRangeType(IProcessor processor, string name = null)
            : base(processor, Localized_Python3_Entities.Type_Range_Name, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            throw new System.NotImplementedException();
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}