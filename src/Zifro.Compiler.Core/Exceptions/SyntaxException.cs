using System;
using System.Linq;
using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Use this for compile-time exceptions based off the source code.
    /// </summary>
    public class SyntaxException : InterpreterLocalizedException
    {
        public SourceReference SourceReference { get; set; }

        /// <summary>
        /// Creates a syntax exception based off the source code.
        /// <para>
        /// The first 4 formatting values are dedicated from the source reference.
        /// </para>
        /// <para><c>{0}</c> source start line</para>
        /// <para><c>{1}</c> source start column</para>
        /// <para><c>{2}</c> source end line</para>
        /// <para><c>{3}</c> source end column</para>
        /// </summary>
        public SyntaxException(SourceReference source, string localizeKey, 
            string localizedMessageFormat, params object[] values)
            : base(localizeKey, localizedMessageFormat, GetParams(source, values))
        {
            SourceReference = source;
        }

        protected static object[] GetParams(SourceReference source)
        {
            return new object[]
            {
                source.FromRow, source.FromColumn,
                source.ToRow, source.ToColumn
            };
        }

        protected static object[] GetParams(SourceReference source, object[] additional)
        {
            return GetParams(source).Concat(additional).ToArray();
        }
    }
}