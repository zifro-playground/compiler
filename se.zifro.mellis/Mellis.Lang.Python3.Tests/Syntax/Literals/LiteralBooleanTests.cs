using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Tests.Syntax.Literals
{
    [TestClass]
    public class LiteralBooleanTests : BaseLiteralTests<LiteralBoolean, bool>
    {
        protected override LiteralBoolean Parse(SourceReference source, string text)
        {
            return LiteralBoolean.Parse(source, text);
        }

        [DataTestMethod]
        [DataRow("True", true)]
        [DataRow("False", false)]
        public override void ParseValidTest(string input, bool expectedValue)
        {
            base.ParseValidTest(input, expectedValue);
        }

        [DataTestMethod]
        [DataRow("true")]
        [DataRow("false")]
        [DataRow("1")]
        [DataRow("TRUE")]
        [DataRow("yes")]
        public override void ParseInvalidTest(string input)
        {
            base.ParseInvalidTest(input);
        }
    }
}