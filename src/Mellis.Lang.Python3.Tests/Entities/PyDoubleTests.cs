using System;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyDoubleTests : BaseEntityTester<PyDouble, double>
    {
        protected override string ExpectedTypeName => Localized_Base_Entities.Type_Double_Name;
        protected override Type ExpectedTypeDef => typeof(PyDoubleType);

        protected override PyDouble CreateEntity(PyProcessor processor, double value)
        {
            return new PyDouble(processor, value, nameof(PyBooleanTests));
        }

        [DataTestMethod]
        [DataRow(0d, "0.0")]
        [DataRow(1d, "1.0")]
        [DataRow(1e10d, "10000000000.0")]
        [DataRow(1e16d, "1e+16")]
        [DataRow(1e-4d, "0.0001")]
        [DataRow(1e-15d, "1e-15")]
        public override void ToStringDataTest(double value, string expected)
        {
            base.ToStringDataTest(value, expected);
        }

        [DataTestMethod]
        [DataRow(double.PositiveInfinity, nameof(Localized_Base_Entities.Type_Double_PosInfinity))]
        [DataRow(double.NegativeInfinity, nameof(Localized_Base_Entities.Type_Double_NegInfinity))]
        [DataRow(double.NaN, nameof(Localized_Base_Entities.Type_Double_NaN))]
        public void ToStringLocalizedDataTest(double value, string localizedKey)
        {
            base.ToStringDataTest(value, Localized_Base_Entities.ResourceManager.GetString(localizedKey));
        }
    }
}