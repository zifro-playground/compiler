using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Statements;

namespace Mellis.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor : Python3BaseVisitor<SyntaxNode>
    {
        public override SyntaxNode VisitSingle_input(Python3Parser.Single_inputContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitFile_input(Python3Parser.File_inputContext context)
        {
            // file_input: (NEWLINE | stmt)* EOF;
            var statements = new List<Statement>();

            // This ignores newlines and eof
            // and flattens nested StatementLists
            foreach (Python3Parser.StmtContext child in context.GetChildren().OfType<Python3Parser.StmtContext>())
            {
                SyntaxNode result = VisitStmt(child);
                if (result is StatementList list)
                {
                    statements.AddRange(list.Statements);
                }
                else
                {
                    statements.Add((Statement) result);
                }
            }

            return statements.Count == 1
                ? statements[0]
                : new StatementList(context.GetSourceReference(), statements);
        }

        public override SyntaxNode VisitEval_input(Python3Parser.Eval_inputContext context)
        {
            throw context.NotYetImplementedException();
        }
    }
}