using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalAnd : BinaryOperator
    {
        public LogicalAnd(
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override void Compile(PyCompiler compiler)
        {
            LeftOperand.Compile(compiler);
            
            var jump = new JumpIfFalse(Source, peek: true);
            compiler.Push(jump);
            compiler.Push(new VarPop(Source));

            RightOperand.Compile(compiler);

            jump.Target = compiler.GetJumpTargetForNext();
        }
    }
}