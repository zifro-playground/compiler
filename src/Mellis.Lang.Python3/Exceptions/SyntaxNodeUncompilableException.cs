using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public class SyntaxUncompilableException : SyntaxException
    {
        public Type UncompilableType { get; }

        public SyntaxUncompilableException(SourceReference source, Type syntaxType)
            : base(
                source,
                nameof (Localized_Python3_Syntax.Ex_SyntaxNode_Uncompilable),
                Localized_Python3_Syntax.Ex_SyntaxNode_Uncompilable,
                syntaxType.Name)
        {
            UncompilableType = syntaxType;
        }
    }
}