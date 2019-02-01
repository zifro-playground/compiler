using System;
using System.Collections.Generic;

namespace Zifro.Compiler.Core.Interfaces
{
    public interface IValueTypeFactory
    {
        /// <summary>
        /// Gets a null/nil/None value from the current processor context.
        /// </summary>
        IValueType Null { get; }

        /// <summary>
        /// Gets a value representing the boolean TRUE value from the current processor context.
        /// </summary>
        IValueType True { get; }

        /// <summary>
        /// Gets a value representing the boolean FALSE value from the current processor context.
        /// </summary>
        IValueType False { get; }

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>int</c> (signed 32-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(int value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>long</c> (signed 64-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(long value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>short</c> (signed 16-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(short value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>byte</c> (unsigned 8-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(byte value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>char</c> (single UTF-16 character code) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(char value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>string</c> specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(string value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from type <c>bool</c> specific to the current <seealso cref="IProcessor"/> context.
        /// <para>You can also use the <see cref="True"/> and <see cref="False"/> properties.</para>
        /// </summary>
        IValueType Create(bool value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from a list of <see cref="IValueType"/> specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(IList<IValueType> value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from a dictionary of <see cref="IValueType"/> specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(IDictionary<IValueType, IValueType> value);

        /// <summary>
        /// Creates a <see cref="IValueType"/> from a function specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IValueType Create(IFunctionType value);
    }
}