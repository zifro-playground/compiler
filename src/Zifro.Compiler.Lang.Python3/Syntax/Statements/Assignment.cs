using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
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