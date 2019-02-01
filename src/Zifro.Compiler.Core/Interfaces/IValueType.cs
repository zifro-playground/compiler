namespace Zifro.Compiler.Core.Interfaces
{
    public interface IValueType
    {
        /// <summary>
        /// Clones this value. Used for integers, floats, strings, etc.
        /// </summary>
        IValueType Clone();
    }
}