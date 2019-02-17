using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class Assignment : Statement
    {
        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }

        public Assignment(SourceReference source,
            ExpressionNode leftOperand, ExpressionNode rightOperand)
            : base(source)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public override void Compile(PyCompiler compiler)
        {
            switch (LeftOperand)
            {
                case Identifier id:
                    RightOperand.Compile(compiler);
                    compiler.Push(new VarSet(Source, id.Name));
                    break;

                default:
                    throw new SyntaxNotYetImplementedException(LeftOperand.Source);
            }
        }
    }
}