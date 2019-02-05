using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyProcessor : IProcessor
    {
        public PyProcessor()
        {
            Factory = new PyScriptTypeFactory(this);
        }

        public IScriptTypeFactory Factory { get; }

        public IScopeContext Globals { get; }

        public void ContinueYieldedValue(IScriptType value)
        {
            throw new System.NotImplementedException();
        }

        public void Walk()
        {
            throw new System.NotImplementedException();
        }
    }
}