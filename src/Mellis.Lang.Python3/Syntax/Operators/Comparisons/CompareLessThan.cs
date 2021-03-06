﻿using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareLessThan : Comparison
    {
        public override ComparisonType Type => ComparisonType.LessThan;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CLt;

        public CompareLessThan(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}