using System;
using Mellis.Core.Interfaces;

namespace Mellis.Core.Entities
{
    /// <summary>
    /// Cause of compile-time defined breakpoints. (Flags)
    /// </summary>
    [Flags]
    public enum BreakCause
    {
        Nothing = 0,

        /// <summary>
        /// A loop (for, repeat, while) was entered.
        /// <para>This only triggers once when the loop is first entered
        /// just before the condition is first checked.
        /// Only triggers once per loop.</para>
        /// </summary>
        LoopEnter = 0x1,
        /// <summary>
        /// A loop (for, repeat, while) has reached end of its code block.
        /// <para>This triggers just before the condition is checked again
        /// and can be triggered multiple times for the same loop.</para>
        /// </summary>
        LoopBlockEnd = 0x2,
        
        /// <summary>
        /// A user-defined function call is about to be invoked.
        /// <para>This triggers just before the call stack is pushed.</para>
        /// </summary>
        FunctionUserCall = 0x10,
        /// <summary>
        /// A CLR-defined function call <see cref="IClrFunction"/> is about to be invoked.
        /// <para>This triggers just before the call stack is pushed.</para>
        /// </summary>
        FunctionClrCall = 0x20,
        /// <summary>
        /// Any function call (user-defined or CLR-defined) is about to be invoked
        /// <para>This triggers just before the call stack is pushed.</para>
        /// </summary>
        FunctionAnyCall = FunctionUserCall | FunctionClrCall,

        /// <summary>
        /// Consecutive jumps count limit was reached.
        /// Is only enabled if compiler settings jump limit property
        /// <see cref="CompilerSettings.JumpLimit"/> is non-zero and positive.
        /// <para>This triggers just after the limited jump was jumped.</para>
        /// </summary>
        JumpLimitReached = 0x100,

        /// <summary>
        /// Consecutive instructions count limit per walk was reached.
        /// Is only enabled if compiler settings instruction limit property
        /// <see cref="CompilerSettings.InstructionLimit"/> is non-zero and positive.
        /// <para>This triggers just after the limited instruction was walked.</para>
        /// </summary>
        InstructionLimitReached = 0x200,
    }
}