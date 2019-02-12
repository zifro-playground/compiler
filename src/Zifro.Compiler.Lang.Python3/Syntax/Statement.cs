using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public abstract class Statement : SyntaxNode
    {
        protected Statement(SourceReference source)
            : base(source)
        {
        }
    }
}