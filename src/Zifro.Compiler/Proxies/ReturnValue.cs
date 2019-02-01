using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Proxies
{
    public class ReturnValue : ValueProxyBase
    {
        /// <summary>
        /// Gets the function from which this value was retrieved.
        /// </summary>
        public IFunctionType Function { get; }

        public ReturnValue(IValueType innerValue, IFunctionType function)
            : base(innerValue)
        {
            Function = function;
        }
    }
}