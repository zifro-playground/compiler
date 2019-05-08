using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Tests.SyntaxConstructor.TestTree
{
    [TestClass]
    public class VisitAtomTests : BaseVisitTestClass<
        Python3Parser.AtomContext,
        Python3Parser.Testlist_compContext
    >
    {
        public override SyntaxNode VisitContext()
        {
            return ctor.VisitAtom(contextMock.Object);
        }

        public override void SetupForInnerMock(Mock<Python3Parser.Testlist_compContext> innerMock, SyntaxNode returnValue)
        {
            ctorMock.Setup(o => o.VisitTestlist_comp(innerMock.Object))
                .Returns(returnValue).Verifiable();
        }

        public override ITerminalNode GetTerminalForThisClass()
        {
            throw new NotSupportedException("Specify them explicitly instead. Too many terminal types in ATOM anyway.");
        }

        [DataTestMethod]
        [DataRow(Python3Parser.NUMBER, "420", typeof(LiteralInteger))]
        [DataRow(Python3Parser.NUMBER, "3.14", typeof(LiteralDouble))]
        [DataRow(Python3Parser.NUMBER, "0x10", typeof(LiteralInteger))] // hex
        [DataRow(Python3Parser.NUMBER, "1e10", typeof(LiteralDouble))] // sci-notation
        [DataRow(Python3Parser.NUMBER, "0o10", typeof(LiteralInteger))] // oct
        [DataRow(Python3Parser.NUMBER, "0b10", typeof(LiteralInteger))] // bin
        [DataRow(Python3Parser.STRING, "\"foo\"", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "'foo'", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "'foo\\'bar'", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "\"foo'bar\"", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "r'raw'", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "\"\"\"long str\"\"\"", typeof(LiteralString))]
        [DataRow(Python3Parser.TRUE, "True", typeof(LiteralBoolean))]
        [DataRow(Python3Parser.FALSE, "False", typeof(LiteralBoolean))]
        [DataRow(Python3Parser.NAME, "x", typeof(Identifier))]
        [DataRow(Python3Parser.NONE, "None", typeof(LiteralNone))]
        public void Visit_LiteralTypes_Test(int token, string text, Type expectedType)
        {
            // Arrange
            var node = GetTerminal(token, text);

            contextMock.SetupChildren(
                node
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, expectedType);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_LiteralStringsConcatenated_Test()
        {
            // Arrange
            var node1 = GetTerminal(Python3Parser.STRING, "\"foo\"");
            var node2 = GetTerminal(Python3Parser.STRING, "\"bar\"");

            contextMock.SetupChildren(
                node1,
                node2
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, node2);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.NUMBER, "1j")] // complex
        [DataRow(Python3Parser.STRING, "u'foo'")] // unicode
        [DataRow(Python3Parser.STRING, "b'foo'")] // bytes
        [DataRow(Python3Parser.STRING, "f'foo'")] // formatted
        [DataRow(Python3Parser.STRING, "fr'foo'")] // raw+formatted
        [DataRow(Python3Parser.STRING, "rf'foo'")] // raw+formatted
        [DataRow(Python3Parser.STRING, "br'foo'")] // raw+bytes
        [DataRow(Python3Parser.STRING, "rb'foo'")] // raw+bytes
        public void Visit_LiteralsNotYetImplemented_Test(int token, string text)
        {
            // Arrange
            var node = GetTerminal(token, text);

            contextMock.SetupChildren(
                node
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, node);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.ELLIPSIS, "...", "...")]
        public void Visit_LiteralsNotYetImplementedKeyword_Test(int token, string text, string expectedKeyword)
        {
            // Arrange
            var node = GetTerminal(token, text);

            contextMock.SetupChildren(
                node
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, node, expectedKeyword);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.OPEN_BRACK, Python3Parser.CLOSE_BRACK, "[]", DisplayName = "nyi []")]
        [DataRow(Python3Parser.OPEN_BRACE, Python3Parser.CLOSE_BRACE, "{}", DisplayName = "nyi {}")]
        public void Visit_GroupAllEmptyNotYetImplemented_Test(int openToken, int closeToken, string expectedKeyword)
        {
            // Arrange
            var open = GetTerminal(openToken);

            contextMock.SetupChildren(
                open,
                GetTerminal(closeToken)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext);

            Assert.That.ErrorNotYetImplFormatArgs(ex, open, expectedKeyword);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.OPEN_PAREN, ")", DisplayName = "no closing (")]
        [DataRow(Python3Parser.OPEN_BRACK, "]", DisplayName = "no closing [")]
        [DataRow(Python3Parser.OPEN_BRACE, "}", DisplayName = "no closing {")]
        public void Visit_GroupAllMissingClosing_Test(int openToken, string expectedEndingParenthesis)
        {
            // Arrange
            var open = GetTerminal(openToken);

            contextMock.SetupChildren(
                open
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorSyntaxFormatArgsEqual(ex, 
                nameof(Localized_Python3_Parser.Ex_Parenthesis_NoClosing),
                open, expectedEndingParenthesis);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_ParenthesesEmpty_Test()
        {
            // Arrange
            contextMock.SetupForSourceReference(startTokenMock, stopTokenMock);
            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

            Assert.That.ErrorExpectedChildFormatArgs(ex, startTokenMock, stopTokenMock, contextMock);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_ParenthesesTestListComp_Test()
        {
            // Arrange
            var expression = GetExpressionMock();
            var innerMock
                = GetInnerMockWithSetup(expression);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                innerMock.Object,
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act
            var result = VisitContext();

            // Assert
            Assert.AreSame(expression, result);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_ParenthesesYieldExpr_Test()
        {
            // Arrange
            var innerMock = GetMockRule<Python3Parser.Yield_exprContext>();
            innerMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                innerMock.Object,
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

            Assert.That.ErrorNotYetImplFormatArgs(ex, startTokenMock, stopTokenMock, "yield");

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_ParenthesesInvalidRule_Test()
        {
            // Arrange
            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                unexpectedMock.Object,
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            unexpectedMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_ParenthesesTooMany_Test()
        {
            // Arrange
            var innerMock = GetInnerMock();
            var unexpectedMock = GetMockRule<Python3Parser.File_inputContext>();
            unexpectedMock.SetupForSourceReference(startTokenMock, stopTokenMock);

            contextMock.SetupChildren(
                GetTerminal(Python3Parser.OPEN_PAREN),
                innerMock.Object,
                unexpectedMock.Object,
                GetTerminal(Python3Parser.CLOSE_PAREN)
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext,
                message: $"expected throw for context `{Python3Parser.ruleNames[contextMock.Object.RuleIndex]}`");

            Assert.That.ErrorUnexpectedChildTypeFormatArgs(ex, startTokenMock, stopTokenMock, contextMock, unexpectedMock.Object);

            innerMock.Verify();
            contextMock.Verify();
            ctorMock.Verify();
        }
    }
}