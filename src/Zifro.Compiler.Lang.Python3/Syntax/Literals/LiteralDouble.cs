using System.Globalization;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralDouble : Literal<double>
    {
        public LiteralDouble(SourceReference source, double value)
            : base(source, value)
        {
        }

        public static LiteralDouble Parse(SourceReference source, string text)
        {
            // floatnumber   ::=  pointfloat | exponentfloat

            if (double.TryParse(text,
                NumberStyles.AllowExponent |
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,  out double value))
            {
                return new LiteralDouble(source, value);
            }

            throw new SyntaxLiteralFormatException(source);
        }

        public override IScriptType ToScriptType(PyProcessor processor)
        {
            return new PyDouble(processor, Value);
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral<double>(this));
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}