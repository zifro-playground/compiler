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
        /// <paramref name="lhs"/> + this
        /// </summary>
        IScriptType ArithmeticAddReverse(IScriptType lhs);

        /// <summary>
        /// this - <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticSubtract(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> - this
        /// </summary>
        IScriptType ArithmeticSubtractReverse(IScriptType lhs);

        /// <summary>
        /// this * <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticMultiply(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> * this
        /// </summary>
        IScriptType ArithmeticMultiplyReverse(IScriptType lhs);

        /// <summary>
        /// this / <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticDivide(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> / this
        /// </summary>
        IScriptType ArithmeticDivideReverse(IScriptType lhs);

        /// <summary>
        /// this % <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticModulus(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> % this
        /// </summary>
        IScriptType ArithmeticModulusReverse(IScriptType lhs);

        /// <summary>
        /// <para>(Python) this ** <paramref name="rhs"/></para>
        /// <para>(Lua) this ^ <paramref name="rhs"/></para>
        /// </summary>
        IScriptType ArithmeticExponent(IScriptType rhs);

        /// <summary>
        /// <para>(Python) <paramref name="lhs"/> ** this</para>
        /// <para>(Lua) <paramref name="lhs"/> ^ this</para>
        /// </summary>
        IScriptType ArithmeticExponentReverse(IScriptType lhs);

        /// <summary>
        /// (Python, Lua) this // <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticFloorDivide(IScriptType rhs);

        /// <summary>
        /// (Python, Lua) <paramref name="lhs"/> // this
        /// </summary>
        IScriptType ArithmeticFloorDivideReverse(IScriptType lhs);

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
        /// </summary>
        IScriptType CompareGreaterThan(IScriptType rhs);

        /// <summary>
        /// this &gt;= <paramref name="rhs"/>
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
        /// <paramref name="lhs"/> &amp; this
        /// </summary>
        IScriptType BinaryAndReverse(IScriptType lhs);

        /// <summary>
        /// this | <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryOr(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> | this
        /// </summary>
        IScriptType BinaryOrReverse(IScriptType lhs);

        /// <summary>
        /// <para>(Python, JavaScript) this ^ <paramref name="rhs"/></para>
        /// <para>(Lua) this ~ <paramref name="rhs"/></para>
        /// </summary>
        IScriptType BinaryXor(IScriptType rhs);

        /// <summary>
        /// <para>(Python, JavaScript) <paramref name="lhs"/> ^ this</para>
        /// <para>(Lua) <paramref name="lhs"/> ~ this</para>
        /// </summary>
        IScriptType BinaryXorReverse(IScriptType lhs);

        /// <summary>
        /// this &lt;&lt; <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryLeftShift(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> &lt;&lt; this
        /// </summary>
        IScriptType BinaryLeftShiftReverse(IScriptType lhs);

        /// <summary>
        /// this &gt;&gt; <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryRightShift(IScriptType rhs);

        /// <summary>
        /// <paramref name="lhs"/> &gt;&gt; this
        /// </summary>
        IScriptType BinaryRightShiftReverse(IScriptType lhs);

        #endregion

        #region Member operators

        /// <summary>
        /// (JavaScript, Python) <paramref name="item"/> in this
        /// </summary>
        IScriptType Contains(IScriptType item);

        #endregion
    }
}
