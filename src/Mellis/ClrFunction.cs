using Mellis.Core.Entities;
using Mellis.Core.Interfaces;

namespace Mellis
{
    public abstract class ClrFunctionBase : IFunction
    {
        #region Predefined properties

        public IProcessor Processor { get; set; }

        public SourceReference Source { get; } = SourceReference.ClrSource;

        #endregion

        #region Abstract properties

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract IScriptType Invoke(IScriptType[] arguments);

        #endregion
    }
}