using System;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.VM;
using Mellis.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyNoneTests : BaseEntityTester<PyNone, object>
    {
        protected override string ExpectedTypeName => Localized_Base_Entities.Type_Null_Name;
        protected override Type ExpectedTypeDef => typeof(PyType<PyNone>);

        protected override PyNone CreateEntity(PyProcessor processor, object value)
        {
            return new PyNone(processor);
        }

        [DataTestMethod]
        [DataRow(null, "None")]
        public override void ToStringDataTest(object value, string expected)
        {
            base.ToStringDataTest(value, expected);
        }
    }
}