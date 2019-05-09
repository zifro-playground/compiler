using System;
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
        [DataRow(BasicOperatorCode.IAAdd, DisplayName = "is bin op a+=b")]
        [DataRow(BasicOperatorCode.IASub, DisplayName = "is bin op a-=b")]
        [DataRow(BasicOperatorCode.IAMul, DisplayName = "is bin op a*=b")]
        [DataRow(BasicOperatorCode.IADiv, DisplayName = "is bin op a/=b")]
        [DataRow(BasicOperatorCode.IAFlr, DisplayName = "is bin op a//=b")]
        [DataRow(BasicOperatorCode.IAMod, DisplayName = "is bin op a%=b")]
        [DataRow(BasicOperatorCode.IAPow, DisplayName = "is bin op a**=b")]
        [DataRow(BasicOperatorCode.IBAnd, DisplayName = "is bin op a&=b")]
        [DataRow(BasicOperatorCode.IBLsh, DisplayName = "is bin op a<<=b")]
        [DataRow(BasicOperatorCode.IBRsh, DisplayName = "is bin op a>>=b")]
        [DataRow(BasicOperatorCode.IBOr, DisplayName = "is bin op a|=b")]
        [DataRow(BasicOperatorCode.IBXor, DisplayName = "is bin op a^=b")]
        public void IsBinaryTests(BasicOperatorCode code)
        {
            // Act
            bool isBinary = code.IsBinary();
            bool isUnary = code.IsUnary();

            // Assert
            Assert.IsTrue(isBinary, "{0}.{1}.IsBinary() was false", nameof(BasicOperatorCode), code);
            Assert.IsFalse(isUnary, "{0}.{1}.IsUnary() was true", nameof(BasicOperatorCode), code);
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
            Assert.IsFalse(isBinary, "{0}.{1}.IsBinary() was true", nameof(BasicOperatorCode), code);
            Assert.IsTrue(isUnary, "{0}.{1}.IsUnary() was false", nameof(BasicOperatorCode), code);
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.IAAdd, DisplayName = "is iadd op a+=b")]
        [DataRow(BasicOperatorCode.IASub, DisplayName = "is isub op a-=b")]
        [DataRow(BasicOperatorCode.IAMul, DisplayName = "is imult op a*=b")]
        [DataRow(BasicOperatorCode.IADiv, DisplayName = "is idiv op a/=b")]
        [DataRow(BasicOperatorCode.IAFlr, DisplayName = "is iflr op a//=b")]
        [DataRow(BasicOperatorCode.IAMod, DisplayName = "is imod op a%=b")]
        [DataRow(BasicOperatorCode.IAPow, DisplayName = "is ipow op a**=b")]
        [DataRow(BasicOperatorCode.IAMat, DisplayName = "is imat op a@=b")]
        [DataRow(BasicOperatorCode.IBAnd, DisplayName = "is iband op a&=b")]
        [DataRow(BasicOperatorCode.IBLsh, DisplayName = "is iblsh op a<<=b")]
        [DataRow(BasicOperatorCode.IBRsh, DisplayName = "is ibrsh op a>>=b")]
        [DataRow(BasicOperatorCode.IBOr, DisplayName = "is ibor op a|=b")]
        [DataRow(BasicOperatorCode.IBXor, DisplayName = "is ibxor op a^=b")]
        public void IsInPlaceTests(BasicOperatorCode code)
        {
            // Act
            bool isInPlace = code.IsInPlace();

            // Assert
            Assert.IsTrue(isInPlace, "{0}.{1}.IsInPlace() was false", nameof(BasicOperatorCode), code);
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.AAdd, DisplayName = "is not in-place op a+b")]
        [DataRow(BasicOperatorCode.ASub, DisplayName = "is not in-place op a-b")]
        [DataRow(BasicOperatorCode.AMul, DisplayName = "is not in-place op a*b")]
        [DataRow(BasicOperatorCode.ADiv, DisplayName = "is not in-place op a/b")]
        [DataRow(BasicOperatorCode.AFlr, DisplayName = "is not in-place op a//b")]
        [DataRow(BasicOperatorCode.AMod, DisplayName = "is not in-place op a%b")]
        [DataRow(BasicOperatorCode.APow, DisplayName = "is not in-place op a**b")]
        [DataRow(BasicOperatorCode.BAnd, DisplayName = "is not in-place op a&b")]
        [DataRow(BasicOperatorCode.BLsh, DisplayName = "is not in-place op a<<b")]
        [DataRow(BasicOperatorCode.BRsh, DisplayName = "is not in-place op a>>b")]
        [DataRow(BasicOperatorCode.BOr, DisplayName = "is not in-place op a|b")]
        [DataRow(BasicOperatorCode.BXor, DisplayName = "is not in-place op a^b")]
        [DataRow(BasicOperatorCode.CEq, DisplayName = "is not in-place op a==b")]
        [DataRow(BasicOperatorCode.CNEq, DisplayName = "is not in-place op a!=b")]
        [DataRow(BasicOperatorCode.CGt, DisplayName = "is not in-place op a>b")]
        [DataRow(BasicOperatorCode.CGtEq, DisplayName = "is not in-place op a>=b")]
        [DataRow(BasicOperatorCode.CLt, DisplayName = "is not in-place op a<b")]
        [DataRow(BasicOperatorCode.CLtEq, DisplayName = "is not in-place op a<=b")]
        [DataRow(BasicOperatorCode.CIn, DisplayName = "is not in-place op a in b")]
        [DataRow(BasicOperatorCode.CNIn, DisplayName = "is not in-place op a not in b")]
        [DataRow(BasicOperatorCode.CIs, DisplayName = "is not in-place op a is b")]
        [DataRow(BasicOperatorCode.CIsN, DisplayName = "is not in-place op a is not b")]
        [DataRow(BasicOperatorCode.ANeg, DisplayName = "is not in-place op +a")]
        [DataRow(BasicOperatorCode.APos, DisplayName = "is not in-place op -a")]
        [DataRow(BasicOperatorCode.BNot, DisplayName = "is not in-place op ~a")]
        [DataRow(BasicOperatorCode.LNot, DisplayName = "is not in-place op !a")]
        public void IsNotInPlaceTests(BasicOperatorCode code)
        {
            // Act
            bool isInPlace = code.IsInPlace();

            // Assert
            Assert.IsFalse(isInPlace, "{0}.{1}.IsInPlace() was true", nameof(BasicOperatorCode), code);
        }
    }
}