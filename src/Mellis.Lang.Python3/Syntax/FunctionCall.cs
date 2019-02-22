using System.Collections.Generic;
using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax
{
    public class FunctionCall : ExpressionNode
    {
        public ExpressionNode Operand { get; internal set; }
        public FunctionArguments Arguments { get; }

        public FunctionCall(
            SourceReference source,
            ExpressionNode operand,
            FunctionArguments arguments)
            : base(source)
        {
            Operand = operand;
            Arguments = arguments;
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}