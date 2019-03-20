using Mellis.Core.Interfaces;

namespace Mellis.Lang.Python3.Entities.Functions
{
    public class Iter : ClrFunction
    {
        public Iter() : base("iter")
        {
        }

        public override IScriptType Invoke(params IScriptType[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}