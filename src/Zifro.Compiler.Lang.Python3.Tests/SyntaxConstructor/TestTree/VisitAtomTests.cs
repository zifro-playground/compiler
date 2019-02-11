using System;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;

namespace Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor.TestTree
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
        [DataRow(Python3Parser.STRING, "\"foo\"", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "'foo'", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "'foo\\'bar'", typeof(LiteralString))]
        [DataRow(Python3Parser.STRING, "\"foo'bar\"", typeof(LiteralString))]
        [DataRow(Python3Parser.TRUE, "True", typeof(LiteralBoolean))]
        [DataRow(Python3Parser.FALSE, "False", typeof(LiteralBoolean))]
        public void Visit_LiteralTypes_Test(int token, string text, Type expectedType)
        {
            // Arrange
            ITerminalNode node = GetTerminal(token, text);

            contextMock.SetupChildren(
                node
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, expectedType);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [TestMethod]
        public void Visit_LiteralStringsConcatenated_Test()
        {
            // Arrange
            ITerminalNode node1 = GetTerminal(Python3Parser.STRING, "\"foo\"");
            ITerminalNode node2 = GetTerminal(Python3Parser.STRING, "\"bar\"");
            const string expected = "foobar";

            contextMock.SetupChildren(
                node1,
                node2
            );

            // Act
            SyntaxNode result = VisitContext();

            // Assert
            Assert.IsInstanceOfType(result, typeof(LiteralString));
            var literal = (LiteralString) result;
            Assert.AreEqual(expected, literal.Value);
            
            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.NUMBER, "0o10")] // oct
        [DataRow(Python3Parser.NUMBER, "0x10")] // hex
        [DataRow(Python3Parser.NUMBER, "0b10")] // bin
        [DataRow(Python3Parser.NUMBER, "1e10")] // sci-notation
        [DataRow(Python3Parser.NUMBER, "1j")] // complex
        [DataRow(Python3Parser.STRING, "r'foo'")] // raw
        [DataRow(Python3Parser.STRING, "u'foo'")] // unicode
        [DataRow(Python3Parser.STRING, "b'foo'")] // bytes
        [DataRow(Python3Parser.STRING, "f'foo'")] // formatted
        [DataRow(Python3Parser.STRING, "fr'foo'")] // raw+formatted
        [DataRow(Python3Parser.STRING, "rf'foo'")] // raw+formatted
        [DataRow(Python3Parser.STRING, "br'foo'")] // raw+bytes
        [DataRow(Python3Parser.STRING, "rb'foo'")] // raw+bytes
        [DataRow(Python3Parser.STRING, "\"\"\"long str\"\"\"")]
        [DataRow(Python3Parser.NONE, "None")]
        [DataRow(Python3Parser.NAME, "x")]
        [DataRow(Python3Parser.ELLIPSIS, "...")]
        public void Visit_LiteralsNotYetImplemented_Test(int token, string text)
        {
            // Arrange
            ITerminalNode node = GetTerminal(token, text);

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
        [DataRow(Python3Parser.NUMBER, "1b01")]
        [DataRow(Python3Parser.NUMBER, "1i")]
        [DataRow(Python3Parser.STRING, "ru'foo'")]
        [DataRow(Python3Parser.STRING, "\"x'")]
        [DataRow(Python3Parser.NONE, "Nope")]
        [DataRow(Python3Parser.NAME, "5")]
        [DataRow(Python3Parser.ELLIPSIS, ";_;")]
        public void Visit_LiteralsInvalidText_Test(int token, string text)
        {
            // Arrange
            ITerminalNode node = GetTerminal(token, text);

            contextMock.SetupChildren(
                node
            );

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxException>(VisitContext);

            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Literal_Format),
                node);

            contextMock.Verify();
            ctorMock.Verify();
        }

        [DataTestMethod]
        [DataRow(Python3Parser.OPEN_PAREN, Python3Parser.CLOSE_PAREN, "()", DisplayName = "nyi ()")]
        [DataRow(Python3Parser.OPEN_BRACK, Python3Parser.CLOSE_BRACK, "[]", DisplayName = "nyi []")]
        [DataRow(Python3Parser.OPEN_BRACE, Python3Parser.CLOSE_BRACE, "{}", DisplayName = "nyi {}")]
        public void Visit_GroupAllEmptyNotYetImplemented_Test(int openToken, int closeToken, string expectedKeyword)
        {
            // Arrange
            ITerminalNode open = GetTerminal(openToken);

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
            ITerminalNode open = GetTerminal(openToken);

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
    }
}