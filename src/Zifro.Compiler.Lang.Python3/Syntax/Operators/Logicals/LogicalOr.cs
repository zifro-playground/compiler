namespace Zifro.Compiler.Lang.Python3.Syntax.Operators.Logicals
{
    public class LogicalOr : BinaryOperator
    {
        public LogicalOr(
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