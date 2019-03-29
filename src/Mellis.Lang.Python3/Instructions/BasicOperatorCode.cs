namespace Mellis.Lang.Python3.Instructions
{
    public enum BasicOperatorCode
    {
        /*
         * Binary arithmetic operators (lhs op rhs)
         */
        /// <summary>a+b</summary>
        AAdd = 0x0,
        /// <summary>a-b</summary>
        ASub,
        /// <summary>a*b</summary>
        AMul,
        /// <summary>a/b</summary>
        ADiv,
        /// <summary>a//b</summary>
        AFlr,
        /// <summary>a%b</summary>
        AMod,
        /// <summary>a**b</summary>
        APow,

        /*
         * Binary binary operators (lhs op rhs)
         */
        /// <summary>a&amp;b</summary>
        BAnd = 0x8,
        /// <summary>a&lt;&lt;b</summary>
        BLsh,
        /// <summary>a&gt;&gt;b</summary>
        BRsh,
        /// <summary>a|b</summary>
        BOr,
        /// <summary>a^b</summary>
        BXor,

        /*
         * Binary comparison operators (lhs op rhs)
         */
        /// <summary>a==b</summary>
        CEq = 0x10,
        /// <summary>a!=b</summary>
        CNEq,
        /// <summary>a&gt;b</summary>
        CGt,
        /// <summary>a&gt;=b</summary>
        CGtEq,
        /// <summary>a&lt;b</summary>
        CLt,
        /// <summary>a&lt;=b</summary>
        CLtEq,

        /*
         * Binary identity comparison operators (lhs op rhs)
         */
        /// <summary>a in b</summary>
        CIn = 0x18,
        /// <summary>a not in b</summary>
        CNIn,
        /// <summary>a is b</summary>
        CIs,
        /// <summary>a is not b</summary>
        CIsN,

        /*
         * Unary operators (op rhs)
         */
        /// <summary>+a</summary>
        ANeg = 0x28,
        /// <summary>-a</summary>
        APos,
        /// <summary>~a</summary>
        BNot,
        /// <summary>not a</summary>
        LNot,
    }
}