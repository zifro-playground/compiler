using System;
using Mellis.Core.Entities;
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
        public abstract void Invoke(IScriptType[] arguments);

        /// <inheritdoc />
        /// <summary>
        /// Should not be used.
        /// You must call the yielding invoke <see cref="Invoke"/> on yielding functions.
        /// </summary>
        [Obsolete("You must call the yielding invoke on yielding functions.", true)]
        IScriptType IClrFunction.Invoke(IScriptType[] arguments)
        {
            throw new NotSupportedException("You must call the yielding invoke on yielding functions.");
        }
    }
}