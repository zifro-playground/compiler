using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Core.Interfaces
{
    public interface IFunction
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
        string Name { get; }

        /// <summary>
        /// Get the description of this function. Used primarily in exceptions.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get the source of where the variable was declared.
        /// </summary>
        SourceReference Source { get; }

        /// <summary>
        /// Executed by the processor when the function is invoked in the script environment.
        /// </summary>
        IScriptType Invoke(IScriptType[] arguments);
    }
}