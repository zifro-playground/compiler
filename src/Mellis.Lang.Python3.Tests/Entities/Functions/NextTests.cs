using Mellis.Lang.Python3.Entities.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities.Functions
{
    [TestClass]
    public class NextTests : BaseClrFunctionTester<Next>
    {
        protected override Next CreateFunction()
        {
            return new Next();
        }

        protected override int MaximumArguments => 2;
        protected override int MinimumArguments => 1;

        [TestMethod]
        public void GetNextFromIEnumerator()
        {
            // Arrange
            // TODO

            // Act

            // Assert
        }

        [TestMethod]
        public void ReturnsDefaultIfExhausted()
        {
            // Arrange
            // TODO

            // Act

            // Assert
        }

        [TestMethod]
        public void ThrowsIfExhaustedWithNoDefault()
        {
            // Arrange
            // TODO

            // Act

            // Assert
        }

        [TestMethod]
        public void ThrowsIfNotIEnumerator()
        {
            // Arrange
            // TODO

            // Act

            // Assert
        }
    }
}