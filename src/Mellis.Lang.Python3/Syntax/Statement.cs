using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax
{
    public abstract class Statement : SyntaxNode
    {
        protected Statement(SourceReference source)
            : base(source)
        {
        }
    }
}