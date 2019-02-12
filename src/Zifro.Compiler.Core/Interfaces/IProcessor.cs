namespace Zifro.Compiler.Core.Interfaces
{
    public interface IProcessor
    {
        IScriptTypeFactory Factory { get; }
        IScopeContext Globals { get; }

        void ContinueYieldedValue(IScriptType value);
        void Walk();
    }
}