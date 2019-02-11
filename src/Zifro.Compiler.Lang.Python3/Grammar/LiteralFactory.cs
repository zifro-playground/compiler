using System.Globalization;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax;
using Zifro.Compiler.Lang.Python3.Syntax.Literals;

namespace Zifro.Compiler.Lang.Python3.Grammar
{
    public static class LiteralFactory
    {
        public static LiteralBoolean CreateBoolean(SourceReference source, string text)
        {
            switch (text)
            {
                case "True":
                    return new LiteralBoolean(source, true);
                case "False":
                    return new LiteralBoolean(source, false);

                default:
                    throw SyntaxFormatException(source);
            }
        }

        public static ExpressionNode CreateNumber(SourceReference source, string text)
        {
            if (text.Length > 2 && text[0] == '0')
            {
                switch (text[1])
                {
                    case 'b':
                        throw new SyntaxNotYetImplementedException(source);
                    case 'o':
                        throw new SyntaxNotYetImplementedException(source);

                    case 'x' when int.TryParse(text.Substring(2),
                        NumberStyles.AllowHexSpecifier,
                        CultureInfo.InvariantCulture,
                        out int hexValue):
                        return new LiteralInteger(source, hexValue);

                    case 'x':
                        throw new SyntaxNotYetImplementedException(source);
                }
            }

            if (int.TryParse(text, out int integer))
            {
                return new LiteralInteger(source, integer);
            }

            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
            {
                return new LiteralDouble(source, doubleValue);
            }

            throw SyntaxFormatException(source);
        }

        private static SyntaxException SyntaxFormatException(in SourceReference source)
        {
            return new SyntaxException(source,
                nameof(Localized_Python3_Parser.Ex_Literal_Format),
                Localized_Python3_Parser.Ex_Literal_Format);
        }

        private static SyntaxNotYetImplementedException NotYetImplementedException(in SourceReference source)
        {
            return new SyntaxNotYetImplementedException(source);
        }
    }
}