using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public abstract class ExpressionNode : SyntaxNode
    {
        protected ExpressionNode(SourceReference source)
            : base(source)
        {
        }
    }
}