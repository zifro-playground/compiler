using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax
{
    public class FunctionCall : ExpressionNode
    {
        public ExpressionNode Operand { get; }
        public ArgumentsList Arguments { get; }

        public FunctionCall(
            SourceReference source,
            ExpressionNode operand,
            ArgumentsList arguments)
            : base(source)
        {
            Operand = operand;
            Arguments = arguments;
        }

        public override void Compile(PyCompiler compiler)
        {
            Operand.Compile(compiler);

            foreach (ExpressionNode argument in Arguments)
            {
                argument.Compile(compiler);
            }

            // Has either ClrCall, UserCall, or both?
            if ((compiler.Settings.BreakOn & BreakCause.FunctionAnyCall) != 0)
            {
                compiler.Push(new Breakpoint(Source, compiler.Settings.BreakOn & BreakCause.FunctionAnyCall));
            }

            int returnAddress = compiler.GetJumpTargetRelative(+2);
            compiler.Push(new Call(Source, Arguments.Count, returnAddress));
        }
    }
}