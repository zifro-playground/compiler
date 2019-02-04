using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Entities;

namespace Zifro.Compiler.Tests
{
    public class BaseTestClass
    {
        protected Mock<IProcessor> processorMock;
        protected Mock<IScriptTypeFactory> factoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            processorMock = new Mock<IProcessor>();
            factoryMock = new Mock<IScriptTypeFactory>();

            processorMock.SetupGet(o => o.Factory).Returns(factoryMock.Object);
            factoryMock.Setup(o => o.Create(It.IsAny<int>()))
                .Returns<int>(GetInteger);
            factoryMock.Setup(o => o.Create(It.IsAny<double>()))
                .Returns<double>(GetDouble);
            factoryMock.Setup(o => o.Create(It.IsAny<string>()))
                .Returns<string>(GetString);
        }

        protected IntegerBase GetInteger(int value)
        {
            var mock = new Mock<IntegerBase>();
            mock.SetupGet(o => o.Processor).Returns(processorMock.Object);
            mock.CallBase = true;
            IntegerBase integerBase = mock.Object;
            integerBase.Value = value;
            return integerBase;
        }

        protected DoubleBase GetDouble(double value)
        {
            var mock = new Mock<DoubleBase>();
            mock.SetupGet(o => o.Processor).Returns(processorMock.Object);
            mock.CallBase = true;
            DoubleBase doubleBase = mock.Object;
            doubleBase.Value = value;
            return doubleBase;
        }

        protected StringBase GetString(string value)
        {
            var mock = new Mock<StringBase>();
            mock.SetupGet(o => o.Processor).Returns(processorMock.Object);
            mock.CallBase = true;
            StringBase stringBase = mock.Object;
            stringBase.Value = value;
            return stringBase;
        }
    }
}