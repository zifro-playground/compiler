using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3.Exceptions
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