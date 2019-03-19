using Mellis.Core.Entities;
using Mellis.Core.Resources;

namespace Mellis.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Use this for compile-time exceptions where the used feature is not yet implemented by the compiler.
    /// </summary>
    public class SyntaxNotYetImplementedException : SyntaxException
    {
        public SyntaxNotYetImplementedException(SourceReference source)
            : base(source,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                Localized_Exceptions.Ex_Syntax_NotYetImplemented)
        {
        }

        protected SyntaxNotYetImplementedException(SourceReference source,
            string localizedKey, string localizedMessageFormat, params object[] additionalFormatValues)
            : base(source, localizedKey, localizedMessageFormat, additionalFormatValues: additionalFormatValues)
        {

        }
    }
}