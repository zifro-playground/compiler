using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Tools.Extensions;

namespace Zifro.Compiler.Tools.Tests
{
    [TestClass]
    public class ScriptTypeFactoryExtensionsTests
    {
        Mock<IScriptTypeFactory> factoryMock;
        IScriptTypeFactory factoryObject;

        [TestInitialize]
        public void TestInitialize()
        {
            factoryMock = new Mock<IScriptTypeFactory>(MockBehavior.Strict);
            factoryObject = factoryMock.Object;
        }

        [TestMethod]
        public void TryCreateIntegersAndText()
        {
            // Arrange
            factoryMock.Setup(o => o.Create((int)0)).Returns((IScriptType) null);
            factoryMock.Setup(o => o.Create((byte)0)).Returns((IScriptType)null);
            factoryMock.Setup(o => o.Create((short)0)).Returns((IScriptType)null);
            factoryMock.Setup(o => o.Create((long)0)).Returns((IScriptType)null);
            factoryMock.Setup(o => o.Create('m')).Returns((IScriptType)null);
            factoryMock.Setup(o => o.Create("foo")).Returns((IScriptType)null);

            // Act + Assert
            Assert.IsTrue(factoryObject.TryCreate<int>(0, out _));
            Assert.IsTrue(factoryObject.TryCreate<byte>(0, out _));
            Assert.IsTrue(factoryObject.TryCreate<short>(0, out _));
            Assert.IsTrue(factoryObject.TryCreate<long>(0, out _));
            Assert.IsTrue(factoryObject.TryCreate<char>('m', out _));
            Assert.IsTrue(factoryObject.TryCreate<string>("foo", out _));

            // Assert
            factoryMock.Verify();
        }

        [TestMethod]
        public void TryCreateNulls()
        {
            // Arrange
            factoryMock.SetupGet(o => o.Null)
                .Returns((IScriptType)null);
            
            // Act + Assert
            Assert.IsTrue(factoryObject.TryCreate<string>(null, out _));
            Assert.IsTrue(factoryObject.TryCreate<IList<IScriptType>>(null, out _));
            Assert.IsTrue(factoryObject.TryCreate<IDictionary<IScriptType, IScriptType>>(null, out _));

            // Assert
            factoryMock.VerifyGet(o => o.Null, Times.Exactly(3));
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TryCreateTrue()
        {
            // Arrange
            factoryMock.SetupGet(o => o.True)
                .Returns((IScriptType)null);

            // Act + Assert
            Assert.IsTrue(factoryObject.TryCreate<bool>(true, out _));

            // Assert
            factoryMock.VerifyGet(o => o.True, Times.Once);
            factoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void TryCreateFalse()
        {
            // Arrange
            factoryMock.SetupGet(o => o.False)
                .Returns((IScriptType) null);

            // Act + Assert
            Assert.IsTrue(factoryObject.TryCreate<bool>(false, out _));

            // Assert
            factoryMock.VerifyGet(o => o.False, Times.Once);
            factoryMock.VerifyNoOtherCalls();
        }
    }
}