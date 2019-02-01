using System;
using System.Collections.Generic;
using System.Text;

namespace Zifro.Compiler.Core.Entities
{
    public struct SourceReference
    {
        /// <summary>
        /// Is the source defined in the Common Language Runtime?
        /// </summary>
        public bool IsFromClr { get; private set; }

        public int FromRow { get; }
        public int ToRow { get; }
        public int FromColumn { get; }
        public int ToColumn { get; }

        public static SourceReference ClrSource => new SourceReference
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
    }
}
