using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class ExpressionStatement : Statement
    {
        public ExpressionNode Expression { get; }

        public ExpressionStatement(ExpressionNode expression)
            : base(expression.Source)
        {
            Expression = expression;
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}