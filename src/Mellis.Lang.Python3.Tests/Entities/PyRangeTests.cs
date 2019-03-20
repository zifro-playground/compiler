using System;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public class PyRangeTests : BaseEntityTester<PyRange>
    {
        protected override string ExpectedTypeName => Localized_Python3_Entities.Type_Range_Name;

        protected override Type ExpectedTypeDef => typeof(PyRangeType);

        protected override PyRange CreateEntity(PyProcessor processor)
        {
            return new PyRange(processor, nameof(PyRangeTests));
        }
    }
}