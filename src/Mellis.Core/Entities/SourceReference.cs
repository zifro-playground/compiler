using System;
using System.Collections.Generic;

namespace Mellis.Core.Entities
{
    public struct SourceReference : IEquatable<SourceReference>
    {
        /// <summary>
        /// Gets a source reference from the Common Language Runtime.
        /// </summary>
        public bool IsFromClr { get; }

        /// <summary>
        /// Starting row in code. 1..n, inclusive.
        /// </summary>
        public int FromRow { get; }

        /// <summary>
        /// Ending row in code. 1..n, inclusive.
        /// Equal to start row property <see cref="FromRow"/> if same row.
        /// </summary>
        public int ToRow { get; }

        /// <summary>
        /// Starting column in code. 0..n-1, inclusive.
        /// </summary>
        public int FromColumn { get; }

        /// <summary>
        /// Ending column in code. 0..n-1, inclusive.
        /// Equal to start column property <see cref="FromColumn"/> if same column (and if single character).
        /// Is 1 less than start column property <see cref="FromColumn"/> if references zero characters.
        /// </summary>
        public int ToColumn { get; }

        public static SourceReference ClrSource { get; } = new SourceReference(true);

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

        private SourceReference(bool isFromClr)
        {
            IsFromClr = isFromClr;
            FromRow = default;
            ToRow = default;
            FromColumn = default;
            ToColumn = default;
        }

        public SourceReference LastRow()
        {
            if (IsFromClr)
            {
                return ClrSource;
            }

            if (FromRow == ToRow)
            {
                return this;
            }

            return new SourceReference(
                ToRow, ToRow,
                0, ToColumn
            );
        }

        public static SourceReference Merge(SourceReference a, SourceReference b)
        {
            if (a.IsFromClr && b.IsFromClr)
            {
                return a;
            }

            if (a.IsFromClr)
            {
                return b;
            }

            if (b.IsFromClr)
            {
                return a;
            }

            int fromRow = a.FromRow;
            int fromColumn = a.FromColumn;
            int toRow = a.ToRow;
            int toColumn = a.ToColumn;

            if (b.FromRow < a.FromRow)
            {
                fromRow = b.FromRow;
                fromColumn = b.FromColumn;
            }
            else if (b.FromRow == a.FromRow &&
                     b.FromColumn < a.FromColumn)
            {
                fromColumn = b.FromColumn;
            }

            if (b.ToRow > a.ToRow)
            {
                toRow = b.ToRow;
                toColumn = b.ToColumn;
            }
            else if (b.ToRow == a.ToRow &&
                     b.ToColumn > a.ToColumn)
            {
                toColumn = b.ToColumn;
            }

            return new SourceReference(fromRow, toRow, fromColumn, toColumn);
        }

        public static SourceReference Merge(params SourceReference[] sourcesParams)
        {
            return Merge(sourcesEnumerable: sourcesParams);
        }

        public static SourceReference Merge(IEnumerable<SourceReference> sourcesEnumerable)
        {
            if (sourcesEnumerable == null)
            {
                throw new ArgumentNullException(nameof(sourcesEnumerable));
            }

            using (IEnumerator<SourceReference> enumerator = sourcesEnumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException(@"SourceReference enumeration is empty.", nameof(sourcesEnumerable));
                }

                SourceReference value = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    value = Merge(value, enumerator.Current);
                }

                return value;
            }
        }

        public override string ToString()
        {
            if (IsFromClr)
            {
                return "$CLR";
            }

            if (FromRow == ToRow)
            {
                return $"ln{FromRow} col{FromColumn}-{ToColumn}";
            }

            return $"ln{FromRow} col{FromColumn}-ln{ToRow} col{ToColumn}";
        }

        public override bool Equals(object obj)
        {
            return obj is SourceReference source && Equals(source);
        }

        public bool Equals(SourceReference other)
        {
            return IsFromClr == other.IsFromClr &&
                   FromRow == other.FromRow &&
                   ToRow == other.ToRow &&
                   FromColumn == other.FromColumn &&
                   ToColumn == other.ToColumn;
        }

        public override int GetHashCode()
        {
            int hashCode = 57309968;
            hashCode = hashCode * -1521134295 + IsFromClr.GetHashCode();
            hashCode = hashCode * -1521134295 + FromRow.GetHashCode();
            hashCode = hashCode * -1521134295 + ToRow.GetHashCode();
            hashCode = hashCode * -1521134295 + FromColumn.GetHashCode();
            hashCode = hashCode * -1521134295 + ToColumn.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SourceReference reference1, SourceReference reference2)
        {
            return reference1.Equals(reference2);
        }

        public static bool operator !=(SourceReference reference1, SourceReference reference2)
        {
            return !(reference1 == reference2);
        }
    }
}