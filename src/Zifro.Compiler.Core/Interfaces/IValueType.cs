using System;
using System.Collections.Generic;
using System.Text;

namespace Zifro.Compiler.Core.Interfaces
{
    public interface IValueType
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
        IValueType GetTypeDef();

        /// <summary>
        /// this(...<paramref name="arguments"/>)
        /// </summary>
        IValueType Invoke(IValueType[] arguments);

        /// <summary>
        /// this[<paramref name="index"/>]
        /// </summary>
        IValueType GetIndex(IValueType index);

        /// <summary>
        /// this[<paramref name="index"/>] = <paramref name="value"/>
        /// </summary>
        IValueType SetIndex(IValueType index, IValueType value);

        /// <summary>
        /// this.<paramref name="property"/>
        /// </summary>
        IValueType GetProperty(string property);

        /// <summary>
        /// this.<paramref name="property"/> = <paramref name="value"/>
        /// </summary>
        IValueType SetProperty(string property, IValueType value);

        /// <summary>
        /// Try convert this value to it's CLR representation.
        /// </summary>
        bool TryConvert<T>(out T value);

        #region Arithmetic operators

        /// <summary>
        /// +this
        /// </summary>
        IValueType ArithmeticUnaryPositive();

        /// <summary>
        /// -this
        /// </summary>
        IValueType ArithmeticUnaryNegative();

        /// <summary>
        /// this + <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticAdd(IValueType rhs);

        /// <summary>
        /// this - <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticSubtract(IValueType rhs);

        /// <summary>
        /// this * <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticMultiply(IValueType rhs);

        /// <summary>
        /// this / <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticDivide(IValueType rhs);

        /// <summary>
        /// this % <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticModulus(IValueType rhs);

        /// <summary>
        /// (Python) this ** <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticExponent(IValueType rhs);

        /// <summary>
        /// (Python) this // <paramref name="rhs"/>
        /// </summary>
        IValueType ArithmeticFloorDivide(IValueType rhs);

        #endregion

        #region Comparison operators

        /// <summary>
        /// this == <paramref name="rhs"/>
        /// </summary>
        IValueType CompareEqual(IValueType rhs);

        /// <summary>
        /// <para>(Lua) this ~= <paramref name="rhs"/></para>
        /// <para>(JavaScript, Python) this != <paramref name="rhs"/></para>
        /// <para>(Python) this &lt;&gt; <paramref name="rhs"/></para>
        /// </summary>
        IValueType CompareNotEqual(IValueType rhs);

        /// <summary>
        /// this &gt; <paramref name="rhs"/>
        /// </summary>
        IValueType CompareGreaterThan(IValueType rhs);

        /// <summary>
        /// this &gt;= <paramref name="rhs"/>
        /// </summary>
        IValueType CompareGreaterThanOrEqual(IValueType rhs);

        /// <summary>
        /// this &lt; <paramref name="rhs"/>
        /// </summary>
        IValueType CompareLessThan(IValueType rhs);

        /// <summary>
        /// this &lt;= <paramref name="rhs"/>
        /// </summary>
        IValueType CompareLessThanOrEqual(IValueType rhs);

        #endregion

        #region Bitwise operators

        /// <summary>
        /// ~this
        /// </summary>
        IValueType BinaryNot();

        /// <summary>
        /// this & <paramref name="rhs"/>
        /// </summary>
        IValueType BinaryAnd(IValueType rhs);

        /// <summary>
        /// this | <paramref name="rhs"/>
        /// </summary>
        IValueType BinaryOr(IValueType rhs);

        /// <summary>
        /// this ^ <paramref name="rhs"/>
        /// </summary>
        IValueType BinaryXor(IValueType rhs);

        /// <summary>
        /// this &lt;&lt; <paramref name="rhs"/>
        /// </summary>
        IValueType BinaryLeftShift(IValueType rhs);

        /// <summary>
        /// this &gt;&gt; <paramref name="rhs"/>
        /// </summary>
        IValueType BinaryRightShift(IValueType rhs);

        #endregion

        #region Logical operators

        /// <summary>
        /// <para>(Lua, Python) not this</para>
        /// <para>(JavaScript) !this</para>
        /// </summary>
        IValueType LogicalNot();

        /// <summary>
        /// this and <paramref name="rhs"/>
        /// </summary>
        IValueType LogicalAnd(IValueType rhs);

        /// <summary>
        /// this or <paramref name="rhs"/>
        /// </summary>
        IValueType LogicalOr(IValueType rhs);

        #endregion

        #region Member operators

        /// <summary>
        /// (JavaScript, Python) <paramref name="lhs"/> in this
        /// </summary>
        IValueType MemberIn(IValueType lhs);

        /// <summary>
        /// (Python) <paramref name="lhs"/> not in this
        /// </summary>
        IValueType MemberNotIn(IValueType lhs);

        #endregion

        #region Identity operators

        /// <summary>
        /// (Python) this is <paramref name="rhs"/>
        /// (JavaScript) this instanceof <paramref name="rhs"/>
        /// </summary>
        IValueType IdentityIs(IValueType rhs);

        /// <summary>
        /// (Python) this is not <paramref name="rhs"/>
        /// </summary>
        IValueType IdentityIsNot(IValueType rhs);

        #endregion
    }
}
