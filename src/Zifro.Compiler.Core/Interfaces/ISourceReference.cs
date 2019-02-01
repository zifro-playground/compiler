namespace Zifro.Compiler.Core.Interfaces
{
    public interface ISourceReference
    {
        /// <summary>
        /// Is the source defined in the Common Language Runtime?
        /// </summary>
        bool IsFromClr { get; }

        int FromRow { get; }
        int ToRow { get; }
        int FromColumn { get; }
        int ToColumn { get; }
    }
}