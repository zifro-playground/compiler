using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public abstract class BaseEntityTester<T>
        where T : IScriptType
    {
        protected abstract T CreateEntity(PyProcessor processor);

        protected T CreateEntity()
        {
            return CreateEntity(new PyProcessor());
        }
    }
}