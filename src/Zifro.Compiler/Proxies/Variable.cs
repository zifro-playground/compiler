using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Proxies
{
    public class Variable : ValueProxyBase
    {
        /// <summary>
        /// Name of the declared variable.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Source of where the variable was declared.
        /// </summary>
        public ISourceReference Source { get; }

        /// <param name="innerValue">The inner value to wrap.</param>
        /// <param name="name">Name of the declared variable.</param>
        /// <param name="source">Source of where the variable was declared.</param>
        public Variable(IValueType innerValue, string name, ISourceReference source)
            : base(innerValue)
        {
            Name = name;
            Source = source;
        }
    }
}