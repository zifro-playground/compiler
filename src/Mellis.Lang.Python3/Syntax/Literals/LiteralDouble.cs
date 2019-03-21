using System.Globalization;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Literals
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

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Double_Name;
        }

        public override IScriptType ToScriptType(VM.PyProcessor processor)
        {
            return new PyDouble(processor, Value);
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral(this));
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}