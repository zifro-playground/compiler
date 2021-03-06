﻿using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticPositive : BasicUnaryOperator
    {
        public override BasicOperatorCode OpCode => BasicOperatorCode.APos;

        public ArithmeticPositive(SourceReference source,
            ExpressionNode operand)
            : base(source, operand)
        {
        }
    }
}