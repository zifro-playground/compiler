using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax
{
    public class Identifier : ExpressionNode
    {
        public string Name { get; }

        public Identifier(SourceReference source, string name) : base(source)
        {
            Name = name;
        }
    }
}