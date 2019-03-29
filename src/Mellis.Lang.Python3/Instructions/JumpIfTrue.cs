using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class JumpIfTrue : Jump
    {
        public bool Peek { get; }

        public JumpIfTrue(SourceReference source, int target = -1, bool peek = false)
            : base(source, target)
        {
            Peek = peek;
        }

        public override void Execute(PyProcessor processor)
        {
            IScriptType value = Peek 
                ? processor.PeekValue()
                : processor.PopValue();

            if (value.IsTruthy())
            {
                processor.JumpToInstruction(Target);
            }
        }

        public override string ToString()
        {
            return $"jmpif[{(Peek ? "peek" : "pop")}]->@{Target}";
        }
    }
}