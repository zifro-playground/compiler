using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Literals
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

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Boolean_Name;
        }

        public override IScriptType ToScriptType(VM.PyProcessor processor)
        {
            return new PyBoolean(processor, Value);
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral(this));
        }

        public override string ToString()
        {
            return Value ? "True" : "False";
        }
    }
}