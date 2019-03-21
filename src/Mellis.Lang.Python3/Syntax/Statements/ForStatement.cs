using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class ForStatement : Statement
    {
        public ExpressionNode Operand { get; }
        public ExpressionNode Iterator { get; }
        public Statement Suite { get; }

        public ForStatement(
            SourceReference source,
            ExpressionNode operand,
            ExpressionNode iterator,
            Statement suite) : base(source)
        {
            Operand = operand;
            Iterator = iterator;
            Suite = suite;
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}