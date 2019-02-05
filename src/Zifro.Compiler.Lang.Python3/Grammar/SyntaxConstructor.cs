using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor : AbstractParseTreeVisitor<IParseTree>, IPython3Visitor<IParseTree>
    {
        public IParseTree VisitSingle_input(Python3Parser.Single_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        public IParseTree VisitFile_input(Python3Parser.File_inputContext context)
        {
            return VisitChildren(context);
        }

        public IParseTree VisitEval_input(Python3Parser.Eval_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        private static SourceReference SourceReference(ParserRuleContext context)
        {
            return new SourceReference(context.Start.Line, context.Stop.Line, context.Start.Column, context.Stop.Column);
        }
    }
}