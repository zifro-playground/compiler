using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    public class InPlaceBinaryOperatorFactory : SyntaxNode
    {
        public BasicOperatorCode OpCode { get; }

        public InPlaceBinaryOperatorFactory(SourceReference source, BasicOperatorCode opCode)
            : base(source)
        {
            OpCode = opCode;
        }

        public InPlaceBinaryOperator Create(ExpressionNode leftOperand, ExpressionNode rightOperand)
        {
            throw new System.NotImplementedException();
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new SyntaxUncompilableException(Source, typeof(InPlaceBinaryOperatorFactory));
        }
    }
}