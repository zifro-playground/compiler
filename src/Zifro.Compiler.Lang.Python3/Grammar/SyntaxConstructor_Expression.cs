using Antlr4.Runtime.Tree;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Extensions;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public partial class SyntaxConstructor
    {
        public override SyntaxNode VisitExpr_stmt(Python3Parser.Expr_stmtContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitTestlist_star_expr(Python3Parser.Testlist_star_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitAnnassign(Python3Parser.AnnassignContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitAugassign(Python3Parser.AugassignContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitTest(Python3Parser.TestContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitOr_test(Python3Parser.Or_testContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitAnd_test(Python3Parser.And_testContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitNot_test(Python3Parser.Not_testContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitComparison(Python3Parser.ComparisonContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitComp_op(Python3Parser.Comp_opContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitStar_expr(Python3Parser.Star_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitExpr(Python3Parser.ExprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitXor_expr(Python3Parser.Xor_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitAnd_expr(Python3Parser.And_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitShift_expr(Python3Parser.Shift_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitArith_expr(Python3Parser.Arith_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitTerm(Python3Parser.TermContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitFactor(Python3Parser.FactorContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitPower(Python3Parser.PowerContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitAtom_expr(Python3Parser.Atom_exprContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitAtom(Python3Parser.AtomContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitTestlist_comp(Python3Parser.Testlist_compContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitTrailer(Python3Parser.TrailerContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitSubscriptlist(Python3Parser.SubscriptlistContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitSubscript(Python3Parser.SubscriptContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitSliceop(Python3Parser.SliceopContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitExprlist(Python3Parser.ExprlistContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitTestlist(Python3Parser.TestlistContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }

        public override SyntaxNode VisitDictorsetmaker(Python3Parser.DictorsetmakerContext context)
        {
            throw new SyntaxNotYetImplementedException(context.GetSourceReference());
        }
    }
}