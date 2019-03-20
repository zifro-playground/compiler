using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities.Functions
{
    public abstract class BaseClrFunctionTester<T>
        where T : IClrFunction
    {
        protected abstract T CreateFunction();
        protected abstract int MaximumArguments { get; }
        protected abstract int MinimumArguments { get; }

        protected PyProcessor Processor;

        [TestInitialize]
        public void TestInitialize()
        {
            Processor = new PyProcessor();
        }

        protected T CreateInitializedFunction()
        {
            var function = CreateFunction();
            function.Processor = Processor;
            return function;
        }

        [TestMethod]
        public virtual void ThrowsOnTooFewArguments()
        {
            // Arrange
            var func = CreateInitializedFunction();

            // Act
            Assert.That.ThrowsTooFewArguments(func, MinimumArguments);
        }

        [TestMethod]
        public virtual void ThrowsOnTooManyArguments()
        {
            // Arrange
            var func = CreateInitializedFunction();

            // Act
            Assert.That.ThrowsTooManyArguments(func, MaximumArguments);
        }
    }
}