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
    public class PyTypeTests : BaseEntityTypeTester<PyType, IScriptType>
    {
        protected override string ExpectedClassName => Localized_Python3_Entities.Type_Type_Name;

        protected override PyType CreateEntity(PyProcessor processor)
        {
            return new PyType(processor, nameof(PyTypeTests));
        }

        [TestMethod]
        public void CtorTooFewArgs()
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[0]);
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
            var result = entity.Invoke(new[] {argMock.Object});

            // Assert
            Assert.AreSame(defMock, result, "Did not return type definition of first argument.");
            argMock.Verify();
        }
    }
}