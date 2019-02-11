using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Resources;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Resources;

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
#if DEBUG
            if (context.Start == null)
                throw new InterpreterException($"{Python3Parser.ruleNames[context.RuleIndex]}.Start == null");
            if (context.Stop == null)
                throw new InterpreterException($"{Python3Parser.ruleNames[context.RuleIndex]}.Stop == null");
#endif
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

        public static SyntaxNotYetImplementedExceptionKeyword NotYetImplementedException(this ITerminalNode terminal)
        {
            return new SyntaxNotYetImplementedExceptionKeyword(terminal.GetSourceReference(), terminal.Symbol.Text);
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
                    throw new InternalException("_unexpected_parse_tree_", "Unexpected tree item type: " + childTree.GetType().Name);
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
                throw context.ExpectedChild();

            IParseTree parseTree = context.GetChild(index);
            if (!(parseTree is ITerminalNode terminal))
                throw context.UnexpectedChildType((ParserRuleContext)parseTree);
            if (terminal.Symbol.Type != expectedType)
                throw context.UnexpectedChildType(terminal);
            return terminal;
        }

        public static T GetChildOrThrow<T>(this ParserRuleContext context, int index)
            where T : IParseTree
        {
            if (index >= context.ChildCount)
                throw context.ExpectedChild();

            IParseTree parseTree = context.GetChild(index);
            if (!(parseTree is T rule))
                throw context.UnexpectedChildType(parseTree);
            return rule;
        }
    }
}