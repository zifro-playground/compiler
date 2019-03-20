namespace Mellis.Core.Interfaces
{
    /// <summary>
    /// Used to define function calls that yields the execution mid-function,
    /// and awaits a value to be returned by the executing environment
    /// via invokes the processors continue function
    /// <see cref="IProcessor.ResolveYield()"/>.
    /// <para>
    /// This can be turned into a value via <seealso cref="IScriptTypeFactory.Create(IClrYieldingFunction)"/>.
    /// </para>
    /// </summary>
    public interface IClrYieldingFunction : IEmbeddedType
    {
        /// <summary>
        /// Executed by the processor when the yielding function is entered in the script environment.
        /// <para>
        /// It is expected that the executing environment invokes the processors continue function
        /// <see cref="IProcessor.ResolveYield()"/>
        /// to resume execution.
        /// </para>
        /// </summary>
        void InvokeEnter(params IScriptType[] arguments);

        /// <summary>
        /// Called after <see cref="IProcessor.ResolveYield()"/> has been executed.
        /// Allows modifications to the returned value.
        /// </summary>
        /// <param name="arguments">The arguments from the original start call <see cref="InvokeEnter"/>.</param>
        /// <param name="returnValue">The returned value from <see cref="IProcessor.ResolveYield()"/>.</param>
        IScriptType InvokeExit(IScriptType[] arguments, IScriptType returnValue);
    }
}