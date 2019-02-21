using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities;

namespace Mellis.Lang.Python3
{
    public partial class PyProcessor
    {
        public void AddBuiltin(params IClrFunction[] builtinList)
        {
            foreach (IClrFunction function in builtinList)
            {
                _builtins.SetVariable(function.Name, Factory.Create(function));
            }
        }
    }
}