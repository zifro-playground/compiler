using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Resources;
using Mellis.Tools;

namespace Mellis.Lang.Python3.Entities
{
    public class PyBoolean : PyInteger, IScriptBoolean
    {
        public new bool Value => base.Value != 0;

        public PyBoolean(IProcessor processor, bool value)
            : base(processor, value ? 1 : 0)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyBooleanType(Processor);
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Boolean_Name;
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}