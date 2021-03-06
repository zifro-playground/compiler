﻿using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Comparisons
{
    public class CompareInNot : Comparison
    {
        public override ComparisonType Type => ComparisonType.InNot;

        public override BasicOperatorCode OpCode => BasicOperatorCode.CNIn;

        public CompareInNot(ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }
    }
}