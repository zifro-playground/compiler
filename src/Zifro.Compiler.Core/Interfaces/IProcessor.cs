namespace Zifro.Compiler.Core.Interfaces
{
    public interface IProcessor
    {
        IScriptTypeFactory Factory { get; }
        IScopeContext GlobalScope { get; }
        IScopeContext CurrentScope { get; }

        void ContinueYieldedValue(IScriptType value);
        void WalkLine();
    }
}