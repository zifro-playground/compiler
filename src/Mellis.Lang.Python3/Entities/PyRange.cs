using System;
using System.Collections;
using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyRange : ScriptType, IEnumerable<IScriptType>
    {
        public int RangeFrom { get; }
        public int RangeTo { get; }
        public int RangeStep { get; }

        public PyRange(
            IProcessor processor,
            int rangeFrom,
            int rangeTo,
            int rangeStep = 1)
            : base(processor)
        {
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;
            RangeStep = rangeStep;
        }

        public override IScriptType GetTypeDef()
        {
            return new PyRangeType(Processor);
        }

        public override string GetTypeName()
        {
            return Localized_Python3_Entities.Type_Range_Name;
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public IEnumerator<IScriptType> GetEnumerator()
        {
            if (RangeStep > 0)
            {
                for (int i = RangeFrom; i < RangeTo; i += RangeStep)
                {
                    yield return Processor.Factory.Create(i);
                }
            }

            if (RangeStep < 0)
            {
                for (int i = RangeFrom; i > RangeTo; i += RangeStep)
                {
                    yield return Processor.Factory.Create(i);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            if (RangeStep == 1)
            {
                return string.Format(
                    Localized_Python3_Entities.Type_Range_ToString,
                    RangeFrom, RangeTo
                );
            }

            return string.Format(
                Localized_Python3_Entities.Type_Range_ToString_Step,
                RangeFrom, RangeTo, RangeStep
            );
        }
    }
}