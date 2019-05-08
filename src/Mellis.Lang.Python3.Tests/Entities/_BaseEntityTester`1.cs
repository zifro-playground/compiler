using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public abstract class BaseEntityTester<T>
        where T : IScriptType
    {
        protected abstract string ExpectedTypeName { get; }
        protected abstract Type ExpectedTypeDef { get; }
        protected abstract T CreateEntity(PyProcessor processor);

        protected T CreateEntity()
        {
            return CreateEntity(new PyProcessor());
        }

        [TestMethod]
        public virtual void GetTypeDefTypeTest()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            var result = entity.GetTypeDef();

            // Assert
            Assert.IsNotNull(result, "Type def was null.");
            Assert.IsInstanceOfType(result, ExpectedTypeDef, "Type def wrong type.");
        }

        [TestMethod]
        public virtual void TypeNameTest()
        {
            // Act
            var entity = CreateEntity();

            // Assert
            Assert.AreEqual(entity.GetTypeName(), ExpectedTypeName, "Entity type name did not match.");
        }
    }
}