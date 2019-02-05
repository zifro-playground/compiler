using Antlr4.Runtime.Tree;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public SyntaxNode VisitStmt(Python3Parser.StmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public SyntaxNode VisitSimple_stmt(Python3Parser.Simple_stmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public SyntaxNode VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public SyntaxNode VisitCompound_stmt(Python3Parser.Compound_stmtContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}