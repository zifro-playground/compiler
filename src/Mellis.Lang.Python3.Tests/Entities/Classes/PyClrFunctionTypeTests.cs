using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities.Classes
{
    [TestClass]
    public class PyClrFunctionTypeTests : BaseEntityTypeTester<PyClrFunctionType, ClrFunctionBase>
    {
        protected override string ExpectedClassName => Localized_Base_Entities.Type_ClrFunction_Name;

        protected override PyClrFunctionType CreateEntity(PyProcessor processor)
        {
            return new PyClrFunctionType(processor, nameof(PyClrFunctionTypeTests));
        }

        [TestMethod]
        public void CtorCantUseIt()
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke();
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Type_CannotInstantiate),
                /* type name */ ExpectedClassName);
        }
    }
}