using System.Collections.Generic;
using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax
{
    public class StatementList : Statement
    {
        public IReadOnlyList<Statement> Statements { get; set; }

        public StatementList(SourceReference source, IReadOnlyList<Statement> statements)
            : base(source)
        {
            Statements = statements;
        }

        public override void Compile(PyCompiler compiler)
        {
            foreach (Statement statement in Statements)
            {
                statement.Compile(compiler);
            }
        }
    }
}