using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public abstract class SyntaxNode
    {
        protected SyntaxNode(SourceReference source)
        {
            Source = source;
        }

        public SourceReference Source { get; }

        public abstract void Compile(PyCompiler compiler);
    }
}