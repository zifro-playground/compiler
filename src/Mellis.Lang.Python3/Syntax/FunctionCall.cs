using System.Collections.Generic;
using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax
{
    public class FunctionCall : ExpressionNode
    {
        public ExpressionNode Operand { get; internal set; }
        public IReadOnlyList<ExpressionNode> Arguments { get; }

        public FunctionCall(
            SourceReference source,
            ExpressionNode operand,
            IReadOnlyList<ExpressionNode> arguments)
            : base(source)
        {
            Operand = operand;
            Arguments = arguments;
        }

        public FunctionCall(
            SourceReference source,
            IReadOnlyList<ExpressionNode> arguments)
            : this(source, null, arguments)
        {
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }
    }
}