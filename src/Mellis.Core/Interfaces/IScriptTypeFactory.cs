using System;
using System.Collections.Generic;

namespace Mellis.Core.Interfaces
{
    public interface IScriptTypeFactory
    {
        /// <summary>
        /// Gets a null/nil/None value from the current processor context.
        /// </summary>
        IScriptType Null { get; }

        /// <summary>
        /// Gets a value representing the boolean TRUE value from the current processor context.
        /// </summary>
        IScriptType True { get; }

        /// <summary>
        /// Gets a value representing the boolean FALSE value from the current processor context.
        /// </summary>
        IScriptType False { get; }

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>int</c> (signed 32-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(int value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>long</c> (signed 64-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(long value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>float</c> (single-precision floating point number) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(float value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>double</c> (double-precision floating point number) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(double value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>short</c> (signed 16-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(short value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>byte</c> (unsigned 8-bit integer) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(byte value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>char</c> (single UTF-16 character code) specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(char value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>string</c> specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(string value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from type <c>bool</c> specific to the current <seealso cref="IProcessor"/> context.
        /// <para>You can also use the <see cref="True"/> and <see cref="False"/> properties.</para>
        /// </summary>
        IScriptType Create(bool value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from a list of <see cref="IScriptType"/> specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(IList<IScriptType> value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from a dictionary of <see cref="IScriptType"/> specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(IDictionary<IScriptType, IScriptType> value);

        /// <summary>
        /// Creates a <see cref="IScriptType"/> from a function specific to the current <seealso cref="IProcessor"/> context.
        /// </summary>
        IScriptType Create(IFunction value);
    }
}