using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public class IfStatement : Statement
    {
        public IfStatement(SourceReference source,
            ExpressionNode condition,
            StatementList ifSuite,
            StatementList elseSuite) : base(source)
        {
            Condition = condition;
            IfSuite = ifSuite;
            ElseSuite = elseSuite;
        }

        public IfStatement(SourceReference source,
            ExpressionNode condition,
            StatementList ifSuite)
            : this(source, condition, ifSuite, null)
        {
        }

        public ExpressionNode Condition { get; }

        public StatementList IfSuite { get; }

        public StatementList ElseSuite { get; }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}