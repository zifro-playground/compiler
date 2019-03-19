﻿using Mellis.Core.Entities;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class ForEachNext : IOpCode
    {
        public SourceReference Source { get; }

        public ForEachNext(SourceReference source)
        {
            Source = source;
        }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            // ReSharper disable once PossiblyMistakenUseOfInterpolatedStringInsert
            return $"iter->next@{0}";
        }
    }
}