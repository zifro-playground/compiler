using System;
using System.Threading;

namespace Mellis.Core.Interfaces
{
    /// <summary>
    /// Used as value throughout Mellis.
    /// </summary>
    public interface IScriptType
    {
        /// <summary>
        /// The assigned processor environment.
        /// </summary>
        IProcessor Processor { get; }

        /// <summary>
        /// Name of this value.
        /// Can be function name, variable name, or parameter name.
        /// Null if undetermined (ex: equation result and literals)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a copy of this value with new name.
        /// Used internally by the processor on variable assignment.
        /// </summary>
        IScriptType Copy(string newName);

        /// <summary>
        /// <para>(Lua, Python) <c>type(this)</c></para>
        /// <para>(JavaScript) <c>typeof(this)</c></para>
        /// </summary>
        IScriptType GetTypeDef();

        /// <summary>
        /// Name of this type. Used in error messages.
        /// Should be localized to the current UI culture set via <seealso cref="Thread.CurrentUICulture"/>.
        /// </summary>
        string GetTypeName();

        /// <summary>
        /// For use in <c>and</c> &amp; <c>or</c> operators, as well as <c>if</c> and <c>while</c> statements.
        /// <para>As <c>and</c> &amp; <c>or</c> operators short-circuit depending on a values truthy'ness.</para>
        /// </summary>
        bool IsTruthy();

        /// <summary>
        /// this[<paramref name="index"/>]
        /// </summary>
        IScriptType GetIndex(IScriptType index);

        /// <summary>
        /// this[<paramref name="index"/>] = <paramref name="value"/>
        /// </summary>
        IScriptType SetIndex(IScriptType index, IScriptType value);

        /// <summary>
        /// this.<paramref name="property"/>
        /// </summary>
        IScriptType GetProperty(string property);

        /// <summary>
        /// this.<paramref name="property"/> = <paramref name="value"/>
        /// </summary>
        IScriptType SetProperty(string property, IScriptType value);

        /// <summary>
        /// Try convert this value to it's CLR representation.
        /// </summary>
        bool TryCoerce<T>(out T value);

        /// <summary>
        /// Try convert this value to it's CLR representation.
        /// </summary>
        bool TryCoerce(Type type, out object value);

        #region Arithmetic operators

        /// <summary>
        /// (Python, JavaScript) +this
        /// </summary>
        IScriptType ArithmeticUnaryPositive();

        /// <summary>
        /// -this
        /// </summary>
        IScriptType ArithmeticUnaryNegative();

        /// <summary>
        /// this + <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticAdd(IScriptType rhs);

        /// <summary>
        /// this - <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticSubtract(IScriptType rhs);

        /// <summary>
        /// this * <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticMultiply(IScriptType rhs);

        /// <summary>
        /// this / <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticDivide(IScriptType rhs);

        /// <summary>
        /// this % <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticModulus(IScriptType rhs);

        /// <summary>
        /// <para>(Python) this ** <paramref name="rhs"/></para>
        /// <para>(Lua) this ^ <paramref name="rhs"/></para>
        /// </summary>
        IScriptType ArithmeticExponent(IScriptType rhs);

        /// <summary>
        /// (Python, Lua) this // <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticFloorDivide(IScriptType rhs);

        #endregion

        #region Comparison operators

        /// <summary>
        /// this == <paramref name="rhs"/>
        /// </summary>
        IScriptType CompareEqual(IScriptType rhs);

        /// <summary>
        /// <para>(Lua) this ~= <paramref name="rhs"/></para>
        /// <para>(JavaScript, Python) this != <paramref name="rhs"/></para>
        /// <para>(Python) this &lt;&gt; <paramref name="rhs"/></para>
        /// </summary>
        IScriptType CompareNotEqual(IScriptType rhs);

        /// <summary>
        /// this &gt; <paramref name="rhs"/>
        /// <para>In Lua this is ignored.
        /// Greater than is evaluated via reversing the order of <see cref="CompareLessThan"/></para>
        /// </summary>
        IScriptType CompareGreaterThan(IScriptType rhs);

        /// <summary>
        /// this &gt;= <paramref name="rhs"/>
        /// <para>In Lua this is ignored.
        /// Greater than is evaluated via reversing the order of <see cref="CompareLessThanOrEqual"/></para>
        /// </summary>
        IScriptType CompareGreaterThanOrEqual(IScriptType rhs);

        /// <summary>
        /// this &lt; <paramref name="rhs"/>
        /// </summary>
        IScriptType CompareLessThan(IScriptType rhs);

        /// <summary>
        /// this &lt;= <paramref name="rhs"/>
        /// </summary>
        IScriptType CompareLessThanOrEqual(IScriptType rhs);

        #endregion

        #region Bitwise operators

        /// <summary>
        /// ~this
        /// </summary>
        IScriptType BinaryNot();

        /// <summary>
        /// this &amp; <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryAnd(IScriptType rhs);

        /// <summary>
        /// this | <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryOr(IScriptType rhs);

        /// <summary>
        /// <para>(Python, JavaScript) this ^ <paramref name="rhs"/></para>
        /// <para>(Lua) this ~ <paramref name="rhs"/></para>
        /// </summary>
        IScriptType BinaryXor(IScriptType rhs);

        /// <summary>
        /// this &lt;&lt; <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryLeftShift(IScriptType rhs);

        /// <summary>
        /// this &gt;&gt; <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryRightShift(IScriptType rhs);

        #endregion

        #region Member operators

        /// <summary>
        /// (JavaScript, Python) <paramref name="lhs"/> in this
        /// </summary>
        IScriptType MemberIn(IScriptType lhs);

        /// <summary>
        /// (Python) <paramref name="lhs"/> not in this
        /// </summary>
        IScriptType MemberNotIn(IScriptType lhs);

        #endregion

        #region Identity operators

        /// <summary>
        /// (Python) this is <paramref name="rhs"/>
        /// (JavaScript) this instanceof <paramref name="rhs"/>
        /// </summary>
        IScriptType IdentityIs(IScriptType rhs);

        /// <summary>
        /// (Python) this is not <paramref name="rhs"/>
        /// </summary>
        IScriptType IdentityIsNot(IScriptType rhs);

        #endregion
    }
}
