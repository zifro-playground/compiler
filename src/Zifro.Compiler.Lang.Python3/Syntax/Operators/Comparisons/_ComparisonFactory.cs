using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons
{
    public class ComparisonFactory : SyntaxNode
    {
        public ComparisonType Type { get; set; }

        public ComparisonFactory(SourceReference source, ComparisonType type)
            : base(source)
        {
            Type = type;
        }
    }
}