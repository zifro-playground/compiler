using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralBoolean : Literal<bool>
    {
        public LiteralBoolean(SourceReference source, bool value)
            : base(source, value)
        {
        }
    }
}