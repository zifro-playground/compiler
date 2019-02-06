using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Grammar
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

            foreach (Python3Parser.StmtContext child in context.GetChildren().Cast<Python3Parser.StmtContext>())
            {
                SyntaxNode result = VisitStmt(child);
                if (result is StatementList list)
                    statements.AddRange(list.Statements);
                else
                    statements.Add((Statement) result);
            }
            
            return new StatementList(context.GetSourceReference(), statements);
        }

        public override SyntaxNode VisitEval_input(Python3Parser.Eval_inputContext context)
        {
            throw context.NotYetImplementedException();
        }

        private static SourceReference SourceReference(ParserRuleContext context)
        {
            return new SourceReference(context.Start.Line, context.Stop.Line, context.Start.Column, context.Stop.Column);
        }
    }
}