using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Entities.Functions;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        public void AddBuiltin(params IEmbeddedType[] builtinList)
        {
            foreach (IEmbeddedType embeddedType in builtinList)
            {
                _builtins.SetVariable(
                    embeddedType.FunctionName,
                    EmbeddedTypeToScriptType(embeddedType));
            }

            IScriptType EmbeddedTypeToScriptType(IEmbeddedType embedded)
            {
                switch (embedded)
                {
                case IClrFunction function:
                    return Factory.Create(function);

                case IClrYieldingFunction yieldingFunction:
                    return Factory.Create(yieldingFunction);

                default:
                    throw new ArgumentException();
                }
            }
        }

        private void AddBuiltinsInternal()
        {
            IScriptType[] builtinVariables = {
                // Base types
                new PyDoubleType(this, "float"),
                new PyIntegerType(this, "int"),
                new PyStringType(this, "str"),
                new PyBooleanType(this, "bool"),
                new PyType(this, "type"),

                // Special variables
                new PyString(this, "__main__", "__name__"),
            };

            foreach (IScriptType builtin in builtinVariables)
            {
                _builtins.SetVariableNoCopyUsingName(builtin);
            }

            IEmbeddedType[] builtinFunctions = {
                /* next */ new Next(), 
                /* iter */ new Iter(), 
            };
            
            AddBuiltin(builtinFunctions);
        }
    }
}