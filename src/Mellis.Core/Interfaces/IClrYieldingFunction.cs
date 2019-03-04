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
    public interface IClrYieldingFunction
    {
        /// <summary>
        /// The assigned processor environment.
        /// Set by the processor when function is created via the processors <see cref="IScriptTypeFactory"/> factory.
        /// </summary>
        IProcessor Processor { set; }

        /// <summary>
        /// Get the name of this function.
        /// Used to identify it in the script environment and should therefore be a valid identifier.
        /// <para>Name rules: <code>(letter | '_') (letter | '_' | number)*</code></para>
        /// <para>Where <c>number</c> is <c>0-9</c></para>
        /// <para>Where <c>letter</c> is defined by all Unicode letter categories: UppercaseLetter, LowercaseLetter, TitlecaseLetter, ModifierLetter, and OtherLetter.</para>
        /// </summary>
        string FunctionName { get; }

        /// <summary>
        /// Executed by the processor when the function is invoked in the script environment.
        /// <para>
        /// It is expected that the executing environment invokes the processors continue function
        /// <see cref="IProcessor.ResolveYield()"/>
        /// to resume execution.
        /// </para>
        /// </summary>
        void Invoke(IScriptType[] arguments);
    }
}