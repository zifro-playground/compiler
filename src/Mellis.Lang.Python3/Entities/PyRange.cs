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
        public IScriptInteger RangeFrom { get; }
        public IScriptInteger RangeTo { get; }
        public IScriptInteger RangeStep { get; }

        public PyRange(
            IProcessor processor,
            IScriptInteger rangeFrom,
            IScriptInteger rangeTo,
            IScriptInteger rangeStep)
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

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            if (rhs is PyRange range &&
                range.RangeFrom == RangeFrom &&
                range.RangeTo == RangeTo &&
                range.RangeStep == RangeStep)
            {
                return Processor.Factory.True;
            }

            return Processor.Factory.False;
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            if (!(rhs is PyRange range) ||
                range.RangeFrom != RangeFrom ||
                range.RangeTo != RangeTo ||
                range.RangeStep != RangeStep)
            {
                return Processor.Factory.True;
            }

            return Processor.Factory.False;
        }

        public IEnumerator<IScriptType> GetEnumerator()
        {
            if (RangeStep.Value > 0)
            {
                for (int i = RangeFrom.Value; i < RangeTo.Value; i += RangeStep.Value)
                {
                    yield return Processor.Factory.Create(i);
                }
            }

            if (RangeStep.Value < 0)
            {
                for (int i = RangeFrom.Value; i > RangeTo.Value; i += RangeStep.Value)
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
            if (RangeStep.Value == 1)
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