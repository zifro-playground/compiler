using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public abstract class BaseEntityTester<T>
        where T : IScriptType
    {
        protected abstract T CreateEntity(PyProcessor processor);

        protected T CreateEntity()
        {
            return CreateEntity(new PyProcessor());
        }

        [TestMethod]
        public void CopyGivesRightType()
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