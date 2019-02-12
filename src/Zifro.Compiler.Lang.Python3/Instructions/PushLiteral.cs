using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class PushLiteral<TValue> : IOpCode
    {
        public PushLiteral(Literal<TValue> literal)
        {
            Literal = literal;
        }

        public Literal<TValue> Literal { get; }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"add ${Literal}";
        }
    }
}