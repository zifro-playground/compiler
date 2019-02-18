namespace Mellis.Core.Interfaces
{
    public interface ICompiler
    {
        IProcessor Compile(string code);
    }
}