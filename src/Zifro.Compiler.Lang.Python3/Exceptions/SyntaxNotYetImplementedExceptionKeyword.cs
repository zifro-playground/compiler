using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3.Exceptions
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