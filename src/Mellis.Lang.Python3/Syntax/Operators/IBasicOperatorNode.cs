using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    public interface IBasicOperatorNode
    {
        BasicOperatorCode OpCode { get; }
    }
}