using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Resources;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;

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

        public static SourceReference GetSourceReference(this ITerminalNode node)
        {
            return new SourceReference(
                fromRow: node.Symbol.Line,
                toRow: node.Symbol.Line, // assumes same line
                fromColumn: node.Symbol.Column,
                toColumn: node.Symbol.Column + node.Symbol.Text.Length - 1);
        }

        public static SyntaxNotYetImplementedException NotYetImplementedException(this ParserRuleContext context)
        {
            return new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public static SyntaxNotYetImplementedExceptionKeyword NotYetImplementedException(this ParserRuleContext context,
            string keyword)
        {
            return new SyntaxNotYetImplementedExceptionKeyword(context.GetSourceReference(), keyword);
        }
    }
}