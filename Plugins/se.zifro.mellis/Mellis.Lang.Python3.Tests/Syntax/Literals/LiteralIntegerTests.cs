using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Tests.Syntax.Literals
{
    [TestClass]
    public class LiteralIntegerTests : BaseLiteralTests<LiteralInteger, int>
    {
        protected override LiteralInteger Parse(SourceReference source, string text)
        {
            return LiteralInteger.Parse(source, text);
        }

        [DataTestMethod]
        [DataRow("0", 0)]
        [DataRow("10", 10)]
        [DataRow("0x10", 0x10)]
        public override void ParseValidTest(string input, int expectedValue)
        {
            base.ParseValidTest(input, expectedValue);
        }

        [DataTestMethod]
        [DataRow("0x-1")]
        [DataRow("1_0")]
        [DataRow("1.0")]
        [DataRow("1e5")]
        public override void ParseInvalidTest(string input)
        {
            base.ParseInvalidTest(input);
        }

        [DataTestMethod]
        [DataRow("0b1")]
        [DataRow("0o1")]
        public override void ParseNotYetImplementedTest(string input)
        {
            base.ParseNotYetImplementedTest(input);
        }
    }
}