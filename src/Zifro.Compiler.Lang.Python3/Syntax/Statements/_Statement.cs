using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public abstract class Statement : SyntaxNode
    {
        protected Statement(SourceReference source, string sourceText)
            : base(source, sourceText)
        {
        }
    }
}