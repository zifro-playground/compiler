using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsBinary(this OperatorCode opCode)
        {
            return opCode < OperatorCode.ANeg;
        }

        public static bool IsUnary(this OperatorCode opCode)
        {
            return opCode >= OperatorCode.ANeg;
        }
    }
}