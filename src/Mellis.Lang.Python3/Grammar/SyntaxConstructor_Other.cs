using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitDecorator(Python3Parser.DecoratorContext context)
        {
            throw context.NotYetImplementedException("@");
        }

        public override SyntaxNode VisitDecorators(Python3Parser.DecoratorsContext context)
        {
            throw context.NotYetImplementedException("@");
        }

        public override SyntaxNode VisitDecorated(Python3Parser.DecoratedContext context)
        {
            throw context.NotYetImplementedException("@");
        }

        public override SyntaxNode VisitAsync_funcdef(Python3Parser.Async_funcdefContext context)
        {
            throw context.NotYetImplementedException("async def");
        }

        public override SyntaxNode VisitFuncdef(Python3Parser.FuncdefContext context)
        {
            throw context.NotYetImplementedException("def");
        }

        public override SyntaxNode VisitParameters(Python3Parser.ParametersContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTypedargslist(Python3Parser.TypedargslistContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitTfpdef(Python3Parser.TfpdefContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitVarargslist(Python3Parser.VarargslistContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitVfpdef(Python3Parser.VfpdefContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitDel_stmt(Python3Parser.Del_stmtContext context)
        {
            throw context.NotYetImplementedException("del");
        }

        public override SyntaxNode VisitPass_stmt(Python3Parser.Pass_stmtContext context)
        {
            throw context.NotYetImplementedException("pass");
        }

        public override SyntaxNode VisitFlow_stmt(Python3Parser.Flow_stmtContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitBreak_stmt(Python3Parser.Break_stmtContext context)
        {
            throw context.NotYetImplementedException("break");
        }

        public override SyntaxNode VisitContinue_stmt(Python3Parser.Continue_stmtContext context)
        {
            throw context.NotYetImplementedException("continue");
        }

        public override SyntaxNode VisitReturn_stmt(Python3Parser.Return_stmtContext context)
        {
            throw context.NotYetImplementedException("return");
        }

        public override SyntaxNode VisitYield_stmt(Python3Parser.Yield_stmtContext context)
        {
            throw context.NotYetImplementedException("yield");
        }

        public override SyntaxNode VisitRaise_stmt(Python3Parser.Raise_stmtContext context)
        {
            throw context.NotYetImplementedException("raise");
        }

        public override SyntaxNode VisitImport_stmt(Python3Parser.Import_stmtContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException("import");
        }

        public override SyntaxNode VisitImport_name(Python3Parser.Import_nameContext context)
        {
            throw context.NotYetImplementedException("import");
        }

        public override SyntaxNode VisitImport_from(Python3Parser.Import_fromContext context)
        {
            throw context.NotYetImplementedException("from");
        }

        public override SyntaxNode VisitImport_as_name(Python3Parser.Import_as_nameContext context)
        {
            throw context.NotYetImplementedException("as");
        }

        public override SyntaxNode VisitDotted_as_name(Python3Parser.Dotted_as_nameContext context)
        {
            throw context.NotYetImplementedException("as");
        }

        public override SyntaxNode VisitImport_as_names(Python3Parser.Import_as_namesContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitDotted_as_names(Python3Parser.Dotted_as_namesContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitDotted_name(Python3Parser.Dotted_nameContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitGlobal_stmt(Python3Parser.Global_stmtContext context)
        {
            throw context.NotYetImplementedException("global");
        }

        public override SyntaxNode VisitNonlocal_stmt(Python3Parser.Nonlocal_stmtContext context)
        {
            throw context.NotYetImplementedException("nonlocal");
        }

        public override SyntaxNode VisitWith_item(Python3Parser.With_itemContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitExcept_clause(Python3Parser.Except_clauseContext context)
        {
            throw context.NotYetImplementedException("except");
        }

        public override SyntaxNode VisitTest_nocond(Python3Parser.Test_nocondContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitLambdef(Python3Parser.LambdefContext context)
        {
            throw context.NotYetImplementedException("lambda");
        }

        public override SyntaxNode VisitLambdef_nocond(Python3Parser.Lambdef_nocondContext context)
        {
            throw context.NotYetImplementedException("lambda");
        }

        public override SyntaxNode VisitClassdef(Python3Parser.ClassdefContext context)
        {
            throw context.NotYetImplementedException("class");
        }

        public override SyntaxNode VisitComp_iter(Python3Parser.Comp_iterContext context)
        {
            VisitChildren(context);
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitComp_for(Python3Parser.Comp_forContext context)
        {
            throw context.NotYetImplementedException("for");
        }

        public override SyntaxNode VisitComp_if(Python3Parser.Comp_ifContext context)
        {
            throw context.NotYetImplementedException("if");
        }

        public override SyntaxNode VisitEncoding_decl(Python3Parser.Encoding_declContext context)
        {
            throw context.NotYetImplementedException();
        }

        public override SyntaxNode VisitYield_expr(Python3Parser.Yield_exprContext context)
        {
            throw context.NotYetImplementedException("yield");
        }

        public override SyntaxNode VisitYield_arg(Python3Parser.Yield_argContext context)
        {
            throw context.NotYetImplementedException("from");
        }
    }
}