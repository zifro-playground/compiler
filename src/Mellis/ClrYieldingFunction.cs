using Mellis.Core.Interfaces;

namespace Mellis
{
    public abstract class ClrYieldingFunction : IClrYieldingFunction
    {
        /// <param name="name">
        /// The name of this function.
        /// Used to identify it in the script environment and should therefore be a valid identifier.
        /// <para>Name rules: <code>(letter | '_') (letter | '_' | number)*</code></para>
        /// <para>Where <c>number</c> is <c>0-9</c></para>
        /// <para>Where <c>letter</c> is defined by all Unicode letter categories: UppercaseLetter, LowercaseLetter, TitlecaseLetter, ModifierLetter, and OtherLetter.</para>
        /// </param>
        protected ClrYieldingFunction(string name)
        {
            FunctionName = name;
        }

        /// <inheritdoc />
        public IProcessor Processor { get; set; }

        /// <inheritdoc />
        public string FunctionName { get; }

        /// <inheritdoc />
        public abstract void InvokeEnter(IScriptType[] arguments);

        /// <inheritdoc />
        /// <summary>
        /// Called after <see cref="IProcessor.ResolveYield()"/> has been executed.
        /// Allows modifications to the returned value.
        /// <para>
        /// Default implementation in the base class <see cref="ClrYieldingFunction"/>
        /// is to just forward the returned value.
        /// </para>
        /// </summary>
        public virtual IScriptType InvokeExit(IScriptType[] arguments, IScriptType returnValue)
        {
            return returnValue;
        }
    }
}