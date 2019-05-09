using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Tools.Tests
{
    [TestClass]
    public class MathUtilitiesTests
    {
        [DataTestMethod]
        [DataRow(1, 0, 1)]
        [DataRow(5125, 0, 1)]
        [DataRow(-128577, 0, 1)]
        [DataRow(2, 30, 1073741824)]
        [DataRow(-2, 30, 1073741824)]
        [DataRow(-2, 31, -2147483648)]
        public void IntPowCorrectResult(int x, int exp, int expected)
        {
            // Act
            int result = MathUtilities.Pow(x, exp);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IntPowThrowsOnNegativeExponent()
        {
            // Act
            void Action()
            {
                MathUtilities.Pow(5, -1);
            }

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>((Action)Action);
        }

        [DataTestMethod]
        [DataRow(2, 31)]
        [DataRow(-2, 32)]
        [DataRow(3, 32)]
        [DataRow(10, 10)]
        public void IntPowThrowsOnTooBigExponent(int x, int exp)
        {
            // Act
            void Action()
            {
                MathUtilities.Pow(x, exp);
            }

            // Assert
            Assert.ThrowsException<OverflowException>((Action)Action);
        }

        [DataTestMethod]
        [DataRow(1L, 0, 1L)]
        [DataRow(5125L, 0, 1L)]
        [DataRow(-128577L, 0, 1L)]
        [DataRow(2L, 62, 4611686018427387904)]
        [DataRow(-2L, 62, 4611686018427387904)]
        [DataRow(-2L, 63, long.MinValue)]
        public void LongPowCorrectResult(long x, int exp, long expected)
        {
            // Act
            long result = MathUtilities.Pow(x, exp);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LongPowThrowsOnNegativeExponent()
        {
            // Act
            void Action()
            {
                MathUtilities.Pow(5L, -1);
            }

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>((Action)Action);
        }

        [DataTestMethod]
        [DataRow(2L, 63)]
        [DataRow(-2L, 64)]
        [DataRow(3L, 64)]
        [DataRow(10L, 20)]
        public void LongPowThrowsOnTooBigExponent(long x, int exp)
        {
            // Act
            void Action()
            {
                MathUtilities.Pow(x, exp);
            }

            // Assert
            Assert.ThrowsException<OverflowException>((Action)Action);
        }
    }
}