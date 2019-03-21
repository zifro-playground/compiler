using System;
using System.Globalization;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
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
                        try
                        {
                            return new LiteralInteger(source, ParseWithNonZeroBase(text.Substring(2), 2));
                        }
                        catch
                        {
                            throw new SyntaxLiteralFormatException(source);
                        }

                    case 'O':
                    case 'o':
                        // octinteger     ::=  "0" ("o" | "O") octdigit+
                        try
                        {
                            return new LiteralInteger(source, ParseWithNonZeroBase(text.Substring(2), 8));
                        }
                        catch
                        {
                            throw new SyntaxLiteralFormatException(source);
                        }

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

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Int_Name;
        }

        public override IScriptType ToScriptType(VM.PyProcessor processor)
        {
            return new PyInteger(processor, Value);
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral(this));
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public static int ParseWithNonZeroBase(string text, int numBase)
        {

            const string fullCharset = "0123456789abcdefghijklmnopqrstuvwxyz";

            if (numBase > fullCharset.Length || numBase < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(numBase), numBase, "2 <= numBase <= 36");
            }

            string charset = fullCharset.Substring(0, numBase);
            string trimmed = text.Trim().ToLowerInvariant();

            // Empty string?
            if (trimmed.Length == 0)
            {
                throw new ArgumentException("String is empty", nameof(text));
            }

            string withoutSign;
            bool positive;

            switch (trimmed[0])
            {
                case '+':
                    positive = true;
                    withoutSign = trimmed.Substring(1);
                    break;
                case '-':
                    positive = false;
                    withoutSign = trimmed.Substring(1);
                    break;
                default:
                    positive = true;
                    withoutSign = trimmed;
                    break;
            }

            var output = 0;
            foreach (char c in withoutSign)
            {
                int digit = charset.IndexOf(c);
                if (digit == -1)
                {
                    throw new FormatException($"Invalid char '{c}' for base {numBase}");
                }

                checked
                {
                    output *= numBase;
                    output += positive
                        ? +digit
                        : -digit;
                }
            }

            return output;
        }
    }
}