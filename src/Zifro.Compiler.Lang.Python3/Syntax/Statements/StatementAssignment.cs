using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public class StatementAssignment : Statement
    {
        public StatementAssignment(SourceReference source, string sourceText)
            : base(source, sourceText)
        {
        }
    }
}