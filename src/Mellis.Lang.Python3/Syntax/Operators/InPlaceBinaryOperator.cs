using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    public class InPlaceBinaryOperator : BasicBinaryOperator
    {
        public override BasicOperatorCode OpCode { get; }

        public InPlaceBinaryOperator(SourceReference source, ExpressionNode leftOperand, ExpressionNode rightOperand, BasicOperatorCode opCode) :
            base(source, leftOperand, rightOperand)
        {
            OpCode = opCode;
        }

        public InPlaceBinaryOperator(ExpressionNode leftOperand, ExpressionNode rightOperand, BasicOperatorCode opCode)
            : base(leftOperand, rightOperand)
        {
            OpCode = opCode;
        }

    }
}