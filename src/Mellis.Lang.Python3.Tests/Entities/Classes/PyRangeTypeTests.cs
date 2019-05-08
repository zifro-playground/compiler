using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
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
    public class PyRangeTypeTests : BaseEntityTypeTester<PyRangeType, PyRange>
    {
        protected override string ExpectedClassName => Localized_Python3_Entities.Type_Range_Name;

        protected override PyRangeType CreateEntity(PyProcessor processor)
        {
            return new PyRangeType(processor);
        }

        private static ScriptInteger CreateConvertibleInteger(int value)
        {
            var mock = new Mock<ScriptInteger>(null, value);
            mock.Setup(o => o.GetTypeName()).Returns($"fake<{value}>");
            return mock.Object;
        }

        [TestMethod]
        public void CtorEmptyArgsTest()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            Assert.That.ThrowsTooFewArguments(entity, minimum: 1);
        }

        [TestMethod]
        public void CtorTooManyArgsTest()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            Assert.That.ThrowsTooManyArguments(entity, maximum: 3);
        }

        [TestMethod]
        public void Ctor1ArgValid()
        {
            // Arrange
            const int to = 10;

            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(
                CreateConvertibleInteger(to)
            );

            // Assert
            Assert.That.ScriptTypeEqual((from: 0, to, step: 1), result);
        }

        [TestMethod]
        public void Ctor2ArgsValid()
        {
            // Arrange
            const int from = 3;
            const int to = 7;

            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(
                CreateConvertibleInteger(from),
                CreateConvertibleInteger(to)
            );

            // Assert
            Assert.That.ScriptTypeEqual((from, to, step: 1), result);
        }

        [TestMethod]
        public void Ctor3ArgsValid()
        {
            // Arrange
            const int from = 10;
            const int to = 5;
            const int step = -1;

            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(
                CreateConvertibleInteger(from),
                CreateConvertibleInteger(to),
                CreateConvertibleInteger(step)
            );

            // Assert
            Assert.That.ScriptTypeEqual((from, to, step), result);
        }

        [TestMethod]
        public void CtorThrows_1Args_Arg1NotInteger()
        {
            // Arrange
            var entity = CreateEntity();
            var unexpectedMock = new Mock<IScriptType>();

            unexpectedMock.Setup(o => o.GetTypeName())
                .Returns("foo1_1");

            // Act
            var ex = Assert.That.Throws(entity,
                unexpectedMock.Object
            );

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg_NotInteger),
                "foo1_1"
            );
        }

        [TestMethod]
        public void CtorThrows_2Args_Arg1NotInteger()
        {
            // Arrange
            var entity = CreateEntity();
            var unexpectedMock = new Mock<IScriptType>();

            unexpectedMock.Setup(o => o.GetTypeName())
                .Returns("foo2_1");

            // Act
            var ex = Assert.That.Throws(entity,
                unexpectedMock.Object,
                CreateConvertibleInteger(0)
            );

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg_NotInteger),
                "foo2_1"
            );
        }

        [TestMethod]
        public void CtorThrows_2Args_Arg2NotInteger()
        {
            // Arrange
            var entity = CreateEntity();
            var unexpectedMock = new Mock<IScriptType>();

            unexpectedMock.Setup(o => o.GetTypeName())
                .Returns("foo2_2");

            // Act
            var ex = Assert.That.Throws(entity,
                CreateConvertibleInteger(0),
                unexpectedMock.Object
            );

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg_NotInteger),
                "foo2_2"
            );
        }

        [TestMethod]
        public void CtorThrows_3Args_Arg3NotInteger()
        {
            // Arrange
            var entity = CreateEntity();
            var unexpectedMock = new Mock<IScriptType>();

            unexpectedMock.Setup(o => o.GetTypeName())
                .Returns("foo3_3");

            // Act
            var ex = Assert.That.Throws(entity,
                CreateConvertibleInteger(0),
                CreateConvertibleInteger(1),
                unexpectedMock.Object
            );

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg_NotInteger),
                "foo3_3"
            );
        }

        [TestMethod]
        public void CtorThrows_3Args_Arg3Zero()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            var ex = Assert.That.Throws(entity,
                CreateConvertibleInteger(0),
                CreateConvertibleInteger(1),
                CreateConvertibleInteger(0)
            );

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_RangeType_Ctor_Arg3_Zero)
            );
        }
    }
}