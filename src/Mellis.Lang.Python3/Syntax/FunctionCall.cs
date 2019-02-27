using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax
{
    public class FunctionCall : ExpressionNode
    {
        public ExpressionNode Operand { get; internal set; }
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

            int returnAddress = compiler.GetJumpTargetRelative(+2);
            compiler.Push(new Call(Source, Arguments.Count, returnAddress));
            compiler.Push(new CallStackPop(Source));
        }
    }
}