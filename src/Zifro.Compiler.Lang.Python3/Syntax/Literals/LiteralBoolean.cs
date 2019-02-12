using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;
using Zifro.Compiler.Lang.Python3.Exceptions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralBoolean : Literal<bool>
    {
        public LiteralBoolean(SourceReference source, bool value)
            : base(source, value)
        {
        }

        public static LiteralBoolean Parse(SourceReference source, string text)
        {
            switch (text)
            {
                case "True":
                    return new LiteralBoolean(source, true);
                case "False":
                    return new LiteralBoolean(source, false);

                default:
                    throw new SyntaxLiteralFormatException(source);
            }
        }

        public override IScriptType ToScriptType(PyProcessor processor)
        {
            return new PyBoolean(processor, Value);
        }
    }
}