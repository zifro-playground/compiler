using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Proxies
{
    public class ReturnValue : ValueProxyBase
    {
        public ReturnValue(IValueType innerValue) : base(innerValue)
        { }
    }
}