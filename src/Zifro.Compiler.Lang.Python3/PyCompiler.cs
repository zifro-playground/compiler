using Antlr4.Runtime;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Grammar;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyCompiler : ICompiler
    {
        public IProcessor Compile(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}