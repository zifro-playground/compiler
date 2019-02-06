using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Resources;

namespace Zifro.Compiler.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Use this for compile-time exceptions where the used feature is not yet implemented by the compiler.
    /// </summary>
    public class SyntaxNotYetImplementedException : SyntaxException
    {
        public SyntaxNotYetImplementedException(SourceReference source, string ruleName)
            : base(source,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                Localized_Exceptions.Ex_Syntax_NotYetImplemented,
                ruleName)
        {
        }
    }
}