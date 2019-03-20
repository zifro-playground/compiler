using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Entities.Classes
{
    public class PyRangeTypeTests : BaseEntityTypeTester<PyRangeType, PyRange>
    {
        protected override string ExpectedClassName => Localized_Python3_Entities.Type_Range_Name;

        protected override PyRangeType CreateEntity(PyProcessor processor)
        {
            return new PyRangeType(processor, nameof(PyRangeTypeTests));
        }
    }
}