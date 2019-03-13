using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyType : ScriptTypeBase
    {
        public PyType(IProcessor processor, string name = null)
            : base(processor, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyType(Processor, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return string.IsNullOrEmpty(Name)
                ? this : new PyType(Processor);
        }

        public override string GetTypeName()
        {
            return Localized_Python3_Entities.Type_Type_Name;
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public override bool TryConvert(Type type, out object value)
        {
            value = default;
            return false;
        }

        public override string ToString()
        {
            return string.Format(Localized_Python3_Entities.Type_Type_ToString,
                /* {0} */ GetTypeName()
            );
        }
    }
}