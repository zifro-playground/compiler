using Mellis.Core.Entities;

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
    public interface IClrYieldingFunction : IClrFunction
    {
        /// <summary>
        /// Executed by the processor when the function is invoked in the script environment.
        /// <para>
        /// It is expected that the executing environment invokes the processors continue function
        /// <see cref="IProcessor.ResolveYield()"/>
        /// to resume execution.
        /// </para>
        /// </summary>
        new void Invoke(IScriptType[] arguments);
    }
}