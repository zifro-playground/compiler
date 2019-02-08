﻿using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Binary
{
    public class BinaryNot : UnaryOperator
    {
        public BinaryNot(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}