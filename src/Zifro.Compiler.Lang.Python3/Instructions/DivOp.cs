using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class DivOp : BaseBinaryOp
    {
        public DivOp(SourceReference source) : base(source)
        {
        }

        protected override IScriptType Evaluate(IScriptType lhs, IScriptType rhs)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "div /";
        }
    }
}