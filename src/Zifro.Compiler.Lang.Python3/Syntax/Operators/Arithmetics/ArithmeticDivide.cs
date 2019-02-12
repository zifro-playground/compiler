﻿namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Arithmetics
{
    public class ArithmeticDivide : BinaryOperator
    {
        public ArithmeticDivide(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}