using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax
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