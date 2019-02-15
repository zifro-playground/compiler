using System.Collections.Generic;
using System.Linq;
using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public class IfStatement : Statement
    {

        public IfStatement(SourceReference source,
            ExpressionNode condition,
            Statement ifSuite,
            Statement elseSuite) : base(source)
        {
            Condition = condition;
            IfSuite = ifSuite;
            ElseSuite = elseSuite;
        }

        public IfStatement(SourceReference source,
            ExpressionNode condition,
            Statement ifSuite)
            : this(source, condition, ifSuite, null)
        {
        }

        public ExpressionNode Condition { get; }

        public Statement IfSuite { get; }

        public Statement ElseSuite { get; }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}