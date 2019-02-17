using Mellis.Core.Entities;
using Mellis.Core.Interfaces;

namespace Mellis.Proxies
{
    public class Variable : ProxyBase
    {
        /// <summary>
        /// Gets the name of the declared variable.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the source of where the variable was declared.
        /// </summary>
        public SourceReference Source { get; }

        /// <summary>
        /// Gets whether or not this variable was declared in the CLR or in the script environment.
        /// </summary>
        public bool IsFromClr { get; }

        /// <param name="innerValue">The inner value to wrap.</param>
        /// <param name="name">Name of the declared variable.</param>
        /// <param name="source">Source of where the variable was declared.</param>
        /// <param name="isClr">Whether or not this variable was declared in the CLR or in the script environment.</param>
        public Variable(IScriptType innerValue, string name, SourceReference source, bool isClr)
            : base(innerValue)
        {
            Name = name;
            Source = source;
            IsFromClr = isClr;
        }
    }
}