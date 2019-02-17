using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Operators;
using Mellis.Lang.Python3.Syntax.Operators.Comparisons;
using Mellis.Lang.Python3.Tests.SyntaxConstructor;

namespace Mellis.Lang.Python3.Tests.Syntax.Operators
{
    [TestClass]
    public class ComparisonFactoryTests
    {
        [DataTestMethod]
        [DataRow(typeof(CompareEquals), ComparisonType.Equals,
            DisplayName = "factory create ==")]
        public void FactoryCreateValid_Test(Type expectedType, ComparisonType type)
        {
            // Arrange
            ExpressionNode lhs = BaseVisitClass.GetExpressionMock();
            ExpressionNode rhs = BaseVisitClass.GetExpressionMock();
            var factory = new ComparisonFactory(type);

            // Act
            Comparison result = factory.Create(lhs, rhs);

            // Assert
            Assert.IsInstanceOfType(result, expectedType);
            Assert.AreSame(lhs, result.LeftOperand);
            Assert.AreSame(rhs, result.RightOperand);
        }

        [DataTestMethod]
        [DataRow("<", ComparisonType.LessThan,
            DisplayName = "factory create <")]
        [DataRow("<=", ComparisonType.LessThanOrEqual,
            DisplayName = "factory create <=")]
        [DataRow(">", ComparisonType.GreaterThan,
            DisplayName = "factory create >")]
        [DataRow(">=", ComparisonType.GreaterThanOrEqual,
            DisplayName = "factory create >=")]
        [DataRow("!=", ComparisonType.NotEquals,
            DisplayName = "factory create !=")]
        [DataRow("<>", ComparisonType.NotEqualsABC,
            DisplayName = "factory create <>")]
        [DataRow("in", ComparisonType.In,
            DisplayName = "factory create in")]
        [DataRow("not in", ComparisonType.InNot,
            DisplayName = "factory create not in")]
        [DataRow("is", ComparisonType.Is,
            DisplayName = "factory create is")]
        [DataRow("is not", ComparisonType.IsNot,
            DisplayName = "factory create is not")]
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
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) GetResult);

            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource, expectedKeyword);
            Assert.AreEqual(expectedKeyword, ex.Keyword);
        }

    }
}