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
        public override void ParseValidTest(string input, string expectedValue)
        {
            base.ParseValidTest(input, expectedValue);
        }

        [DataTestMethod]
        [DataRow("1e5")]
        [DataRow("ru'raw unicode'")]
        [DataRow("fb'formatted bytes'")]
        [DataRow("frb'formatted raw bytes'")]
        [DataRow("\"unescaped\"quote\"")]
        [DataRow("'unescaped'quote'")]
        public override void ParseInvalidTest(string input)
        {
            base.ParseInvalidTest(input);
        }

        [DataTestMethod]
        [DataRow("\"\"\"foo\"\"\"")]
        [DataRow("'''foo'''")]
        [DataRow("r'foo'")] // raw
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
    }
}