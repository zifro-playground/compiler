using Mellis.Core.Interfaces;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        public void AddBuiltin(params IClrFunction[] builtinList)
        {
            foreach (IClrFunction function in builtinList)
            {
                _builtins.SetVariable(function.FunctionName, Factory.Create(function));
            }
        }
    }
}