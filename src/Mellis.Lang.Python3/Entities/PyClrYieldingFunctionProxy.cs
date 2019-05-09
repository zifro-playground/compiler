﻿using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyClrYieldingFunctionProxy : ScriptClrYieldingFunctionProxy
    {
        public PyClrYieldingFunctionProxy(
            IProcessor processor,
            IClrYieldingFunction definition)
            : base(processor, definition)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyType<ScriptClrFunction>(Processor, Localized_Base_Entities.Type_ClrFunction_Name);
        }
    }
}