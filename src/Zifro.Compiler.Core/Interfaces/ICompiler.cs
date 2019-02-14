namespace Zifro.Compiler.Core.Interfaces
{
    public interface ICompiler
    {
        IProcessor Compile(string code);
    }
}