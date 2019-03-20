using System;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Tests.Entities
{
    public class PyRangeTests : BaseEntityTester<PyRange, (int from, int to, int step)>
    {
        protected override string ExpectedTypeName
            => Localized_Python3_Entities.Type_Range_Name;

        protected override Type ExpectedTypeDef
            => typeof(PyRangeType);

        protected override (int from, int to, int step) DefaultValue
            => (from: 0, to: 10, step: 1);

        protected override PyRange CreateEntity(PyProcessor processor, (int from, int to, int step) value)
        {
            (int from, int to, int step) = value;
            return new PyRange(processor, from, to, step, nameof(PyRangeTests));
        }
    }
}