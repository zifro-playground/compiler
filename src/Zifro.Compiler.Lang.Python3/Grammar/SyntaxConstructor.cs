using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor : AbstractParseTreeVisitor<SyntaxNode>, IPython3Visitor<SyntaxNode>
    {
        public SyntaxNode VisitSingle_input(Python3Parser.Single_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        public SyntaxNode VisitFile_input(Python3Parser.File_inputContext context)
        {
            return VisitChildren(context);
        }

        public SyntaxNode VisitEval_input(Python3Parser.Eval_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        private static SourceReference SourceReference(ParserRuleContext context)
        {
            return new SourceReference(context.Start.Line, context.Stop.Line, context.Start.Column, context.Stop.Column);
        }
    }
}