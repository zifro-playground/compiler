using Antlr4.Runtime.Tree;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public IParseTree VisitStmt(Python3Parser.StmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public IParseTree VisitSimple_stmt(Python3Parser.Simple_stmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public IParseTree VisitSmall_stmt(Python3Parser.Small_stmtContext context)
        {
            throw new System.NotImplementedException();
        }

        public IParseTree VisitCompound_stmt(Python3Parser.Compound_stmtContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}