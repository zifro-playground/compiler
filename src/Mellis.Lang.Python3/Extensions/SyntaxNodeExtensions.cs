using System;
using System.Linq;
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
            return node as T ?? throw node.WrongTypeException(typeof(T));
        }

        public static InternalException WrongTypeException(this SyntaxNode node, params Type[] allowedTypes)
        {
            string joined = string.Join(", ", allowedTypes.Select(o => o?.Name ?? "null"));
            return new InternalException(
                nameof(Localized_Python3_Syntax.Ex_InvalidType),
                Localized_Python3_Syntax.Ex_InvalidType,
                joined, node?.GetType().Name ?? "null");
        }
    }
}