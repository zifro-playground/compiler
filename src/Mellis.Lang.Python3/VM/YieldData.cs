using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;

namespace Mellis.Lang.Python3.VM
{
    public class YieldData
    {
        public YieldData(
            IScriptType[] arguments,
            IClrYieldingFunction definition)
        {
            Arguments = arguments;
            Definition = definition;
        }

        public IScriptType[] Arguments { get; }
        public IClrYieldingFunction Definition { get; }
    }
}