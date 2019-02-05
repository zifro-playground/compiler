using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralBoolean : Literal<bool>
    {
        public LiteralBoolean(SourceReference source, string sourceText, bool value)
            : base(source, sourceText, value)
        {
        }
    }
}