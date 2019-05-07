using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class NullBase : ScriptTypeBase
    {
        protected NullBase(IProcessor processor, string name = null) : base(processor, name)
        {
        }

        public override bool IsTruthy()
        {
            return false;
        }

        public override bool TryCoerce(Type type, out object value)
        {
            if (type == null || type == typeof(void))
            {
                value = null;
                return true;
            }

            value = default;
            return false;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Null_Name;
        }

        public override string ToString()
        {
            return "null";
        }
    }
}