using System;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class Breakpoint : IOpCode
    {
        public SourceReference Source { get; }
        public BreakCause BreakCause { get; }

        public Breakpoint(SourceReference source, BreakCause breakCause)
        {
            Source = source;
            BreakCause = breakCause;
        }

        public Breakpoint(BreakCause breakCause)
            : this(SourceReference.ClrSource, breakCause)
        {
        }

        public void Execute(PyProcessor processor)
        {
        }

        public override string ToString()
        {
            return $"break->{BreakCause}";
        }
    }
}