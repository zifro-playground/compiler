using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;

namespace Mellis.Core.Tests
{
    [TestClass]
    public class SourceReferenceTests
    {
        public static void AssertExactCompare(SourceReference expected, SourceReference actual)
        {
            Assert.AreEqual(expected.IsFromClr, actual.IsFromClr, "CLR flag does not match.");
            Assert.AreEqual(expected.FromRow, actual.FromRow, "From row does not match.");
            Assert.AreEqual(expected.FromColumn, actual.FromColumn, "From column does not match.");
            Assert.AreEqual(expected.ToRow, actual.ToRow, "To row does not match.");
            Assert.AreEqual(expected.ToColumn, actual.ToColumn, "To column does not match.");
        }

        public static void AssertEquals(SourceReference expected, SourceReference actual)
        {
            Assert.IsTrue(expected.Equals(actual), $"expected({expected}).Equals(actual({actual})) is not true");
            Assert.IsTrue(actual.Equals(expected), $"actual({actual}).Equals(expected({expected})) is not true");
            Assert.IsTrue(expected == actual, $"expected({expected}) == actual({actual}) is not true");
            Assert.IsTrue(actual == expected, $"actual({actual}) == expected({expected}) is not true");
            Assert.IsFalse(expected != actual, $"expected({expected}) != actual({actual}) is not false");
            Assert.IsFalse(actual != expected, $"actual({actual}) != expected({expected}) is not false");
        }

        public static void AssertNotEquals(SourceReference expected, SourceReference actual)
        {
            Assert.IsFalse(expected.Equals(actual), $"expected({expected}).Equals(actual({actual})) is not false");
            Assert.IsFalse(actual.Equals(expected), $"actual({actual}).Equals(expected({expected})) is not false");
            Assert.IsFalse(expected == actual, $"expected({expected}) == actual({actual}) is not false");
            Assert.IsFalse(actual == expected, $"actual({actual}) == expected({expected}) is not false");
            Assert.IsTrue(expected != actual, $"expected({expected}) != actual({actual}) is not true");
            Assert.IsTrue(actual != expected, $"actual({actual}) != expected({expected}) is not true");
        }

        [TestMethod]
        public void MergeCLR_CLR_Test()
        {
            // Arrange
            var clr = SourceReference.ClrSource;

            // Act
            var result = SourceReference.Merge(clr, clr);

            // Assert
            AssertExactCompare(SourceReference.ClrSource, result);
        }

        [TestMethod]
        public void MergeCLR_NonCLR_Test()
        {
            // Arrange
            var clr = SourceReference.ClrSource;
            var nonClr = new SourceReference(1, 2, 3, 4);

            // Act
            var result = SourceReference.Merge(clr, nonClr);

            // Assert
            AssertExactCompare(nonClr, result);
        }

        [TestMethod]
        public void MergeNonClr_CLR_Test()
        {
            // Arrange
            var nonClr = new SourceReference(1, 2, 3, 4);
            var clr = SourceReference.ClrSource;

            // Act
            var result = SourceReference.Merge(nonClr, clr);

            // Assert
            AssertExactCompare(nonClr, result);
        }

        [TestMethod]
        public void MergeTwoSameRow_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 0, 1);
            var value2 = new SourceReference(1, 1, 3, 4);
            var expected = new SourceReference(1, 1, 0, 4);

            // Act
            var result = SourceReference.Merge(value1, value2);

            // Assert
            AssertExactCompare(expected, result);
        }

        [TestMethod]
        public void MergeTwoSameRowInverse_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 3, 4);
            var value2 = new SourceReference(1, 1, 0, 1);
            var expected = new SourceReference(1, 1, 0, 4);

            // Act
            var result = SourceReference.Merge(value1, value2);

            // Assert
            AssertExactCompare(expected, result);
        }

        [TestMethod]
        public void MergeTwoDifferentRowsInverse_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 0, 1);
            var value2 = new SourceReference(2, 2, 3, 4);
            var expected = new SourceReference(1, 2, 0, 4);

            // Act
            var result = SourceReference.Merge(value1, value2);

            // Assert
            AssertExactCompare(expected, result);
        }

        [TestMethod]
        public void MergeTwoDifferentRowsFallingEndColumn_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 3, 4);
            var value2 = new SourceReference(2, 2, 0, 1);
            var expected = new SourceReference(1, 2, 3, 1);

            // Act
            var result = SourceReference.Merge(value1, value2);

            // Assert
            AssertExactCompare(expected, result);
        }

        [TestMethod]
        public void MergeTwoInverseRowOrder_Test()
        {
            // Arrange
            var value1 = new SourceReference(2, 2, 3, 4);
            var value2 = new SourceReference(1, 1, 0, 1);
            var expected = new SourceReference(1, 2, 0, 4);

            // Act
            var result = SourceReference.Merge(value1, value2);

            // Assert
            AssertExactCompare(expected, result);
        }

        [TestMethod]
        public void MergeMany_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 5, 9);
            var value2 = new SourceReference(2, 2, 3, 3);
            var value3 = new SourceReference(5, 6, 10, 12);
            var value4 = new SourceReference(4, 9, 0, 1);
            var expected = new SourceReference(1, 9, 5, 1);

            // Act
            var result = SourceReference.Merge(value1, value2, value3, value4);

            // Assert
            AssertExactCompare(expected, result);
        }

        [TestMethod]
        public void MergeEmptyList_Test()
        {
            // Arrange
            void Action()
            {
                SourceReference.Merge();
            }

            // Act + Assert
            Assert.ThrowsException<ArgumentException>((Action) Action);
        }

        [TestMethod]
        public void MergeNull_Test()
        {
            // Arrange
            void Action()
            {
                SourceReference.Merge(null);
            }

            // Act + Assert
            Assert.ThrowsException<ArgumentNullException>((Action) Action);
        }

        [TestMethod]
        public void CompareEqualsCLR_CLR_Test()
        {
            // Arrange
            var value1 = SourceReference.ClrSource;
            var value2 = SourceReference.ClrSource;

            // Act + Assert
            AssertEquals(value1, value2);
        }

        [TestMethod]
        public void CompareNotEqualsNonCLR_CLR_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 5, 9);
            var value2 = SourceReference.ClrSource;

            // Act + Assert
            AssertNotEquals(value1, value2);
        }

        [TestMethod]
        public void CompareNotEqualsNonCLR_NonCLR_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 5, 9);
            var value2 = new SourceReference(2, 2, 3, 3);

            // Act + Assert
            AssertNotEquals(value1, value2);
        }

        [TestMethod]
        public void CompareEqualsNonCLR_NonCLR_Test()
        {
            // Arrange
            var value1 = new SourceReference(1, 1, 5, 9);
            var value2 = new SourceReference(1, 1, 5, 9);

            // Act + Assert
            AssertEquals(value1, value2);
        }
    }
}