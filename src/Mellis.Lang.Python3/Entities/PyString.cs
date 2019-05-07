using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities
{
    public class PyString : StringBase, IEnumerable<IScriptType>
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