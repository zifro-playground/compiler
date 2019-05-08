using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptNull : ScriptBaseType
    {
        protected ScriptNull(IProcessor processor) : base(processor)
        {
        }

        public override bool IsTruthy()
        {
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