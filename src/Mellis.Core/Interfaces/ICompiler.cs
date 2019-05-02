using Mellis.Core.Entities;

namespace Mellis.Core.Interfaces
{
    public interface ICompiler
    {
        CompilerSettings Settings { get; set; }

        IProcessor Compile(string code);
    }
}