using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public class SyntaxLiteralFormatException : SyntaxException
    {
        public SyntaxLiteralFormatException(SourceReference source)
            : base(source, nameof(Localized_Python3_Parser.Ex_Literal_Format),
                Localized_Python3_Parser.Ex_Literal_Format)
        {
        }
    }
}