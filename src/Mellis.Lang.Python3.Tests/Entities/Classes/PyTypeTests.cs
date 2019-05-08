using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities.Classes
{
    [TestClass]
    public class PyTypeTests : BaseEntityTypeTester<PyType, IScriptType>
    {
        protected override string ExpectedClassName => Localized_Python3_Entities.Type_Type_Name;

        protected override PyType CreateEntity(PyProcessor processor)
        {
            return new PyType(processor);
        }

        private static PyType<T> CreateEntity<T>(PyProcessor processor, string className)
            where T : IScriptType
        {
            return new Mock<PyType<T>>(processor, className)
            {
                CallBase = true
            }.Object;
        }

        [TestMethod]
        public void CtorTooFewArgs()
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke();
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeTooFewArgumentsException>((Action)Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooFewArguments),
                /* func name */ ExpectedClassName,
                /* minimum */ 1,
                /* actual */ 0);
        }

        [TestMethod]
        public void CtorTooManyArgs()
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[2]);
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeTooManyArgumentsException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                /* func name */ ExpectedClassName,
                /* maximum */ 1,
                /* actual */ 2);
        }

        [TestMethod]
        public void CtorGetsTypeDef()
        {
            // Arrange
            var argMock = new Mock<IScriptType>();
            var defMock = Mock.Of<IScriptType>();
            argMock.Setup(o => o.GetTypeDef())
                .Returns(defMock).Verifiable();

            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(argMock.Object);

            // Assert
            Assert.AreSame(defMock, result, "Did not return type definition of first argument.");
            argMock.Verify();
        }

        [TestMethod]
        public void CompareEqualTrueTest()
        {
            CompareEqualTest<IScriptType, IScriptType>(expected: true);
        }

        [TestMethod]
        public void CompareEqualFalseTest()
        {
            CompareEqualTest<IScriptType, PyString>(expected: false);
        }

        private static void CompareEqualTest<TA, TB>(bool expected)
            where TA : IScriptType
            where TB : IScriptType
        {
            // Arrange
            var processor = new PyProcessor();
            var entityA = CreateEntity<TA>(processor, "typeA");
            var entityB = CreateEntity<TB>(processor, "typeB");

            // Act
            var result = entityA.CompareEqual(entityB);

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [TestMethod]
        public void CompareNotEqualTrueTest()
        {
            CompareNotEqualTest<PyDouble, IScriptType>(expected: true);
        }

        [TestMethod]
        public void CompareNotEqualFalseTest()
        {
            CompareNotEqualTest<IScriptType, IScriptType>(expected: false);
        }

        private static void CompareNotEqualTest<TA, TB>(bool expected)
            where TA : IScriptType
            where TB : IScriptType
        {
            // Arrange
            var processor = new PyProcessor();
            var entityA = CreateEntity<TA>(processor, "typeA");
            var entityB = CreateEntity<TB>(processor, "typeB");

            // Act
            var result = entityA.CompareNotEqual(entityB);

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }
    }
}