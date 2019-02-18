using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    public abstract class BaseVisitTestClass<TContext, TInnerContext> 
        : BaseVisitClass<TContext> 
        where TContext : ParserRuleContext
        where TInnerContext : ParserRuleContext
    {
        public virtual Mock<TInnerContext> GetInnerMock()
        {
            return GetMockRule<TInnerContext>();
        }

        public virtual Mock<TInnerContext> GetInnerMockWithSetup(SyntaxNode returnValue)
        {
            Mock<TInnerContext> mock = GetMockRule<TInnerContext>();
            SetupForInnerMock(mock, returnValue);
            return mock;
        }

        public abstract void SetupForInnerMock(
            Mock<TInnerContext> innerMock,
            SyntaxNode returnValue);

        public abstract ITerminalNode GetTerminalForThisClass();
    }
}