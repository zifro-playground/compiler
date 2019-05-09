using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Resources;
using Mellis.Tools;

namespace Mellis.Lang.Python3.Entities
{
    public class PyBoolean : ScriptBoolean, IScriptInteger
    {
        int IScriptInteger.Value => Value ? 1 : 0;

        public PyBoolean(IProcessor processor, bool value)
            : base(processor, value)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyBooleanType(Processor);
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}