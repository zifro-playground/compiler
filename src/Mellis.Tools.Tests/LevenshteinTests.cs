using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Tools.Tests
{
    [TestClass]
    public class LevenshteinTests
    {
        [DataTestMethod]
        [DataRow("book", "back", 2)]
        [DataRow("x", "X", 1)]
        [DataRow("a", "z", 1)]
        [DataRow("a", "a123456789", 9)]
        [DataRow("123456789", "1", 8)]
        [DataRow("fisk", "mås", 3)]
        [DataRow("same", "same", 0)]
        [DataRow("", "null", 4)]
        [DataRow(null, "null", 4)] // Assume null as empty string
        [DataRow(null, null, 0)] // Assume null as empty string
        [DataRow("null", null, 4)] // Assume null as empty string
        public void TestDistanceResult(string a, string b, int expected)
        {
            // Act
            int result = StringUtilities.LevenshteinDistance(in a, in b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestBestMatchReturned()
        {
            // Arrange
            string[] strings = {
                "abc123",
                "123abc",
                "foo_bar",
                "Hello World!",
                "Kalle"
            };

            const string input = "Hobbe";
            const string expected = "Kalle";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.AreEqual(expected, result.value);
        }

        [TestMethod]
        public void TestBestMatchReturnsNullOnListEmpty()
        {
            // Arrange
            string[] strings = {};

            const string input = "Hobbe";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.IsTrue(result.IsNull);
        }

        [TestMethod]
        public void TestBestMatchReturnsNullOnListNull()
        {
            // Arrange
            const string input = "Hobbe";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, null);

            // Assert
            Assert.IsTrue(result.IsNull);
        }

        [TestMethod]
        public void TestBestMatchReturnedOnListItemNull()
        {
            // Arrange
            string[] strings = {
                "abc123",
                null,
                "foo_bar",
                "Hello World!",
                "Kalle"
            };

            const string input = "Hobbe";
            const string expected = "Kalle";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.AreEqual(expected, result.value);
        }

        [TestMethod]
        public void TestBestMatchReturnsNullOnInputNull()
        {
            // Arrange
            string[] strings = {
                "abc",
                "123",
                "foo_bar",
                "Hello World!",
                "Kalle"
            };

            const string input = null;

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.IsTrue(result.IsNull);
        }

        [TestMethod]
        public void TestBestMatchReturnsNullOnInputEmpty()
        {
            // Arrange
            string[] strings = {
                "abc",
                "123",
                "foo_bar",
                "Hello World!",
                "Kalle"
            };

            const string input = "";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.IsTrue(result.IsNull);
        }

        [TestMethod]
        public void TestBestMatchReturnsNullOnListOnlyNullsOrEmpty()
        {
            // Arrange
            string[] strings = {
                null,
                "",
                null,
                ""
            };

            const string input = "Hobbe";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.IsTrue(result.IsNull);
        }

        [TestMethod]
        public void TestBestMatchReturnsAlphabeticallyBestMatch()
        {
            // Arrange
            string[] strings = {
                "b___",
                "c___",
                "a___",
                "d___"
            };

            const string input = "e___";
            const string expected = "a___";

            // Act
            var result = StringUtilities.LevenshteinBestMatch(input, in strings);

            // Assert
            Assert.AreEqual(expected, result.value);
        }

        [DataTestMethod] // threshold: (a.len + b.len) / 8 + 1 [using floor division]
        [DataRow("a", "bc")] // dist:2, threshold: 1
        [DataRow("foo", "bar")] // dist:3, threshold: 1
        [DataRow("foo", "far")] // dist:2, threshold: 1
        [DataRow("1234567", "1234")] // dist:3, threshold: 2
        [DataRow("Kalle", "Hobbe")] // dist:4, threshold: 2
        public void TestBestMatchFilteredReturnsNullIfOutsideThreshold(string haystack, string needle)
        {
            // Arrange
            string[] strings = {
                haystack
            };

            // Act
            var result = StringUtilities.LevenshteinBestMatchFiltered(in needle, in strings);

            // Assert
            Assert.IsTrue(result.IsNull);
        }

        [DataTestMethod] // threshold: (a.len + b.len) / 8 + 1 [using floor division]
        [DataRow("x", "y")] // dist:1, threshold: 1
        [DataRow("foo", "moo")] // dist:1, threshold: 1
        [DataRow("1234567890", "1234567")] // dist:3, threshold: 3
        [DataRow("Kalle", "Palme")] // dist:2, threshold: 2
        public void TestBestMatchFilteredReturnsMatch(string haystack, string needle)
        {
            // Arrange
            string[] strings = {
                haystack
            };

            // Act
            var result = StringUtilities.LevenshteinBestMatchFiltered(in needle, in strings);

            // Assert
            Assert.AreEqual(haystack, result.value);
        }
    }
}