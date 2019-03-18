using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public abstract class BaseEntityTester<T, TInner>
        : BaseEntityTester<T>
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

        #region Redefine tests

        // Some testing libraries only check 1 deep in inheritance for tests

        [TestMethod]
        public override void CopyGivesRightType()
        {
            base.CopyGivesRightType();
        }

        [TestMethod]
        public override void TypeNameTest()
        {
            base.TypeNameTest();
        }

        [TestMethod]
        public override void GetTypeDefTypeTest()
        {
            base.GetTypeDefTypeTest();
        }

        #endregion
    }
}