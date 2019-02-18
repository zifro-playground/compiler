using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public class SyntaxNotYetImplementedExceptionKeyword : SyntaxNotYetImplementedException
    {
        public string Keyword { get; }

        public SyntaxNotYetImplementedExceptionKeyword(SourceReference source, string keyword)
            : base(source,
                nameof(Localized_Python3_Parser.Ex_Syntax_NotYetImplemented_Keyword),
                Localized_Python3_Parser.Ex_Syntax_NotYetImplemented_Keyword,
                keyword)
        {
            Keyword = keyword;
        }
    }
}