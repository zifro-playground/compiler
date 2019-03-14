using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyBooleanTypeTests : BaseEntityTypeTester<PyBooleanType, PyBoolean>
    {
        protected override string ExpectedClassName => Localized_Base_Entities.Type_Boolean_Name;

        protected override PyBooleanType CreateEntity(PyProcessor processor)
        {
            return new PyBooleanType(processor);
        }

        [TestMethod]
        public void CtorEmptyArgsTest()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(new IScriptType[0]);

            // Assert
            Assert.That.ScriptTypeEqual(expected: false, actual: result);
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

        [DataTestMethod]
        [DataRow(true, DisplayName = "arg0.isTruthy=>true")]
        [DataRow(false, DisplayName = "arg0.isTruthy=>false")]
        public void CtorOneTruthy(bool truthy)
        {
            // Arrange
            var argMock = new Mock<IScriptType>();
            argMock.Setup(o => o.IsTruthy())
                .Returns(truthy).Verifiable();

            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(new[] {argMock.Object});

            // Assert
            Assert.That.ScriptTypeEqual(expected: truthy, actual: result);
        }
    }
}