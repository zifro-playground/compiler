using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public abstract class BaseEntityTester<T, TInner> : BaseEntityTester<T>
        where T : IScriptType
    {
        protected abstract T CreateEntity(PyProcessor processor, TInner value);
        protected virtual TInner DefaultValue => default;

        protected T CreateEntity(TInner value)
        {
            return CreateEntity(new PyProcessor(), value);
        }

        protected override T CreateEntity(PyProcessor processor)
        {
            return CreateEntity(processor, DefaultValue);
        }

        public virtual void ToStringDataTest(TInner value, string expected)
        {
            // Arrange
            var entity = CreateEntity(value);

            // Act
            var result = entity.ToString();

            // Assert
            Assert.AreEqual(expected, result, $"{typeof(T).Name}.ToString() returned wrong value.");
        }
    }

    public abstract class BaseEntityTester<T>
        where T : IScriptType
    {
        protected abstract string ExpectedTypeName { get; }
        protected abstract T CreateEntity(PyProcessor processor);

        protected T CreateEntity()
        {
            return CreateEntity(new PyProcessor());
        }

        [TestMethod]
        public void TypeNameTest()
        {
            // Act
            var entity = CreateEntity();

            // Assert
            Assert.AreEqual(entity.GetTypeName(), ExpectedTypeName, "Entity type name did not match.");
        }

        [TestMethod]
        public virtual void CopyGivesRightType()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            var result = entity.Copy("foo");

            // Assert
            Assert.IsNotNull(result, "Copied entity is null.");
            Assert.IsInstanceOfType(result, typeof(T), "Copied entity is not same type.");
            Assert.AreEqual(result.Name, "foo", "Copied entity did not get name.");
        }
    }
}