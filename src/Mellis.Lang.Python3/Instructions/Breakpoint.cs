using System;
using System.Diagnostics.Contracts;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
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
            // Do nothing
        }

        [Pure]
        public bool ShouldBreak(PyProcessor processor, IOpCode nextOpCode)
        {
            if ((BreakCause & BreakCause.FunctionAnyCall) != 0)
            {
                if (!(nextOpCode is Call call))
                {
                    return false;
                }

                if (BreakCause.HasFlag(BreakCause.FunctionAnyCall))
                {
                    return true;
                }

                IScriptType func = processor.PeekValue(call.ArgumentCount);

                // Assume not user func if CLR func
                if (func is IClrFunction || func is IClrYieldingFunction)
                {
                    return BreakCause.HasFlag(BreakCause.FunctionClrCall);
                }

                if ((BreakCause & BreakCause.FunctionAnyCall) == BreakCause.FunctionUserCall)
                {
                    // Only user call? Nah not yet
                    // TODO
                    throw new NotImplementedException("User functions not yet supported.");
                }

                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"break->{BreakCause}";
        }
    }
}