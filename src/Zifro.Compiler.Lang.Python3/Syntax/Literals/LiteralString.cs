using System;
using System.Text;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Exceptions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralString : Literal<string>
    {
        public LiteralString(SourceReference source, string value)
            : base(source, value)
        {
        }

        public override IScriptType ToScriptType(PyProcessor processor)
        {
            return new PyString(processor, Value);
        }

        public static LiteralString Parse(SourceReference source, string text)
        {
            // At least 2 for an empty quoted string
            if (text.Length < 2)
            {
                throw new SyntaxLiteralFormatException(source);
            }

            string modifiers = GetModifiers();
            string withoutModifiers = text.Substring(modifiers.Length);
            string quotes = GetQuotes();

            // Check it's longer than quotes
            if (withoutModifiers.Length < quotes.Length * 2)
            {
                throw new SyntaxLiteralFormatException(source);
            }

            // Check ending with correct quotes
            if (withoutModifiers.Substring(withoutModifiers.Length - quotes.Length) != quotes)
            {
                throw new SyntaxLiteralFormatException(source);
            }

            string withoutQuotes = withoutModifiers.Substring(quotes.Length,
                withoutModifiers.Length - quotes.Length * 2);
            switch (modifiers.ToLowerInvariant())
            {
                case "":
                    return new LiteralString(source, Unescape(withoutQuotes));

                case "r": // raw
                    return new LiteralString(source, withoutQuotes);

                case "b": // bytes
                case "br": // raw+bytes
                case "rb": // raw+bytes
                case "f": // formatted
                case "rf": // raw+formatted
                case "fr": // raw+formatted
                case "u": // unicode
                    throw new SyntaxNotYetImplementedException(source);

                default:
                    throw new SyntaxLiteralFormatException(source);
            }

            string GetQuotes()
            {
                if (withoutModifiers.Length == 0)
                    throw new SyntaxLiteralFormatException(source);

                char quote = withoutModifiers[0];
                var longStringQuotes = new string(quote, 3);
                return withoutModifiers.StartsWith(longStringQuotes)
                    ? longStringQuotes
                    : quote.ToString();
            }

            string GetModifiers()
            {
                var builder = new StringBuilder();

                foreach (char c in text)
                {
                    if (c == '"' || c == '\'')
                        return builder.ToString();

                    builder.Append(c);
                }

                return builder.ToString();
            }
        }

        public static string Unescape(string value)
        {
            var builder = new StringBuilder(value.Length);

            for (var i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (c == '\\')
                {
                    if (i + 1 >= value.Length)
                    {
                        builder.Append(value[i]);
                        continue;
                    }

                    if (GetEscapedChar(i + 1, out c))
                    {
                        builder.Append(c);
                        i++;
                    }
                    else if (GetOctalChars(i, out string oStr))
                    {
                        builder.Append((char) Convert.ToInt32(oStr, 8));
                        i += oStr.Length;
                    }
                    else if (GetHexChars(i, out string xStr))
                    {
                        builder.Append((char) Convert.ToInt32(xStr, 16));
                        i += xStr.Length + 1;
                    }
                    else
                    {
                        builder.Append(value[i]);
                        builder.Append(value[++i]);
                    }
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();

            bool GetEscapedChar(int i, out char c)
            {
                switch (value[i])
                {
                    case '\\': c = '\\'; break;
                    case '\'': c = '\''; break;
                    case '"': c = '"'; break;
                    case 'a': c = '\a'; break;
                    case 'b': c = '\b'; break;
                    case 'f': c = '\f'; break;
                    case 'n': c = '\n'; break;
                    case 'r': c = '\r'; break;
                    case 't': c = '\t'; break;
                    case 'v': c = '\v'; break;
                    default:
                        c = default;
                        return false;
                }
                return true;
            }

            bool GetOctalChars(int i, out string octString)
            {
                var b = new StringBuilder(3);

                // Skip the /
                i++;

                for (; i < value.Length && b.Length < 3; i++)
                {
                    char v = value[i];
                    if (v >= '0' && v <= '7')
                        b.Append(v);
                    else
                        break;
                }

                if (b.Length > 0)
                {
                    octString = b.ToString();
                    return true;
                }

                octString = default;
                return false;
            }

            bool GetHexChars(int i, out string hexStr)
            {
                var b = new StringBuilder(3);

                // Skip the /
                i++;

                if (value.Length > i && value[i] == 'x')
                {
                    // Skip the x
                    i++;

                    for (; i < value.Length && b.Length < 2; i++)
                    {
                        char v = value[i];
                        if (v >= '0' && v <= '9' ||
                            v >= 'a' && v <= 'f' ||
                            v >= 'A' && v <= 'F')
                            b.Append(v);
                        else
                            break;
                    }

                    if (b.Length > 0)
                    {
                        hexStr = b.ToString();
                        return true;
                    }
                }

                hexStr = default;
                return false;
            }
        }
    }
}