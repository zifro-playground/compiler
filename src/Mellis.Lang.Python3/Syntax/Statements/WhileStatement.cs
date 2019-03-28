using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class WhileStatement : Statement
    {
        public ExpressionNode Condition { get; }
        public Statement Suite { get; }

        public WhileStatement(SourceReference source,
            ExpressionNode condition,
            Statement suite)
            : base(source)
        {
            Condition = condition;
            Suite = suite;
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}