using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Basic operators with ready compilation implementation into <see cref="BasicOperator"/> op-code.
    /// Common for negating operators such as the "not" (!value), "binary not" (~value)
    /// </summary>
    public abstract class BasicUnaryOperator : UnaryOperator, IBasicOperatorNode
    {
        public abstract BasicOperatorCode OpCode { get; }

        protected BasicUnaryOperator(SourceReference source, ExpressionNode operand)
            : base(source, operand)
        {
        }

        public override void Compile(PyCompiler compiler)
        {
            Operand.Compile(compiler);
            compiler.Push(new BasicOperator(Source, OpCode));
        }
    }
}