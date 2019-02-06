using System.Collections.Generic;
using System.Linq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public class StatementList : SyntaxNode
    {
        public IReadOnlyList<Statement> Statements { get; set; }

        public StatementList(IReadOnlyList<Statement> statements)
            : base(SourceReference.Merge(statements.Select(s => s.Source)), null)
        {
            Statements = statements;
        }
    }
}