using Antlr4.Runtime.Tree;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitStmt(Python3Parser.StmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public override SyntaxNode VisitSimple_stmt(Python3Parser.Simple_stmtContext context)
        {
            return VisitChildren(context);
        }

        public override SyntaxNode VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public override SyntaxNode VisitCompound_stmt(Python3Parser.Compound_stmtContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}