using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Resources;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;

namespace Mellis.Lang.Python3.Tests.Syntax.Literals
{
    public abstract class BaseLiteralTests<TLiteral, TValue>
        where TLiteral : Literal<TValue>
    {
        protected abstract TLiteral Parse(SourceReference source, string text);

        public virtual void ParseValidTest(string input, TValue expectedValue)
        {
            // Arrange
            var source = SourceReference.ClrSource;

            // Act
            TLiteral result = Parse(source, input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedValue, result.Value);
        }

        public virtual void ParseInvalidTest(string input)
        {
            // Arrange
            var source = SourceReference.ClrSource;

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxLiteralFormatException>(() => Parse(source, input));

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Python3_Parser.Ex_Literal_Format),
                source);
        }

        public virtual void ParseNotYetImplementedTest(string input)
        {
            // Arrange
            var source = SourceReference.ClrSource;

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>(() => Parse(source, input));

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                nameof(Localized_Exceptions.Ex_Syntax_NotYetImplemented),
                source);
        }
    }
}