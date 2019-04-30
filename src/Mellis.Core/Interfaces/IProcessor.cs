using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Resources;

namespace Mellis.Core.Interfaces
{
    public interface IProcessor
    {
        /// <summary>
        /// Value factory. Use this to create new values, bound to this
        /// processor.
        /// </summary>
        IScriptTypeFactory Factory { get; }

        IScopeContext GlobalScope { get; }

        /// <summary>
        /// Current scope for the processor.
        /// </summary>
        IScopeContext CurrentScope { get; }

        /// <summary>
        /// Current state in the process.
        /// </summary>
        ProcessState State { get; }

        /// <summary>
        /// Current source pointer of the execution.
        /// </summary>
        SourceReference CurrentSource { get; }

        /// <summary>
        /// Last thrown error from inside the processor during a walk.
        /// Does not record "process ended" nor "already yielded" errors.
        /// </summary>
        InterpreterException LastError { get; }

        /// <summary>
        /// Compiler settings used when compiling this processor.
        /// </summary>
        CompilerSettings CompilerSettings { get; }

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

        /// <summary>
        /// Adds CLR functions to list of builtins.
        /// Functions added this way does not show up in the current scope
        /// but are still accessible in the code.
        /// </summary>
        void AddBuiltin(params IEmbeddedType[] builtinList);
    }
}