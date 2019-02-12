using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;

namespace Zifro.Compiler.Lang.Python3.Tests.Syntax.Literals
{
    [TestClass]
    public class LiteralStringTests : BaseLiteralTests<LiteralString, string>
    {
        protected override LiteralString Parse(SourceReference source, string text)
        {
            return LiteralString.Parse(source, text);
        }

        [DataTestMethod]
        [DataRow("\"foo\"", "foo")]
        [DataRow("'foo'", "foo")]
        [DataRow("'new\\nline'", "new\nline")]
        [DataRow("'foo\\'bar'", "foo'bar")]
        [DataRow("\"foo'bar\"", "foo'bar")]
        [DataRow("r'foo'", "foo")] // raw
        [DataRow("r'new\\nline'", "new\\nline")]
        [DataRow("\"\"\"foo\"\"\"", "foo")] // triple quoted
        [DataRow("'''foo'''", "foo")]
        [DataRow("r'''foo\\bbar'''", "foo\\bbar")]
        [DataRow("''", "")]
        public override void ParseValidTest(string input, string expectedValue)
        {
            base.ParseValidTest(input, expectedValue);
        }

        [DataTestMethod]
        [DataRow("1e5")]
        [DataRow("ru'raw unicode'")]
        [DataRow("fb'formatted bytes'")]
        [DataRow("frb'formatted raw bytes'")]
        //[DataRow("\"unescaped\"quote\"")]
        //[DataRow("'unescaped'quote'")]
        [DataRow("")]
        [DataRow("r")]
        [DataRow("'''")]
        [DataRow("'\"")]
        public override void ParseInvalidTest(string input)
        {
            base.ParseInvalidTest(input);
        }

        [DataTestMethod]
        [DataRow("u'foo'")] // unicode
        [DataRow("b'foo'")] // bytes
        [DataRow("f'foo'")] // formatted
        [DataRow("fr'foo'")] // raw+formatted
        [DataRow("rf'foo'")] // raw+formatted
        [DataRow("br'foo'")] // raw+bytes
        [DataRow("rb'foo'")] // raw+bytes
        public override void ParseNotYetImplementedTest(string input)
        {
            base.ParseNotYetImplementedTest(input);
        }

        [DataTestMethod]
        [DataRow("hello world", "hello world")]
        [DataRow("new\\nline", "new\nline")]
        [DataRow("unknown escape \\p", "unknown escape \\p")]
        [DataRow("\\\\n", "\\n")]
        [DataRow("\"\'\\\"\\n\\f", "\"'\"\n\f")]
        [DataRow("\\x42", "\x42")] // hexadecimal
        [DataRow("\\X42", "\\X42")] // hexadecimal
        [DataRow("\\123", "S")] // octal
        [DataRow("\\1", "\x1")] // octal
        [DataRow("\\1\\123", "\x1S")] // octal
        public void UnescapeTest(string input, string expected)
        {
            // Act
            string result = LiteralString.Unescape(input);

            // Assert
            Assert.AreEqual(expected, result, $"\nEscaped expected:<{Regex.Escape(expected)}>\nEscaped actual:<{Regex.Escape(result)}>");
        }
    }
}