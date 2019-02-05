using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public abstract class SyntaxNode
    {
        protected SyntaxNode(SourceReference source, string sourceText)
        {
            Source = source;
            SourceText = sourceText;
        }

        public SourceReference Source { get; }
        public string SourceText { get; }
    }
}