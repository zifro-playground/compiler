using System;
using Mellis.Core.Entities;
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
        [DataRow(BasicOperatorCode.ANeg, DisplayName = "is un op +a")]
        [DataRow(BasicOperatorCode.APos, DisplayName = "is un op -a")]
        [DataRow(BasicOperatorCode.BNot, DisplayName = "is un op ~a")]
        [DataRow(BasicOperatorCode.LNot, DisplayName = "is un op !a")]
        public void FactoryCreateNonInPlaceOps(BasicOperatorCode operatorCode, string expectedKeyword)
        {
            // TODO
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
    }
}