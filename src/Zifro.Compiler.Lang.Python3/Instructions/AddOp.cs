using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class AddOp : BaseBinaryOp
    {
        public AddOp(SourceReference source)
            : base(source)
        {
        }

        protected override IScriptType Evaluate(IScriptType lhs, IScriptType rhs)
        {
            return lhs.ArithmeticAdd(rhs);
        }

        public override string ToString()
        {
            return "add +";
        }
    }
}