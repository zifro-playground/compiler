using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class Jump : IOpCode
    {
        public Jump(SourceReference source, Label label)
        {
            Source = source;
            Label = label;
        }

        public SourceReference Source { get; }

        public Label Label { get; }

        public virtual void Execute(PyProcessor processor)
        {
            if (Label.OpCodeIndex == -1)
                throw new InvalidOperationException("Label was not assigned an index. Are you sure it was added to the processor?");
            
            processor.JumpToInstruction(Label.OpCodeIndex);
        }

        public override string ToString()
        {
            return Label?.OpCodeIndex >= 0
                ? $"jmp->@{Label.OpCodeIndex}"
                : "jmp->{undefined}";
        }
    }
}