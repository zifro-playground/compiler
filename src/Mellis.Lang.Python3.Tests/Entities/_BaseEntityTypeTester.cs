﻿using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public abstract class BaseEntityTypeTester<T, TInner> : BaseEntityTester<T>
        where T : PyType<TInner>
        where TInner : IScriptType
    {
        protected abstract string ExpectedClassName { get; }

        [TestMethod]
        public void ClassNameTest()
        {
            // Act
            var entity = CreateEntity();

            // Assert
            Assert.AreEqual(entity.ClassName, ExpectedClassName, "Entity class name did not match.");
        }

        // Some testing libraries only check 1 deep in inheritance for tests
        [TestMethod]
        public override void CopyGivesRightType()
        {
            base.CopyGivesRightType();
        }
    }
}