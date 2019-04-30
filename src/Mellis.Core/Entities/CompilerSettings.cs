using Mellis.Core.Interfaces;

namespace Mellis.Core.Entities
{
    /// <summary>
    /// Common compiler settings.
    /// </summary>
    public struct CompilerSettings
    {
        /// <summary>
        /// The setting for inserting breaks in the compiled code
        /// that when the processor walks over in <see cref="IProcessor.Walk"/>
        /// or walk line <see cref="IProcessor.WalkLine"/> shall break.
        /// <para>Default: <see cref="BreakCause.Nothing"/>, i.e. no breaks.</para>
        /// </summary>
        public BreakCause BreakOn { get; set; }

        /// <summary>
        /// Will try to optimize the produced code for speed and performance.
        /// Is not compatible with the processor walk line method <see cref="IProcessor.WalkLine"/>.
        /// <para>Default: <c>false</c>.</para>
        /// </summary>
        public bool Optimize { get; set; }

        /// <summary>
        /// Specifies a jump limit for each call to the processors
        /// walk method <see cref="IProcessor.Walk"/>
        /// or walk line method <see cref="IProcessor.WalkLine"/>.
        /// Is only active if value is non-zero positive number and
        /// break setting <see cref="BreakOn"/> has jump limit flag
        /// <see cref="BreakCause.JumpLimitReached"/> set.
        /// <para>Default: <c>239</c>, a Oliver &amp; Fredrik approved ✔ limit</para>
        /// </summary>
        public int JumpLimit { get; set; }

        /// <summary>
        /// Specifies an instruction (op-code) limit for each call to the processors
        /// walk method <see cref="IProcessor.Walk"/>
        /// or walk line method <see cref="IProcessor.WalkLine"/>.
        /// Is only active if value is non-zero positive number and
        /// break setting <see cref="BreakOn"/> has instruction limit flag
        /// <see cref="BreakCause.InstructionLimitReached"/> set.
        /// <para>Default: <c>0</c>, i.e. disabled</para>
        /// </summary>
        public int InstructionLimit { get; set; }

        /// <summary>
        /// Gets the default settings used in compilation among the language modules.
        /// </summary>
        public static CompilerSettings DefaultSettings { get; } = new CompilerSettings {
            BreakOn = BreakCause.Nothing,
            Optimize = false,
            JumpLimit = 102 + 137, // Oliver & Fredrik approved ✔
            InstructionLimit = 0
        };
    }
}