using System;
using System.ComponentModel;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Tests.SyntaxConstructor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Syntax.Operators
{
    public class InPlaceBinaryOperatorFactoryTests
    {
        [DataTestMethod]
        [DataRow(BasicOperatorCode.IAAdd)]
        [DataRow(BasicOperatorCode.IASub)]
        [DataRow(BasicOperatorCode.IADiv)]
        [DataRow(BasicOperatorCode.IAMul)]
        [DataRow(BasicOperatorCode.IAFlr)]
        [DataRow(BasicOperatorCode.IAMod)]
        [DataRow(BasicOperatorCode.IAPow)]
        [DataRow(BasicOperatorCode.IBAnd)]
        [DataRow(BasicOperatorCode.IBOr)]
        [DataRow(BasicOperatorCode.IBXor)]
        [DataRow(BasicOperatorCode.IBLsh)]
        [DataRow(BasicOperatorCode.IBRsh)]
        public void FactoryCreateValid(BasicOperatorCode operatorCode)
        {
            // Arrange
            var lhs = BaseVisitClass.GetExpressionMock();
            var rhs = BaseVisitClass.GetExpressionMock();
            var factory = new InPlaceBinaryOperatorFactory(SourceReference.ClrSource, operatorCode);

            // Act
            var result = factory.Create(lhs, rhs);

            // Assert
            Assert.That.IsBinaryOperator<InPlaceBinaryOperator>(
                lhs, rhs, result
            );
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.IAMat)]
        public void FactoryCreateNYI(BasicOperatorCode operatorCode, string expectedKeyword)
        {
            // Arrange
            var factory = new InPlaceBinaryOperatorFactory(SourceReference.ClrSource, operatorCode);

            void Action()
            {
                factory.Create(null, null);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action)Action);

            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource, expectedKeyword);
        }

        [DataTestMethod]
        [DataRow(BasicOperatorCode.AAdd, DisplayName = "bin op a+b")]
        [DataRow(BasicOperatorCode.ASub, DisplayName = "bin op a-b")]
        [DataRow(BasicOperatorCode.AMul, DisplayName = "bin op a*b")]
        [DataRow(BasicOperatorCode.ADiv, DisplayName = "bin op a/b")]
        [DataRow(BasicOperatorCode.AFlr, DisplayName = "bin op a//b")]
        [DataRow(BasicOperatorCode.AMod, DisplayName = "bin op a%b")]
        [DataRow(BasicOperatorCode.APow, DisplayName = "bin op a**b")]
        [DataRow(BasicOperatorCode.BAnd, DisplayName = "bin op a&b")]
        [DataRow(BasicOperatorCode.BLsh, DisplayName = "bin op a<<b")]
        [DataRow(BasicOperatorCode.BRsh, DisplayName = "bin op a>>b")]
        [DataRow(BasicOperatorCode.BOr, DisplayName = "bin op a|b")]
        [DataRow(BasicOperatorCode.BXor, DisplayName = "bin op a^b")]
        [DataRow(BasicOperatorCode.CEq, DisplayName = "bin op a==b")]
        [DataRow(BasicOperatorCode.CNEq, DisplayName = "bin op a!=b")]
        [DataRow(BasicOperatorCode.CGt, DisplayName = "bin op a>b")]
        [DataRow(BasicOperatorCode.CGtEq, DisplayName = "bin op a>=b")]
        [DataRow(BasicOperatorCode.CLt, DisplayName = "bin op a<b")]
        [DataRow(BasicOperatorCode.CLtEq, DisplayName = "bin op a<=b")]
        [DataRow(BasicOperatorCode.CIn, DisplayName = "bin op a in b")]
        [DataRow(BasicOperatorCode.CNIn, DisplayName = "bin op a not in b")]
        [DataRow(BasicOperatorCode.CIs, DisplayName = "bin op a is b")]
        [DataRow(BasicOperatorCode.CIsN, DisplayName = "bin op a is not b")]
        [DataRow(BasicOperatorCode.ANeg, DisplayName = "un op +a")]
        [DataRow(BasicOperatorCode.APos, DisplayName = "un op -a")]
        [DataRow(BasicOperatorCode.BNot, DisplayName = "un op ~a")]
        [DataRow(BasicOperatorCode.LNot, DisplayName = "un op !a")]
        public void FactoryCreateNonInPlaceOps(BasicOperatorCode operatorCode, string expectedKeyword)
        {
            // Arrange
            var factory = new InPlaceBinaryOperatorFactory(SourceReference.ClrSource, operatorCode);

            void Action()
            {
                factory.Create(null, null);
            }

            // Act
            Assert.ThrowsException<InvalidEnumArgumentException>((Action)Action);
        }
    }
}