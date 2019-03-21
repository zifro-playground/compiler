using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;
using Moq;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.CompoundTree
{
    //public class VisitForStmtTests : BaseVisitClass<Python3Parser.For_stmtContext>
    //{
    //    public override SyntaxNode VisitContext()
    //    {
    //        return ctor.VisitFor_stmt(contextMock.Object);
    //    }

    //    private void CreateAndSetupTestList(out Mock<Python3Parser.TestlistContext> testMock, out ExpressionNode testExpr)
    //    {
    //        testMock = GetMockRule<Python3Parser.TestlistContext>();
    //        var refCopy = testMock;
    //        testExpr = ctorMock.SetupExpressionMock(o => o.VisitTest(refCopy.Object));
    //    }

    //    private void CreateAndSetupSuite(out Mock<Python3Parser.SuiteContext> suiteMock, out Statement suiteStmt)
    //    {
    //        suiteMock = GetMockRule<Python3Parser.SuiteContext>();
    //        var refCopy = suiteMock;
    //        suiteStmt = ctorMock.SetupStatementMock(o => o.VisitSuite(refCopy.Object));
    //    }
    //}
}