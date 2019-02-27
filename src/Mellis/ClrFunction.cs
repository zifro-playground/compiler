using Mellis.Core.Entities;
using Mellis.Core.Interfaces;

namespace Mellis
{
    public abstract class ClrFunction : IClrFunction
    {
        /// <param name="name">
        /// The name of this function.
        /// Used to identify it in the script environment and should therefore be a valid identifier.
        /// <para>Name rules: <code>(letter | '_') (letter | '_' | number)*</code></para>
        /// <para>Where <c>number</c> is <c>0-9</c></para>
        /// <para>Where <c>letter</c> is defined by all Unicode letter categories: UppercaseLetter, LowercaseLetter, TitlecaseLetter, ModifierLetter, and OtherLetter.</para>
        /// </param>
        protected ClrFunction(string name)
        {
            FunctionName = name;
        }

        /// <inheritdoc />
        public IProcessor Processor { get; set; }

        /// <inheritdoc />
        public string FunctionName { get; }

        /// <inheritdoc />
        public abstract IScriptType Invoke(IScriptType[] arguments);
    }
}