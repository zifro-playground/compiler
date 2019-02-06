using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor : Python3BaseVisitor<SyntaxNode>
    {
        public override SyntaxNode VisitSingle_input(Python3Parser.Single_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        public override SyntaxNode VisitFile_input(Python3Parser.File_inputContext context)
        {
            return VisitChildren(context);
        }

        public override SyntaxNode VisitEval_input(Python3Parser.Eval_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        private static SourceReference SourceReference(ParserRuleContext context)
        {
            return new SourceReference(context.Start.Line, context.Stop.Line, context.Start.Column, context.Stop.Column);
        }
    }
}