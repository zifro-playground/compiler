using System.Globalization;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Literals
{
    public class LiteralInteger : Literal<int>
    {
        public LiteralInteger(SourceReference source, int value)
            : base(source, value)
        {
        }

        public static LiteralInteger Parse(SourceReference source, string text)
        {
            if (text.Length > 2 && text[0] == '0')
            {
                switch (text[1])
                {
                    case 'B':
                    case 'b':
                        // bininteger     ::=  "0" ("b" | "B") bindigit+
                        throw new SyntaxNotYetImplementedException(source);
                    case 'O':
                    case 'o':
                        // octinteger     ::=  "0" ("o" | "O") octdigit+
                        throw new SyntaxNotYetImplementedException(source);

                    case 'X':
                    case 'x':
                        // hexinteger     ::=  "0" ("x" | "X") hexdigit+
                        if (int.TryParse(text.Substring(2),
                            NumberStyles.AllowHexSpecifier,
                            CultureInfo.InvariantCulture,
                            out int hexValue))
                        {
                            return new LiteralInteger(source, hexValue);
                        }

                        throw new SyntaxLiteralFormatException(source);
                }
            }

            // decimalinteger ::=  nonzerodigit digit* | "0"+
            if (int.TryParse(text, out int integer))
            {
                return new LiteralInteger(source, integer);
            }

            throw new SyntaxLiteralFormatException(source);
        }

        public override IScriptType ToScriptType(PyProcessor processor)
        {
            return new PyInteger(processor, Value);
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral<int>(this));
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}