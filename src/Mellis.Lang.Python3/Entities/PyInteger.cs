using System;
using System.Globalization;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Tools;

namespace Mellis.Lang.Python3.Entities
{
    public class PyInteger : ScriptInteger
    {
        public PyInteger(IProcessor processor, int value)
            : base(processor, value)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyIntegerType(Processor);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override IScriptType ArithmeticExponent(IScriptType rhs)
        {
            switch (rhs)
            {
            case ScriptInteger i when i.Value >= 0:
                // Try integer exponentiation
                return Processor.Factory.Create(MathUtilities.Pow(Value, i.Value));
            case ScriptInteger i:
                return Processor.Factory.Create(Math.Pow(Value, i.Value));
            case ScriptDouble d:
                return Processor.Factory.Create(Math.Pow(Value, d.Value));
            default:
                return null;
            }
        }

        public override IScriptType ArithmeticExponentReverse(IScriptType lhs)
        {
            switch (lhs)
            {
            case ScriptInteger i when Value >= 0:
                // Try integer exponentiation
                return Processor.Factory.Create(MathUtilities.Pow(i.Value, Value));
            case ScriptInteger i:
                return Processor.Factory.Create(Math.Pow(i.Value, Value));
            case ScriptDouble d:
                return Processor.Factory.Create(Math.Pow(d.Value, Value));
            default:
                return null;
            }
        }
    }
}