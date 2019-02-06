using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;

namespace Zifro.Compiler.Lang.Python3.Extensions
{
    public static class ParserRuleContextExtensions
    {
        public static IEnumerable<IParseTree> GetChildren(this ParserRuleContext context)
        {
            for (var i = 0; i < context.ChildCount; i++)
            {
                yield return context.GetChild(i);
            }
        }

        public static SourceReference GetSourceReference(this ParserRuleContext context)
        {
            return new SourceReference(
                fromRow: context.Start.Line, toRow: context.Stop.Line,
                fromColumn: context.Start.Column, toColumn: context.Stop.Column);
        }
    }
}