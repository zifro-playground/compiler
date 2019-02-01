using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Tools.Tests.AutoObject
{
    [TestClass]
    public class AutoValueTests
    {
        private Mock<IScriptTypeFactory> factoryMock;
        private Mock<IProcessor> processorMock;
        private Mock<IScriptType> valueMock;

        [TestInitialize]
        public void TestInitialize()
        {
            factoryMock = new Mock<IScriptTypeFactory>(MockBehavior.Strict);

            processorMock = new Mock<IProcessor>(MockBehavior.Strict);
            processorMock.Setup(m => m.Factory).Returns(factoryMock.Object);

            valueMock = new Mock<IScriptType>(MockBehavior.Strict);
            valueMock.Setup(o => o.Processor)
                .Returns(processorMock.Object);
        }

        private void VerifyMocks()
        {
            factoryMock.Verify();
            processorMock.Verify();
            valueMock.Verify();
        }

        [TestMethod]
        public void GetPublicPropertyTest()
        {
            // Arrange
            const string testString = "foo";
            var model = new TestingClass
            {
                Processor = processorMock.Object,
                PublicProperty = testString
            };

            factoryMock.Setup(o => o.Create(testString))
                .Returns(valueMock.Object);

            // Act
            IScriptType value = model.GetProperty(nameof(model.PublicProperty));

            // Assert
            Assert.AreSame(value, valueMock.Object);
            VerifyMocks();
        }

        [TestMethod]
        public void GetPrivatePropertyTest()
        {
            // Arrange
            const string testString = "foo";
            var model = new TestingClass
            {
                Processor = processorMock.Object,
            }.WithPrivateProperty(testString);

            factoryMock.Setup(o => o.Create(testString))
                .Returns(valueMock.Object);

            // Act
            IScriptType value = model.GetProperty(TestingClass.PrivatePropertyName);

            // Assert
            Assert.AreSame(value, valueMock.Object);
            VerifyMocks();
        }

        [TestMethod]
        public void GetProtectedPropertyTest()
        {
            // Arrange
            const string testString = "foo";
            var model = new TestingClass
            {
                Processor = processorMock.Object,
            }.WithProtectedProperty(testString);

            factoryMock.Setup(o => o.Create(testString))
                .Returns(valueMock.Object);

            // Act
            IScriptType value = model.GetProperty(TestingClass.ProtectedPropertyName);

            // Assert
            Assert.AreSame(value, valueMock.Object);
            VerifyMocks();
        }

        [TestMethod]
        public void GetProtectedDerivedPropertyTest()
        {
            const string testString = "foo";

            // Arrange
            var model = new TestingDerivedClass
            {
                Processor = processorMock.Object,
            }.WithProtectedProperty(testString);

            factoryMock.Setup(o => o.Create(testString))
                .Returns(valueMock.Object);

            // Act
            IScriptType value = model.GetProperty(TestingClass.ProtectedPropertyName);

            // Assert
            Assert.AreSame(value, valueMock.Object);
            VerifyMocks();
        }

        [TestMethod]
        public void GetNullTest()
        {
            // Arrange
            const string testString = null;
            var model = new TestingClass
            {
                Processor = processorMock.Object,
                PublicProperty = testString
            };

            factoryMock.Setup(o => o.Null)
                .Returns(valueMock.Object);

            // Act
            IScriptType value = model.GetProperty(nameof(model.PublicProperty));

            // Assert
            Assert.AreSame(value, valueMock.Object);
            VerifyMocks();
        }

        [TestMethod]
        public void GetPropertyWithoutAttributeTest()
        {
            // Arrange
            const string testString = null;
            var model = new TestingClass
            {
                Processor = processorMock.Object,
                PublicFieldWithoutAttribute = testString
            };

            factoryMock.Setup(o => o.Null)
                .Returns(valueMock.Object);

            // Act
            IScriptType value = model.GetProperty(nameof(model.PublicFieldWithoutAttribute));

            // Assert
            Assert.AreSame(value, valueMock.Object);
            VerifyMocks();
        }
    }
}