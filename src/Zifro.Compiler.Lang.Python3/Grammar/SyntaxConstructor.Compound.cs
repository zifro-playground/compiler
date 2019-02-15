using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitAssert_stmt(Python3Parser.Assert_stmtContext context)
        {
            throw context.NotYetImplementedException("assert");
        }

        public override SyntaxNode VisitAsync_stmt(Python3Parser.Async_stmtContext context)
        {
            throw context.NotYetImplementedException("async");
        }

        public override SyntaxNode VisitIf_stmt(Python3Parser.If_stmtContext context)
        {
            // if_stmt: 'if' test ':' suite
            //      ('elif' test ':' suite)*
            //      ['else' ':' suite]
            context.GetChildOrThrow(0, Python3Parser.IF);
            var testRule = context.GetChildOrThrow<Python3Parser.TestContext>(1);
            context.GetChildOrThrow(2, Python3Parser.COLON)
                .ThrowIfMissing(nameof(Localized_Python3_Parser.Ex_Syntax_If_MissingColon));
            var suiteRule = context.GetChildOrThrow<Python3Parser.SuiteContext>(3);



            throw context.NotYetImplementedException("if");
        }

        public override SyntaxNode VisitWhile_stmt(Python3Parser.While_stmtContext context)
        {
            throw context.NotYetImplementedException("while");
        }

        public override SyntaxNode VisitFor_stmt(Python3Parser.For_stmtContext context)
        {
            throw context.NotYetImplementedException("for");
        }

        public override SyntaxNode VisitTry_stmt(Python3Parser.Try_stmtContext context)
        {
            throw context.NotYetImplementedException("try");
        }

        public override SyntaxNode VisitWith_stmt(Python3Parser.With_stmtContext context)
        {
            throw context.NotYetImplementedException("with");
        }
    }
}