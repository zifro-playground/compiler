using System;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities.Classes
{
    [TestClass]
    public class PyNoneTypeTests : BaseEntityTypeTester<PyNoneType, PyNone>
    {
        protected override string ExpectedClassName => Localized_Base_Entities.Type_Null_Name;

        protected override PyNoneType CreateEntity(PyProcessor processor)
        {
            return new PyNoneType(processor, nameof(PyNoneTypeTests));
        }

        [TestMethod]
        public void CtorCantUseIt()
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[0]);
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