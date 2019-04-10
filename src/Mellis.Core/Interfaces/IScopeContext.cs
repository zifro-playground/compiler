using System.Collections.Generic;

namespace Mellis.Core.Interfaces
{
    public interface IScopeContext
    {
        /// <summary>
        /// Parent scope, if any. Otherwise null.
        /// </summary>
        IScopeContext ParentScope { get; }

        /// <summary>
        /// Collection of all variables defined in this scope.
        /// </summary>
        IReadOnlyDictionary<string, IScriptType> Variables { get; }

        /// <summary>
        /// Gets a list of all the names defined in this scope.
        /// </summary>
        string[] ListVariableNames();

        /// <summary>
        /// Gets a list of all the (distinct) names defined in this scope
        /// and its parent scopes combined.
        /// </summary>
        string[] ListVariableNamesUpwards();
    }
}