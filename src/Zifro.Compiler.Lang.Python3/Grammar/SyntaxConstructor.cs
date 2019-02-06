using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Statements;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class Python3Parser
    {
    }

    public partial class SyntaxConstructor : Python3BaseVisitor<SyntaxNode>
    {
        public override SyntaxNode VisitSingle_input(Python3Parser.Single_inputContext context)
        {
            throw new System.NotImplementedException();
        }

        public override SyntaxNode VisitFile_input(Python3Parser.File_inputContext context)
        {
            // file_input: (NEWLINE | stmt)* EOF;
            Statement[] children = context.GetChildren()
                .Cast<Python3Parser.StmtContext>()
                .Select(VisitStmt)
                .Cast<Statement>()
                .ToArray();
            
            return new StatementList(context.GetSourceReference(), children);
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