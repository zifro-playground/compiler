using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons;
using Zifro.Compiler.Lang.Python3.Tests.SyntaxConstructor;

namespace Zifro.Compiler.Lang.Python3.Tests.Syntax.Operators
{
    [TestClass]
    public class ComparisonFactoryTests
    {
        [DataTestMethod]
        [DataRow(ComparisonType.Equals, typeof(CompareEquals), DisplayName = "factory create ==")]
        public void FactoryCreateValid_Test(ComparisonType type, Type expectedType)
        {
            // Arrange
            ExpressionNode lhs = BaseVisitClass.GetExpressionMock();
            ExpressionNode rhs = BaseVisitClass.GetExpressionMock();
            var source = new SourceReference(1, 2, 3, 4);
            var factory = new ComparisonFactory(source, type);

            // Act
            Comparison result = factory.Create(lhs, rhs);

            // Assert
            Assert.IsInstanceOfType(result, expectedType);
            Assert.AreSame(lhs, result.LeftOperand);
            Assert.AreSame(rhs, result.RightOperand);
            Assert.AreEqual(source, result.Source);
        }

        [DataTestMethod]
        [DataRow(ComparisonType.LessThan, "<", DisplayName = "factory create <")]
        [DataRow(ComparisonType.LessThanOrEqual, "<=", DisplayName = "factory create <=")]
        [DataRow(ComparisonType.GreaterThan, ">", DisplayName = "factory create >")]
        [DataRow(ComparisonType.GreaterThanOrEqual, ">=", DisplayName = "factory create >=")]
        [DataRow(ComparisonType.NotEquals, "!=", DisplayName = "factory create !=")]
        [DataRow(ComparisonType.NotEqualsABC, "<>", DisplayName = "factory create <>")]
        [DataRow(ComparisonType.In, "in", DisplayName = "factory create in")]
        [DataRow(ComparisonType.InNot, "not in", DisplayName = "factory create not in")]
        [DataRow(ComparisonType.Is, "is", DisplayName = "factory create is")]
        [DataRow(ComparisonType.IsNot, "is not", DisplayName = "factory create is not")]
        public void FactoryCreateNYI_Test(ComparisonType inputType, string expectedKeyword)
        {
            // Arrange
            ExpressionNode lhs = BaseVisitClass.GetExpressionMock();
            ExpressionNode rhs = BaseVisitClass.GetExpressionMock();
            var source = new SourceReference(1, 2, 3, 4);
            var factory = new ComparisonFactory(source, inputType);

            void GetResult()
            {
                factory.Create(lhs, rhs);
            }

            // Act + Assert
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedExceptionKeyword>((Action) GetResult);

            Assert.That.ErrorNotYetImplFormatArgs(ex, source, expectedKeyword);
            Assert.AreEqual(expectedKeyword, ex.Keyword);
            Assert.AreEqual(source, ex.SourceReference);
        }

    }
}