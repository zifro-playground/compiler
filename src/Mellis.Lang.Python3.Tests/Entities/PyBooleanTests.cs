using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyBooleanTests : BaseEntityTester<PyBoolean, bool>
    {
        protected override string ExpectedTypeName => Localized_Base_Entities.Type_Boolean_Name;

        protected override PyBoolean CreateEntity(PyProcessor processor, bool value)
        {
            return new PyBoolean(processor, value, nameof(PyBooleanTests));
        }

        [DataTestMethod]
        [DataRow(true, "True")]
        [DataRow(false, "False")]
        public override void ToStringDataTest(bool value, string expected)
        {
            base.ToStringDataTest(value, expected);
        }
    }
}