namespace Mellis.Core.Entities
{
    public enum ProcessState
    {
        /// <summary>
        /// Interpreter has just been initialized and not yet executed its first instruction.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Execution is in the middle of the interpreters instructions.
        /// </summary>
        Running,

        /// <summary>
        /// Interpreter has paused in anticipation of a yielded function return.
        /// </summary>
        Yielded,

        /// <summary>
        /// Exception was thrown from last instruction.
        /// </summary>
        Error,

        /// <summary>
        /// End of instructions
        /// </summary>
        Ended
    }
}