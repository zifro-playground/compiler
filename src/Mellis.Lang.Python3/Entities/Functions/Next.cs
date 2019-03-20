using Mellis.Core.Interfaces;

namespace Mellis.Lang.Python3.Entities.Functions
{
    public class Next : ClrFunction
    {
        public Next() : base("next")
        {
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}