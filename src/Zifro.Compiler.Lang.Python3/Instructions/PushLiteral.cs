﻿using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;
using Zifro.Compiler.Lang.Python3.Syntax;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class PushLiteral<TValue> : IOpCode
    {
        public PushLiteral(Literal<TValue> literal)
        {
            Literal = literal;
            Source = literal.Source;
        }

        public Literal<TValue> Literal { get; }

        public SourceReference Source { get; }

        public void Execute(PyProcessor processor)
        {
            processor.PushValue(Literal.ToScriptType(processor));
        }

        public override string ToString()
        {
            return $"push {{{Literal}}}";
        }
    }
}