using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax
{
    public abstract class ExpressionNode : SyntaxNode
    {
        protected ExpressionNode(SourceReference source)
            : base(source)
        {
        }
    }
}