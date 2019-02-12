using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
{
    /// <summary>
    /// Common two part operators, such as "and" (a && b), "or" (a || b), "xor" (a ^ b)
    /// </summary>
    public abstract class BinaryOperator : ExpressionNode
    {
        protected BinaryOperator(SourceReference source,
            ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        // You can happily merge them without conflict
        // since the operator source will always be the
        // combined range from lhs to rhs
        protected BinaryOperator(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : this(SourceReference.Merge(leftOperand.Source, rightOperand.Source),
                leftOperand, rightOperand)
        {
        }

        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }

        public override void Compile(PyCompiler compiler)
        {
            LeftOperand.Compile(compiler);
            RightOperand.Compile(compiler);
            compiler.Push(GetOp());
        }

        protected abstract BaseBinaryOp GetOp();
    }
}