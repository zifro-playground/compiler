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

        public int FromRow { get; private set; }
        public int ToRow { get; private set; }
        public int FromColumn { get; private set; }
        public int ToColumn { get; private set; }

        public static SourceReference ClrSource { get; } = new SourceReference
        {
            IsFromClr = true,
        };

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