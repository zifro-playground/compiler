using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Python3.Entities
{
    public class PyRange : ScriptTypeBase
    {
        public int RangeFrom { get; }
        public int RangeTo { get; }
        public int RangeStep { get; }

        public PyRange(
            IProcessor processor,
            int rangeFrom,
            int rangeTo,
            int rangeStep = 1,
            string name = null)
            : base(processor, name)
        {
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            RangeStep = rangeStep;
        }

        public override IScriptType Copy(string newName)
        {
            throw new NotImplementedException();
        }

        public override IScriptType GetTypeDef()
        {
            throw new NotImplementedException();
        }

        public override string GetTypeName()
        {
            throw new NotImplementedException();
        }

        public override bool IsTruthy()
        {
            throw new NotImplementedException();
        }

        public override bool TryConvert(Type type, out object value)
        {
            throw new NotImplementedException();
        }
    }
}