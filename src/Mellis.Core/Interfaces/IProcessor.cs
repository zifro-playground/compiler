using Mellis.Core.Entities;
using Mellis.Core.Exceptions;

namespace Mellis.Core.Interfaces
{
    public interface IProcessor
    {
        IScriptTypeFactory Factory { get; }
        IScopeContext GlobalScope { get; }
        IScopeContext CurrentScope { get; }
        ProcessState State { get; }
        SourceReference CurrentSource { get; }

        InterpreterException LastError { get; }

        /// <summary>
        /// Resolves a yielding function <see cref="IClrYieldingFunction"/>
        /// with specified return value and allows walking operations
        /// <seealso cref="WalkLine"/> to be executed again.
        /// <para>Can only be called while the state variable <see cref="State"/>
        /// is in the yielded state <see cref="ProcessState.Yielded"/>.</para>
        /// </summary>
        /// <param name="returnValue">The return value from the yielded function.</param>
        void ResolveYield(IScriptType returnValue);

        /// <summary>
        /// Resolves a yielding function <see cref="IClrYieldingFunction"/>
        /// with return value same as <see cref="IScriptTypeFactory.Null"/>
        /// from this processors internal factory variable <seealso cref="Factory"/>
        /// and allows walking operations  <seealso cref="WalkLine"/>
        /// to be executed again.
        /// <para>Can only be called while the state variable <see cref="State"/>
        /// is in the yielded state <see cref="ProcessState.Yielded"/>.</para>
        /// </summary>
        void ResolveYield();

        /// <summary>
        /// Walks the instructions and stops if a new line was reached,
        /// a yielding function <seealso cref="IClrYieldingFunction"/> was called,
        /// a breakpoint defined in the compiler settings was reached,
        /// or the processor finishes all instructions.
        /// <para>See walk status enum <see cref="WalkStatus"/> for more info.</para>
        /// </summary>
        WalkStatus WalkLine();

        /// <summary>
        /// Walks the instructions and stops if
        /// a yielding function <seealso cref="IClrYieldingFunction"/> was called,
        /// a breakpoint <see cref="BreakCause"/> in the compiler settings was reached,
        /// or the processor finishes all instructions.
        /// <para>See walk status enum <see cref="WalkStatus"/> for more info.</para>
        /// </summary>
        WalkStatus Walk();

        void AddBuiltin(params IEmbeddedType[] builtinList);
    }
}