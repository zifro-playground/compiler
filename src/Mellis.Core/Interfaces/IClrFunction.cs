using Mellis.Core.Entities;

namespace Mellis.Core.Interfaces
{
    public interface IClrFunction
    {
        /// <summary>
        /// The assigned processor environment.
        /// Set by the processor when function is created via the processors <see cref="IScriptTypeFactory"/> factory.
        /// <para>
        /// This can be turned into a value via <seealso cref="IScriptTypeFactory.Create(IClrFunction)"/>.
        /// </para>
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
        /// </summary>
        IScriptType Invoke(IScriptType[] arguments);
    }
}