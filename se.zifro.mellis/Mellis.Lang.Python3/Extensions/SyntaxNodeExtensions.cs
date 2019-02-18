using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static T AsTypeOrThrow<T>(this SyntaxNode node)
            where T : SyntaxNode
        {
            return node as T ??
                   throw new InternalException(
                       nameof(Localized_Python3_Syntax.Ex_InvalidType),
                       Localized_Python3_Syntax.Ex_InvalidType,
                       typeof(T).Name, node?.GetType().Name ?? "null");
        }
    }
}