using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralInteger : Literal<int>
    {
        public LiteralInteger(SourceReference source, string sourceText, int value)
            : base(source, sourceText, value)
        {
        }
    }
}