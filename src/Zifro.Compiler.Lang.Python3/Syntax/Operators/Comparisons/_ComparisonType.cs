namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons
{
    public enum ComparisonType
    {
        /// <summary>a <c>&lt;</c> b</summary>
        LessThan,

        /// <summary>a <c>&lt;=</c> b</summary>
        LessThanOrEqual,

        /// <summary>a <c>&gt;</c> b</summary>
        GreaterThan,

        /// <summary>a <c>&gt;=</c> b</summary>
        GreaterThanOrEqual,

        /// <summary>a <c>==</c> b</summary>
        Equals,

        /// <summary>a <c>!=</c> b</summary>
        NotEquals,

        /// <summary>
        /// a <c>&lt;&gt;</c> b
        /// <para>
        /// Unsupported syntax, but included in the grammar. Comes from the ABC language <see href="https://homepages.cwi.nl/~steven/abc/qr.html#TESTS"/>
        /// </para>
        /// </summary>
        NotEqualsABC,

        /// <summary>a <c>in</c> b</summary>
        In,

        /// <summary>a <c>not in</c> b</summary>
        InNot,

        /// <summary>a <c>is</c> b</summary>
        Is,

        /// <summary>a <c>is not</c> b</summary>
        IsNot,
    }
}