using System;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyIntegerTests : BaseEntityTester<PyInteger, int>
    {
        protected override string ExpectedTypeName => Localized_Base_Entities.Type_Int_Name;
        protected override Type ExpectedTypeDef => typeof(PyIntegerType);

        protected override PyInteger CreateEntity(PyProcessor processor, int value)
        {
            return new PyInteger(processor, value);
        }

        [DataTestMethod]
        [DataRow(0, "0")]
        [DataRow(+1, "1")]
        [DataRow(-1, "-1")]
        [DataRow(int.MaxValue, "2147483647")]
        [DataRow(int.MinValue, "-2147483648")]
        public override void ToStringDataTest(int value, string expected)
        {
            base.ToStringDataTest(value, expected);
        }
    }
}