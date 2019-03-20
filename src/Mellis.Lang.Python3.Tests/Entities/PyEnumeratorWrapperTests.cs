using System;
using System.Collections;
using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    [DoNotParallelize]
    public class PyEnumeratorWrapperTests
        : BaseEntityTester<PyEnumeratorWrapper, IEnumerator<IScriptType>>
    {
        private Mock<IScriptType> sourceTypeMock;
        private Mock<IScriptType> sourceTypeMockTypeDefMock;
        private const string sourceTypeName = "foo";

        protected override string ExpectedTypeName
            => Localized_Python3_Entities.Type_Enumerator_Name;

        protected override Type ExpectedTypeDef
            => sourceTypeMockTypeDefMock.Object.GetType();

        [TestInitialize]
        public void TestInitialize()
        {
            sourceTypeMock = new Mock<IScriptType>();

            sourceTypeMock.Setup(o => o.GetTypeName())
                .Returns(sourceTypeName)
                .Verifiable();

            sourceTypeMockTypeDefMock = new Mock<IScriptType>();
            sourceTypeMock.Setup(o => o.GetTypeDef())
                .Returns(sourceTypeMockTypeDefMock.Object)
                .Verifiable();
        }

        protected override PyEnumeratorWrapper CreateEntity(
            PyProcessor processor,
            IEnumerator<IScriptType> value)
        {
            return new PyEnumeratorWrapper(
                processor: processor,
                sourceType: sourceTypeMock.Object,
                enumerator: value,
                name: nameof(PyEnumeratorWrapperTests));
        }

        [TestMethod]
        public void ToStringTest()
        {
            var enumMock = new Mock<IEnumerator<IScriptType>>();

            base.ToStringDataTest(enumMock.Object, string.Format(
                Localized_Python3_Entities.Type_Enumerator_ToString,
                sourceTypeName
            ));
        }

        [TestMethod]
        public void WrapsEnumMoveNext()
        {
            // Arrange
            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.MoveNext())
                .Returns(false).Verifiable();

            var entity = (IEnumerator)CreateEntity(enumMock.Object);

            // Act
            entity.MoveNext();

            // Assert
            enumMock.Verify();
        }

        [TestMethod]
        public void WrapsEnumCurrentGeneric()
        {
            // Arrange
            var current = Mock.Of<IScriptType>();

            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.SetupGet(o => o.Current)
                .Returns(current).Verifiable();

            var entity = (IEnumerator<IScriptType>)CreateEntity(enumMock.Object);

            // Act
            var result = entity.Current;

            // Assert
            Assert.AreSame(current, result);
            enumMock.Verify();
        }

        [TestMethod]
        public void WrapsEnumCurrent()
        {
            // Arrange
            var current = Mock.Of<IScriptType>();

            var enumMock = new Mock<IEnumerator<IScriptType>>();

            enumMock.As<IEnumerator>().SetupGet(o => o.Current)
                .Returns(current).Verifiable();

            var entity = (IEnumerator)CreateEntity(enumMock.Object);

            // Act
            var result = entity.Current;

            // Assert
            Assert.AreSame(current, result);
            enumMock.Verify();
        }

        [TestMethod]
        public void WrapsEnumDispose()
        {
            // Arrange
            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.Dispose())
                .Verifiable();

            var entity = (IEnumerator<IScriptType>)CreateEntity(enumMock.Object);

            // Act
            entity.Dispose();

            // Assert
            enumMock.Verify();
        }

        [TestMethod]
        public void WrapsEnumReset()
        {
            // Arrange
            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.Reset())
                .Verifiable();

            var entity = (IEnumerator)CreateEntity(enumMock.Object);

            // Act
            entity.Reset();

            // Assert
            enumMock.Verify();
        }

        [TestMethod]
        public void GetEnumeratorGenericReturnsSelf()
        {
            // Arrange
            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.Reset())
                .Verifiable();

            var entity = (IEnumerable<IScriptType>)CreateEntity(enumMock.Object);

            // Act
            var result = entity.GetEnumerator();

            // Assert
            Assert.AreSame(entity, result);
        }

        [TestMethod]
        public void GetEnumeratorReturnsSelf()
        {
            // Arrange
            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.Reset())
                .Verifiable();

            var entity = (IEnumerable)CreateEntity(enumMock.Object);

            // Act
            var result = entity.GetEnumerator();

            // Assert
            Assert.AreSame(entity, result);
        }
    }
}