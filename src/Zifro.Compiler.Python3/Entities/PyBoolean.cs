using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Entities;

namespace Zifro.Compiler.Lang.Python3.Entities
{
    public class PyBoolean : BooleanBase
    {
        public PyBoolean(bool value, PyProcessor processor)
            : base(processor)
        {
            Value = value;
        }

        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }
    }
}