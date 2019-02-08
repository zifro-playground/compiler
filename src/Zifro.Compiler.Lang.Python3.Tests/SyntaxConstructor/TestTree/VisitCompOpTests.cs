using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitCompOpTests : BaseVisitClass<Python3Parser.Comp_opContext>
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitComp_op(contextMock.Object);
        }

        [TestMethod]
        public void Visit_NoChildren_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren();

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_TooManyChildren_Test()
        {
            // Arrange
            ITerminalNode excess = GetTerminal(Python3Parser.NOT);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.NOT),
                GetTerminal(Python3Parser.IN),
                excess
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, excess);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_InvalidRule_Test()
        {
            // Arrange
            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                unexpectedMock.Object
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            unexpectedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.LESS_THAN, ComparisonType.LessThan, DisplayName = "valid <")]
        [DataRow(Python3Parser.GREATER_THAN, ComparisonType.GreaterThan, DisplayName = "valid >")]
        [DataRow(Python3Parser.EQUALS, ComparisonType.Equals, DisplayName = "valid ==")]
        [DataRow(Python3Parser.GT_EQ, ComparisonType.GreaterThanOrEqual, DisplayName = "valid >=")]
        [DataRow(Python3Parser.LT_EQ, ComparisonType.LessThanOrEqual, DisplayName = "valid <=")]
        [DataRow(Python3Parser.NOT_EQ_1, ComparisonType.NotEqualsABC, DisplayName = "valid <>")]
        [DataRow(Python3Parser.NOT_EQ_2, ComparisonType.NotEquals, DisplayName = "valid !=")]
        [DataRow(Python3Parser.IN, ComparisonType.In, DisplayName = "valid in")]
        [DataRow(Python3Parser.IS, ComparisonType.Is, DisplayName = "valid is")]
        public void Visit_ValidSingleToken_Tests(int token, ComparisonType type)
        {
            // Arrange
            ITerminalNode term = GetTerminal(token);
            contextMock.SetupChildren(
                term
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ComparisonFactory));
            var factory = (ComparisonFactory) result;
            Assert.AreEqual(type, factory.Type);
            Assert.AreEqual(term.Symbol.Text, factory.Keyword);

            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
            ctorMock.Verify(o => o.VisitComp_op(contextMock.Object));
            ctorMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.NOT, Python3Parser.IN, ComparisonType.InNot, DisplayName = "valid not in")]
        [DataRow(Python3Parser.IS, Python3Parser.NOT, ComparisonType.IsNot, DisplayName = "valid is not")]
        public void Visit_ValidDoubleTokens_Tests(int token1, int token2, ComparisonType type)
        {
            // Arrange
            ITerminalNode term1 = GetTerminal(token1);
            ITerminalNode term2 = GetTerminal(token2);
            string expectedKeyword = $"{term1.Symbol.Text} {term2.Symbol.Text}";

            contextMock.SetupChildren(
                term1, term2
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ComparisonFactory));
            var factory = (ComparisonFactory) result;
            Assert.AreEqual(type, factory.Type);
            Assert.AreEqual(expectedKeyword, factory.Keyword);

            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
            ctorMock.Verify(o => o.VisitComp_op(contextMock.Object));
            ctorMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.ASYNC, Python3Parser.ARROW, DisplayName = "invalid async ->")]
        [DataRow(Python3Parser.AS, Python3Parser.IN, DisplayName = "invalid as in")]
        public void Visit_InvalidDoubleFirstToken_Tests(int token1, int token2)
        {
            // Arrange
            ITerminalNode first = GetTerminal(token1);
            contextMock.SetupChildren(
                first, GetTerminal(token2)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, first);
            contextMock.VerifyLoopedChildren(1);

            contextMock.Verify();
            ctorMock.Verify();
            ctorMock.Verify(o => o.VisitComp_op(contextMock.Object));
            ctorMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.IN, Python3Parser.NOT, DisplayName = "invalid in not")]
        [DataRow(Python3Parser.NOT, Python3Parser.IS, DisplayName = "invalid not is")]
        [DataRow(Python3Parser.EQUALS, Python3Parser.EQUALS, DisplayName = "invalid == ==")]
        [DataRow(Python3Parser.IS, Python3Parser.IN, DisplayName = "invalid is in")]
        [DataRow(Python3Parser.NOT, Python3Parser.EQUALS, DisplayName = "invalid not ==")]
        [DataRow(Python3Parser.NOT, Python3Parser.GREATER_THAN, DisplayName = "invalid not >")]
        public void Visit_InvalidDoubleSecondToken_Tests(int token1, int token2)
        {
            // Arrange
            ITerminalNode second = GetTerminal(token2);
            contextMock.SetupChildren(
                GetTerminal(token1), second
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, contextMock, second);
            contextMock.VerifyLoopedChildren(2);

            contextMock.Verify();
            ctorMock.Verify();
            ctorMock.Verify(o => o.VisitComp_op(contextMock.Object));
            ctorMock.VerifyNoOtherCalls();
        }
    }
}