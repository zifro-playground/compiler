using System;
using System.Globalization;
using System.Text;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Literals
{
    public class LiteralString : Literal<string>
    {
        public LiteralString(SourceReference source, string value)
            : base(source, value)
        {
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_String_Name;
        }

        public override IScriptType ToScriptType(VM.PyProcessor processor)
        {
            return new PyString(processor, Value);
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral<string>(this));
        }

        public override string ToString()
        {
            return Escape(Value);
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
                {
                    throw new SyntaxLiteralFormatException(source);
                }

                char quote = withoutModifiers[0];
                string longStringQuotes = new string(quote, 3);
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
                    {
                        return builder.ToString();
                    }

                    builder.Append(c);
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Replaces escaped characters with the actual character.
        /// <para>NOTE: This is not the reverse of <see cref="Escape"/>. This does not remove the surrounding quotes created by <see cref="Escape"/>.</para>
        /// </summary>
        public static string Unescape(string value)
        {
            var builder = new StringBuilder(value.Length);

            for (int i = 0; i < value.Length; i++)
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
                    case '\\':
                        c = '\\';
                        break;
                    case '\'':
                        c = '\'';
                        break;
                    case '"':
                        c = '"';
                        break;
                    case 'a':
                        c = '\a';
                        break;
                    case 'b':
                        c = '\b';
                        break;
                    case 'f':
                        c = '\f';
                        break;
                    case 'n':
                        c = '\n';
                        break;
                    case 'r':
                        c = '\r';
                        break;
                    case 't':
                        c = '\t';
                        break;
                    case 'v':
                        c = '\v';
                        break;
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
                    {
                        b.Append(v);
                    }
                    else
                    {
                        break;
                    }
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
                        if ((v >= '0' && v <= '9') ||
                            (v >= 'a' && v <= 'f') ||
                            (v >= 'A' && v <= 'F'))
                        {
                            b.Append(v);
                        }
                        else
                        {
                            break;
                        }
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

        public static string Escape(string value)
        {
            // Algorithm ported from
            // https://github.com/python/cpython/blob/3.7/Objects/unicodeobject.c#L12609

            // Compute quote characters
            bool singleQuotes = value.IndexOf('\'') != -1;

            // Prefer single quotes
            char quote = !singleQuotes ? '\'' : '"';
            var builder = new StringBuilder(value.Length + 10);

            builder.Append(quote);

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                switch (c)
                {
                    // Escape quotes and backslashes
                    case '\'' when quote == '\'':
                    case '"' when quote == '"':
                    case '\\':
                        builder.Append('\\');
                        builder.Append(c);
                        break;

                    // Map special whitespace to '\t', \n', '\r' 
                    case '\a':
                        builder.Append("\\a");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    case '\v':
                        builder.Append("\\v");
                        break;

                    default:
                        // Map non-printable US ASCII to '\xhh'
                        if (c < ' ' || c == '\x7F')
                        {
                            builder.AppendFormat("\\x{0:x2}", (int) c);
                        }
                        // Copy ASCII characters as-is
                        else if (c < '\x7F')
                        {
                            builder.Append(c);
                        }
                        // Non-ASCII characters
                        else
                        {
                            if (!IsPrintable(value, i))
                            {
                                // Map 8-bit characters to '\xhh'
                                if (c <= '\xff')
                                {
                                    builder.AppendFormat("\\x{0:x2}", (int) c);
                                }
                                else
                                {
                                    int c32 = char.ConvertToUtf32(c, value[++i]);
                                    // Map 16 - bit characters to '\uxxxx'
                                    if (c32 <= 0xffff)
                                    {
                                        builder.AppendFormat("\\u{0:x4}", c32);
                                    }
                                    // Map 21-bit characters to '\U00xxxxxx'
                                    else
                                    {
                                        builder.AppendFormat("\\U{0:x8}", c32);
                                    }
                                }
                            }
                            else if (char.IsSurrogatePair(value, i))
                            {
                                // special because c# only handles 16bit in char
                                builder.Append(value.Substring(i, 2));
                                i++;
                            }
                            else
                            {
                                // Copy characters as-is
                                builder.Append(c);
                            }
                        }

                        break;
                } // switch
            } // for

            // Closing quote already added at the beginning
            builder.Append(quote);

            return builder.ToString();
        }

        private static bool IsPrintable(string value, int index)
        {
            // According to cpython/Objects/unicodectype.c
            // https://github.com/python/cpython/blob/3.7/Objects/unicodectype.c#L147
            switch (char.GetUnicodeCategory(value, index))
            {
                case UnicodeCategory.Control:
                case UnicodeCategory.Format:
                case UnicodeCategory.Surrogate:
                case UnicodeCategory.PrivateUse:
                case UnicodeCategory.OtherNotAssigned:
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                case UnicodeCategory.SpaceSeparator:
                    return false;

                default:
                    return true;
            }
        }
    }
}