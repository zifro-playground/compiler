using Zifro.Compiler.Core.Entities;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public class Assignment : Statement
    {
        public Assignment(SourceReference source, string sourceText)
            : base(source, sourceText)
        {
        }
    }
}