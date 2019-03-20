using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities.Functions
{
    public abstract class BaseClrFunctionTester<T>
        where T : IClrFunction
    {
        protected abstract T CreateFunction();

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
    }
}