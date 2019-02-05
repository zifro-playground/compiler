using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralString : Literal<string>
    {
        public LiteralString(SourceReference source, string sourceText, string value)
            : base(source, sourceText, value)
        {
        }
    }
}