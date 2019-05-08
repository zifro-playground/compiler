using System;
using System.Collections.Generic;
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
                case IScriptType scriptType:
                    return scriptType;

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
            var builtinVariables = new Dictionary<string, IScriptType> {
                // Literal types
                ["float"] = new PyDoubleType(this),
                ["int"] = new PyIntegerType(this),
                ["str"] = new PyStringType(this),
                ["bool"] = new PyBooleanType(this),

                // Special objects
                ["type"] = new PyType(this),
                ["range"] = new PyRangeType(this),

                // Special variables
                ["__name__"] = new PyString(this, "__main__"),
            };

            foreach (KeyValuePair<string, IScriptType> builtin in builtinVariables)
            {
                _builtins.SetVariable(builtin.Key, builtin.Value);
            }

            IEmbeddedType[] builtinFunctions = {
                /* next */ new Next(), 
                /* iter */ new Iter(), 
            };
            
            AddBuiltin(builtinFunctions);
        }
    }
}