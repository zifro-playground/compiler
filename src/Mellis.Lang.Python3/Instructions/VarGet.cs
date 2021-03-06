﻿using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class VarGet : IOpCode
    {
        public VarGet(SourceReference source, string identifier)
        {
            Source = source;
            Identifier = identifier;
        }

        public SourceReference Source { get; }

        public string Identifier { get; }

        public void Execute(VM.PyProcessor processor)
        {
            IScriptType value = processor.GetVariable(Identifier);
            processor.PushValue(value);
        }

        public override string ToString()
        {
            return $"get->{Identifier}";
        }
    }
}