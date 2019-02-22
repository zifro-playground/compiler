using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

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
            Expression.Compile(compiler);

            // Pop it because it didn't use it
            compiler.Push(new VarPop(Source));
        }
    }
}