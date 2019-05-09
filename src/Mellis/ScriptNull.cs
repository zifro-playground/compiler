using Mellis.Core.Interfaces;
using Mellis.Resources;

namespace Mellis
{
    public abstract class ScriptNull : ScriptType
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

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            return Processor.Factory.Create(rhs is ScriptNull);
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            return Processor.Factory.Create(!(rhs is ScriptNull));
        }
    }
}