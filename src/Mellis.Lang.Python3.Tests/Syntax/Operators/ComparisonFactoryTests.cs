using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Tests.SyntaxConstructor;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Syntax.Operators.Comparisons;

namespace Mellis.Lang.Python3.Tests.Syntax.Operators
{
    [TestClass]
    public class ComparisonFactoryTests
    {
        [DataTestMethod]
        [DataRow(typeof(CompareEquals), ComparisonType.Equals, OperatorCode.CEq,
            DisplayName = "factory create ==")]
        [DataRow(typeof(CompareLessThan), ComparisonType.LessThan, OperatorCode.CLt,
            DisplayName = "factory create <")]
        [DataRow(typeof(CompareLessThanOrEqual), ComparisonType.LessThanOrEqual, OperatorCode.CLtEq,
            DisplayName = "factory create <=")]
        [DataRow(typeof(CompareGreaterThan), ComparisonType.GreaterThan, OperatorCode.CGt,
            DisplayName = "factory create >")]
        [DataRow(typeof(CompareGreaterThanOrEqual), ComparisonType.GreaterThanOrEqual, OperatorCode.CGtEq,
            DisplayName = "factory create >=")]
        [DataRow(typeof(CompareNotEquals), ComparisonType.NotEquals, OperatorCode.CNEq,
            DisplayName = "factory create !=")]
        [DataRow(typeof(CompareIn), ComparisonType.In, OperatorCode.CIn,
            DisplayName = "factory create in")]
        [DataRow(typeof(CompareInNot), ComparisonType.InNot, OperatorCode.CNIn,
            DisplayName = "factory create not in")]
        [DataRow(typeof(CompareIs), ComparisonType.Is, OperatorCode.CIs,
            DisplayName = "factory create is")]
        [DataRow(typeof(CompareIsNot), ComparisonType.IsNot, OperatorCode.CIsN,
            DisplayName = "factory create is not")]
        public void FactoryCreateValid_Test(Type expectedType, ComparisonType compType, OperatorCode opCode)
        {
            // Arrange
            ExpressionNode lhs = BaseVisitClass.GetExpressionMock();
            ExpressionNode rhs = BaseVisitClass.GetExpressionMock();
            var factory = new ComparisonFactory(compType);

            // Act
            Comparison result = factory.Create(lhs, rhs);

            // Assert
            Assert.AreEqual(compType, result.Type, "ComparisonType did not match.");
            Assert.AreEqual(opCode, result.OpCode, "OperatorCode did not match.");
            Assert.IsInstanceOfType(result, expectedType);
            Assert.AreSame(lhs, result.LeftOperand, "LHS did not match.");
            Assert.AreSame(rhs, result.RightOperand, "RHS did not match.");
        }

        [DataTestMethod]
        [DataRow("<>", DisplayName = "factory create <>")]
        public void FactoryCreateNYI_Test(string expectedKeyword, ComparisonType inputType)
        {
            // Arrange
            ExpressionNode lhs = BaseVisitClass.GetExpressionMock();
            ExpressionNode rhs = BaseVisitClass.GetExpressionMock();
            var factory = new ComparisonFactory(inputType);

            void GetResult()
            {
                factory.Create(lhs, rhs);
            }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action)GetResult);

            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource, expectedKeyword);
            Assert.AreEqual(expectedKeyword, ex.Keyword);
        }

    }
}