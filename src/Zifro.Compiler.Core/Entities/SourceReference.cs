using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Zifro.Compiler.Core.Entities
{
    public struct SourceReference
    {
        /// <summary>
        /// Gets a source reference from the Common Language Runtime.
        /// </summary>
        public bool IsFromClr { get; private set; }

        /// <summary>
        /// Starting row in code. 1..n, inclusive.
        /// </summary>
        public int FromRow { get; private set; }

        /// <summary>
        /// Ending row in code. 1..n, inclusive.
        /// Equal to start row property <see cref="FromRow"/> if same row.
        /// </summary>
        public int ToRow { get; private set; }

        /// <summary>
        /// Starting column in code. 0..n-1, inclusive.
        /// </summary>
        public int FromColumn { get; private set; }

        /// <summary>
        /// Ending column in code. 0..n-1, inclusive.
        /// Equal to start column property <see cref="FromColumn"/> if same column (and if single character).
        /// Is 1 less than start column property <see cref="FromColumn"/> if references zero characters.
        /// </summary>
        public int ToColumn { get; private set; }

        public static SourceReference ClrSource { get; } = new SourceReference
        {
            IsFromClr = true,
        };

        /// <param name="fromRow">Starting row in code. 1..n, inclusive</param>
        /// <param name="toRow">
        /// Ending row in code. 1..n, inclusive.
        /// Equal to <paramref name="fromRow"/> if same row.
        /// </param>
        /// <param name="fromColumn">Starting column in code. 0..n-1, inclusive.</param>
        /// <param name="toColumn">
        /// Ending column in code. 0..n-1, inclusive.
        /// Equal to <paramref name="fromColumn"/> if same column (and if single character).
        /// Is 1 less than <paramref name="fromColumn"/> if references zero characters.
        /// </param>
        public SourceReference(
            int fromRow,
            int toRow,
            int fromColumn,
            int toColumn)
        {
            IsFromClr = false;
            FromRow = fromRow;
            ToRow = toRow;
            FromColumn = fromColumn;
            ToColumn = toColumn;
        }

        public static SourceReference Merge(SourceReference a, SourceReference b)
        {
            if (a.IsFromClr && b.IsFromClr)
                return a;

            if (b.FromRow < a.FromRow)
            {
                a.FromRow = b.FromRow;
                a.FromColumn = b.FromColumn;
            }
            else if (b.FromRow == a.FromRow &&
                     b.FromColumn < a.FromColumn)
            {
                a.FromColumn = b.FromColumn;
            }

            if (b.ToRow > a.ToRow)
            {
                a.ToRow = b.ToRow;
                a.ToColumn = b.ToColumn;
            }
            else if (b.ToRow == a.ToRow &&
                     b.ToColumn > a.ToColumn)
            {
                a.ToColumn = b.ToColumn;
            }

            return a;
        }

        public static SourceReference Merge(IEnumerable<SourceReference> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            using (IEnumerator<SourceReference> enumerator = sources.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException("SourceReference enumeration is empty.", nameof(sources));
                }

                SourceReference value = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    value = Merge(value, enumerator.Current);
                }

                return value;
            }
        }
    }
}