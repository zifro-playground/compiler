using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Tests.Syntax.Literals
{
    [TestClass]
    public class LiteralDoubleTests : BaseLiteralTests<LiteralDouble, double>
    {
        protected override LiteralDouble Parse(SourceReference source, string text)
        {
            return LiteralDouble.Parse(source, text);
        }

        [DataTestMethod]
        [DataRow("0", 0d)]
        [DataRow("10", 10d)]
        [DataRow("0.1", 0.1d)]
        [DataRow("1e5", 1e5d)]
        [DataRow("1e+5", 1e5d)]
        [DataRow("1e-5", 1e-5d)]
        public override void ParseValidTest(string input, double expectedValue)
        {
            base.ParseValidTest(input, expectedValue);
        }

        [DataTestMethod]
        [DataRow("0x-1")]
        [DataRow("1_0")]
        [DataRow("1e0.1")]
        [DataRow("0x10")]
        [DataRow("0b1")]
        [DataRow("0o1")]
        public override void ParseInvalidTest(string input)
        {
            base.ParseInvalidTest(input);
        }
    }
}