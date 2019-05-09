using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Common two part operators, such as "and" (a &amp;&amp; b), "or" (a || b), "xor" (a ^ b)
    /// </summary>
    public abstract class BasicBinaryOperator : BinaryOperator, IBasicOperatorNode
    {
        public abstract BasicOperatorCode OpCode { get; }

        protected BasicBinaryOperator(SourceReference source,
            ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source, leftOperand, rightOperand)
        {
        }

        protected BasicBinaryOperator(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override void Compile(PyCompiler compiler)
        {
            LeftOperand.Compile(compiler);
            RightOperand.Compile(compiler);
            compiler.Push(new BasicOperator(Source, OpCode));
        }
    }
}