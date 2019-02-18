using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax
{
    public class Identifier : ExpressionNode
    {
        public string Name { get; }

        public Identifier(SourceReference source, string name) : base(source)
        {
            Name = name;
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new VarGet(Source, Name));
        }
    }
}