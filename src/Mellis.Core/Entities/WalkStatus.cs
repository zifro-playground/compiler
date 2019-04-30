using Mellis.Core.Interfaces;

namespace Mellis.Core.Entities
{
    /// <summary>
    /// Returned status from the processor walk <see cref="IProcessor.Walk"/>
    /// or walk line <see cref="IProcessor.WalkLine"/> call.
    /// </summary>
    public enum WalkStatus
    {
        /// <summary>
        /// Script execution completed. No more instructions to walk.
        /// <para>Matches the process state <see cref="ProcessState"/>
        /// ended value <see cref="ProcessState.Ended"/>.</para>
        /// </summary>
        Ended,

        /// <summary>
        /// A yielding function was called.
        /// Use the resolve yield method <see cref="IProcessor.ResolveYield()"/>
        /// on processor <see cref="IProcessor"/> to release the yield lock.
        /// <para>Matches the process state <see cref="ProcessState"/>
        /// yielded value <see cref="ProcessState.Yielded"/>.</para>
        /// </summary>
        Yielded,

        /// <summary>
        /// Walker finished all instructions before line switches.
        /// <para>Note: Only applicable for processor <see cref="IProcessor"/>
        /// walk line method <see cref="IProcessor.WalkLine"/>.</para>
        /// </summary>
        NewLine,

        /// <summary>
        /// A breakpoint defined at compile-time with <see cref="BreakCause"/> in the
        /// <see cref="ICompiler"/> compiler property <see cref="ICompiler.Settings"/>.
        /// </summary>
        Break,
    }
}