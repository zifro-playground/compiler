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

        void WalkLine();

        void AddBuiltin(params IEmbeddedType[] builtinList);
    }
}