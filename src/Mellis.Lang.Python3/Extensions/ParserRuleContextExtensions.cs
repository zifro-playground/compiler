using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Extensions
{
    public static class ParserRuleContextExtensions
    {
        public static IEnumerable<IParseTree> GetChildren(this ParserRuleContext context)
        {
            for (int i = 0; i < context.ChildCount; i++)
            {
                yield return context.GetChild(i);
            }
        }

        public static SourceReference GetSourceReference(this ParserRuleContext context)
        {
#if DEBUG
            if (context.Start == null)
            {
                throw new InterpreterException($"{Python3Parser.ruleNames[context.RuleIndex]}.Start == null");
            }

            if (context.Stop == null)
            {
                throw new InterpreterException($"{Python3Parser.ruleNames[context.RuleIndex]}.Stop == null");
            }
#endif
            return new SourceReference(
                fromRow: context.Start.Line, toRow: context.Stop.Line,
                fromColumn: context.Start.Column, toColumn: context.Stop.Column);
        }

        public static SourceReference GetSourceReference(this ITerminalNode node)
        {
            int stopOffset = node.Symbol.StartIndex == -1
                ? -1 // since it's missing
                : node.Symbol.StopIndex - node.Symbol.StartIndex;

            return new SourceReference(
                fromRow: node.Symbol.Line,
                toRow: node.Symbol.Line, // assumes same line
                fromColumn: node.Symbol.Column,
                toColumn: node.Symbol.Column + stopOffset);
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

        public static SyntaxNotYetImplementedExceptionKeyword NotYetImplementedException(this ITerminalNode terminal)
        {
            return new SyntaxNotYetImplementedExceptionKeyword(terminal.GetSourceReference(), terminal.Symbol.Text);
        }

        public static SyntaxNotYetImplementedExceptionKeyword NotYetImplementedException(this ITerminalNode terminal,
            string keywordOverride)
        {
            return new SyntaxNotYetImplementedExceptionKeyword(terminal.GetSourceReference(), keywordOverride);
        }

        public static SyntaxException UnexpectedChildType(this ParserRuleContext context, ParserRuleContext childRule)
        {
            return new SyntaxException(childRule.GetSourceReference(),
                nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType,
                Python3Parser.ruleNames[context.RuleIndex],
                Python3Parser.ruleNames[childRule.RuleIndex]);
        }

        public static SyntaxException UnexpectedChildType(this ParserRuleContext context, ITerminalNode childTerm)
        {
            return new SyntaxException(childTerm.GetSourceReference(),
                nameof(Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType),
                Localized_Python3_Parser.Ex_Syntax_UnexpectedChildType,
                Python3Parser.ruleNames[context.RuleIndex],
                childTerm.Symbol.Text);
        }

        public static SyntaxException UnexpectedChildType(this ParserRuleContext context, IParseTree childTree)
        {
            switch (childTree)
            {
                case ITerminalNode terminal:
                    return context.UnexpectedChildType(terminal);
                case ParserRuleContext rule:
                    return context.UnexpectedChildType(rule);

                default:
                    throw new InternalException("_unexpected_parse_tree_",
                        "Unexpected tree item type: " + childTree.GetType().Name);
            }
        }

        public static SyntaxException ExpectedChild(this ParserRuleContext context)
        {
            return new SyntaxException(context.GetSourceReference(),
                nameof(Localized_Python3_Parser.Ex_Syntax_ExpectedChild),
                Localized_Python3_Parser.Ex_Syntax_ExpectedChild,
                Python3Parser.ruleNames[context.RuleIndex]);
        }

        public static ITerminalNode GetChildOrThrow(this ParserRuleContext context, int index, int expectedType)
        {
            if (index >= context.ChildCount)
            {
                throw context.ExpectedChild();
            }

            IParseTree parseTree = context.GetChild(index);
            if (!(parseTree is ITerminalNode terminal))
            {
                throw context.UnexpectedChildType((ParserRuleContext) parseTree);
            }

            if (terminal.Symbol.Type != expectedType)
            {
                throw context.UnexpectedChildType(terminal);
            }

            return terminal;
        }

        public static T GetChildOrThrow<T>(this ParserRuleContext context, int index)
            where T : IParseTree
        {
            if (index >= context.ChildCount)
            {
                throw context.ExpectedChild();
            }

            IParseTree parseTree = context.GetChild(index);
            if (!(parseTree is T rule))
            {
                throw context.UnexpectedChildType(parseTree);
            }

            return rule;
        }

        public static void ThrowIfMissing(this ITerminalNode terminal,
            string localizedPython3ParserKey,
            params object[] additionalFormatArgs)
        {
            if (terminal.IsMissing())
            {
                throw new SyntaxException(terminal.GetSourceReference(),
                    localizedPython3ParserKey,
                    Localized_Python3_Parser.ResourceManager.GetString(localizedPython3ParserKey),
                    additionalFormatValues: additionalFormatArgs);
            }
        }

        public static bool IsMissing(this ITerminalNode terminal)
        {
            return terminal.Symbol.StartIndex == -1 ||
                            terminal.Symbol.StopIndex == -1;
        }

        public static ITerminalNode ExpectClosingParenthesis(this ParserRuleContext context, ITerminalNode opening,
            int closingType)
        {
            IParseTree last = context.GetChild(context.ChildCount - 1);

            if (!(last is ITerminalNode node) || // not terminal node
                node.Symbol.Type != closingType || // wrong symbol
                node.Symbol.StartIndex == -1) // missing
            {
                throw new SyntaxException(opening.GetSourceReference(),
                    nameof(Localized_Python3_Parser.Ex_Parenthesis_NoClosing),
                    Localized_Python3_Parser.Ex_Parenthesis_NoClosing,
                    Python3Parser.DefaultVocabulary.GetLiteralName(closingType).Trim('\''));
            }

            return node;
        }
    }
}