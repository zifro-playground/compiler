using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Extensions
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