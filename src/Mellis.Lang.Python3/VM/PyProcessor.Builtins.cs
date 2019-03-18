using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;

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

        private void AddBuiltinsInternal()
        {
            IScriptType[] builtins = {
                // Base types
                new PyDoubleType(this, "float"),
                new PyIntegerType(this, "int"),
                new PyStringType(this, "str"), 
                new PyBooleanType(this, "bool"),
                new PyType(this, "type"),

                // Special variables
                new PyString(this, "__main__", "__name__"),
            };

            foreach (IScriptType builtin in builtins)
            {
                _builtins.SetVariableNoCopyUsingName(builtin);
            }
        }
    }
}