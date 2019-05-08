using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class BasicOperatorCodeTests
    {
        [DataTestMethod]
        [DataRow(BasicOperatorCode.AAdd, DisplayName = "is bin op a+b")]
        [DataRow(BasicOperatorCode.ASub, DisplayName = "is bin op a-b")]
        [DataRow(BasicOperatorCode.AMul, DisplayName = "is bin op a*b")]
        [DataRow(BasicOperatorCode.ADiv, DisplayName = "is bin op a/b")]
        [DataRow(BasicOperatorCode.AFlr, DisplayName = "is bin op a//b")]
        [DataRow(BasicOperatorCode.AMod, DisplayName = "is bin op a%b")]
        [DataRow(BasicOperatorCode.APow, DisplayName = "is bin op a**b")]
        [DataRow(BasicOperatorCode.BAnd, DisplayName = "is bin op a&b")]
        [DataRow(BasicOperatorCode.BLsh, DisplayName = "is bin op a<<b")]
        [DataRow(BasicOperatorCode.BRsh, DisplayName = "is bin op a>>b")]
        [DataRow(BasicOperatorCode.BOr, DisplayName = "is bin op a|b")]
        [DataRow(BasicOperatorCode.BXor, DisplayName = "is bin op a^b")]
        [DataRow(BasicOperatorCode.CEq, DisplayName = "is bin op a==b")]
        [DataRow(BasicOperatorCode.CNEq, DisplayName = "is bin op a!=b")]
        [DataRow(BasicOperatorCode.CGt, DisplayName = "is bin op a>b")]
        [DataRow(BasicOperatorCode.CGtEq, DisplayName = "is bin op a>=b")]
        [DataRow(BasicOperatorCode.CLt, DisplayName = "is bin op a<b")]
        [DataRow(BasicOperatorCode.CLtEq, DisplayName = "is bin op a<=b")]
        [DataRow(BasicOperatorCode.CIn, DisplayName = "is bin op a in b")]
        [DataRow(BasicOperatorCode.CNIn, DisplayName = "is bin op a not in b")]
        [DataRow(BasicOperatorCode.CIs, DisplayName = "is bin op a is b")]
        [DataRow(BasicOperatorCode.CIsN, DisplayName = "is bin op a is not b")]
        public void IsBinaryTests(BasicOperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsTrue(isBinary, $"{nameof(BasicOperatorCode)}.{code}.IsBinary() was false");
            Assert.IsFalse(isUnary, $"{nameof(BasicOperatorCode)}.{code}.IsUnary() was true");
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.ANeg, DisplayName = "is un op +a")]
        [DataRow(BasicOperatorCode.APos, DisplayName = "is un op -a")]
        [DataRow(BasicOperatorCode.BNot, DisplayName = "is un op ~a")]
        [DataRow(BasicOperatorCode.LNot, DisplayName = "is un op !a")]
        public void IsUnaryTests(BasicOperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsFalse(isBinary, $"{nameof(BasicOperatorCode)}.{code}.IsBinary() was true");
            Assert.IsTrue(isUnary, $"{nameof(BasicOperatorCode)}.{code}.IsUnary() was false");
        }
    }
}