namespace Zifro.Compiler.Core.Interfaces
{
    public interface IProcessor
    {
        IValueTypeFactory Factory { get; }
        IScopeContext TopScope { get; }

        void ContinueYieldedValue(IValueType value);
        void Walk();
    }
}