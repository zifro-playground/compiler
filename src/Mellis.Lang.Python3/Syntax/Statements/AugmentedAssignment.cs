using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Resources;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class AugmentedAssignment : Assignment
    {
        public BasicOperatorCode OpCode { get; }

        public AugmentedAssignment(
            SourceReference source,
            ExpressionNode leftOperand,
            ExpressionNode rightOperand,
            BasicOperatorCode opCode)
            : base(source, leftOperand, rightOperand)
        {
            OpCode = opCode;
        }

        public AugmentedAssignment(
            SourceReference source,
            BasicOperatorCode opCode)
            : base(source, null, null)
        {
            OpCode = opCode;
        }

        public override void Compile(PyCompiler compiler)
        {
            VarSet op = GetOpCodeForAssignmentOrThrow(Source, LeftOperand);

            // TODO
        }
    }
}