﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities
{
    public class PyString : ScriptString, IEnumerable<IScriptType>
    {
        public PyString(IProcessor processor, string value)
            : base(processor, value)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyStringType(Processor);
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
                case ScriptInteger rhsInteger when rhsInteger.Value <= 0:
                    return Processor.Factory.Create(string.Empty);

                case ScriptInteger rhsInteger:
                    var builder = new StringBuilder(Value.Length * rhsInteger.Value);
                    for (int i = 0; i < rhsInteger.Value; i++)
                    {
                        builder.Append(Value);
                    }

                    return Processor.Factory.Create(builder.ToString());

                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return LiteralString.Escape(Value);
        }

        public IEnumerator<IScriptType> GetEnumerator()
        {
            foreach (char c in Value)
            {
                yield return Processor.Factory.Create(c);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}