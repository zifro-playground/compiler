using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class JumpIfFalse : Jump
    {
        public JumpIfFalse(SourceReference source, Label label) : base(source, label)
        {
        }

        public override void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }
    }
}