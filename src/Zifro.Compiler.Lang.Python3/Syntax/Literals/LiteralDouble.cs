using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Literals
{
    public class LiteralDouble : Literal<double>
    {
        public LiteralDouble(SourceReference source, double value)
            : base(source, value)
        {
        }
    }
}