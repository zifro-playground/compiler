using System;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class Jump : IOpCode
    {
        public Jump(SourceReference source, int target = -1)
        {
            Source = source;
            Target = target;
        }

        public SourceReference Source { get; }

        public int Target { get; set; }

        public virtual void Execute(PyProcessor processor)
        {
            processor.JumpToInstruction(Target);
        }

        public override string ToString()
        {
            return $"jmp->@{Target}";
        }
    }
}