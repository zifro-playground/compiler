﻿using System.Text;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities
{
    public class PyString : StringBase
    {
        public PyString(IProcessor processor, string value, string name = null)
            : base(processor, value, name)
        {
        }

        /// <inheritdoc />
        public override IScriptType Copy(string newName)
        {
            return new PyString(Processor, Value, newName);
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            return new PyStringType(Processor);
        }

        public override IScriptType ArithmeticMultiply(IScriptType rhs)
        {
            switch (rhs)
            {
                case IntegerBase rhsInteger when rhsInteger.Value <= 0:
                    return Processor.Factory.Create(string.Empty);

                case IntegerBase rhsInteger:
                    var builder = new StringBuilder(Value.Length * rhsInteger.Value);
                    for (var i = 0; i < rhsInteger.Value; i++)
                    {
                        builder.Append(Value);
                    }

                    return Processor.Factory.Create(builder.ToString());

                default:
                    throw InvalidType(rhs, "*");
            }
        }

        public override string ToString()
        {
            return LiteralString.Escape(Value);
        }
    }
}