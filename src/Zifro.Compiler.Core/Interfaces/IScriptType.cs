using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Zifro.Compiler.Core.Interfaces
{
    /// <summary>
    /// Used for reference typed variables.
    /// </summary>
    public interface IScriptType
    {
        /// <summary>
        /// The assigned processor environment.
        /// Set by the processor when value is binded to the context.
        /// </summary>
        IProcessor Processor { set; }

        /// <summary>
        /// <para>(Lua, Python) type(this)</para>
        /// <para>(JavaScript) typeof(this)</para>
        /// </summary>
        IScriptType GetTypeDef();

        /// <summary>
        /// Name of this type. Used in error messages.
        /// Should be localized to the current UI culture set via <seealso cref="Thread.CurrentUICulture"/>.
        /// </summary>
        string GetTypeName();

        /// <summary>
        /// this(...<paramref name="arguments"/>)
        /// </summary>
        IScriptType Invoke(IScriptType[] arguments);

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
        bool TryConvert<T>(out T value);

        /// <summary>
        /// Try convert this value to it's CLR representation.
        /// </summary>
        bool TryConvert(Type type, out object value);

        #region Arithmetic operators

        /// <summary>
        /// +this
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
        /// (Python) this ** <paramref name="rhs"/>
        /// </summary>
        IScriptType ArithmeticExponent(IScriptType rhs);

        /// <summary>
        /// (Python) this // <paramref name="rhs"/>
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
        /// this & <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryAnd(IScriptType rhs);

        /// <summary>
        /// this | <paramref name="rhs"/>
        /// </summary>
        IScriptType BinaryOr(IScriptType rhs);

        /// <summary>
        /// this ^ <paramref name="rhs"/>
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

        #region Logical operators

        /// <summary>
        /// <para>(Lua, Python) not this</para>
        /// <para>(JavaScript) !this</para>
        /// </summary>
        IScriptType LogicalNot();

        /// <summary>
        /// this and <paramref name="rhs"/>
        /// </summary>
        IScriptType LogicalAnd(IScriptType rhs);

        /// <summary>
        /// this or <paramref name="rhs"/>
        /// </summary>
        IScriptType LogicalOr(IScriptType rhs);

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
