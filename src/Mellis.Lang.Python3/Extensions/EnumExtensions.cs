using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsBinary(this BasicOperatorCode opCode)
        {
            return opCode < BasicOperatorCode.ANeg;
        }

        public static bool IsUnary(this BasicOperatorCode opCode)
        {
            return opCode >= BasicOperatorCode.ANeg;
        }
    }
}