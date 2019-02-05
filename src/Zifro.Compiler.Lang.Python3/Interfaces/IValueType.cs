namespace Zifro.Compiler.Lang.Python3.Interfaces
{
    public interface IValueType
    {
        /// <summary>
        /// Clones this value. Used for integers, floats, strings, etc.
        /// </summary>
        IValueType Clone();
    }
}