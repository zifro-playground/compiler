using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Proxies
{
    public class ReturnValue : ProxyBase
    {
        /// <summary>
        /// Gets the function from which this value was retrieved.
        /// </summary>
        public IFunction Function { get; }

        public ReturnValue(IScriptType innerValue, IFunction function)
            : base(innerValue)
        {
            Function = function;
        }
    }
}