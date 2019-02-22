using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mellis.Core.Entities;

namespace Mellis.Lang.Python3.Syntax
{
    public class ArgumentsList : SyntaxNode, IReadOnlyList<ExpressionNode>
    {
        private readonly IReadOnlyList<ExpressionNode> _arguments;

        public int Count => _arguments.Count;

        public ExpressionNode this[int index] => _arguments[index];

        public ArgumentsList(
            SourceReference source,
            IEnumerable<ExpressionNode> arguments)
            : base(source)
        {
            _arguments = arguments.ToArray();
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<ExpressionNode> GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}