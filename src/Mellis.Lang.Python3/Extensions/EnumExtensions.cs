using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Is the operator a binary operator? I.e two operands.
        /// <para>Example: a + b</para>
        /// </summary>
        public static bool IsBinary(this BasicOperatorCode opCode)
        {
            return opCode < BasicOperatorCode.ANeg ||
                   opCode >= BasicOperatorCode.IAAdd;
        }

        /// <summary>
        /// Is the operator a unary operator? I.e only one operand.
        /// <para>Example: ~a</para>
        /// </summary>
        public static bool IsUnary(this BasicOperatorCode opCode)
        {
            return opCode >= BasicOperatorCode.ANeg &&
                   opCode < BasicOperatorCode.IAAdd;
        }

        /// <summary>
        /// Is the operator a "in-place" operator?
        /// <para>Example: a += b</para>
        /// </summary>
        public static bool IsInPlace(this BasicOperatorCode opCode)
        {
            return opCode >= BasicOperatorCode.IAAdd;
        }
    }
}