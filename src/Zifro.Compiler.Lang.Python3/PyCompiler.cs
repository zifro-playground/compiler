using Antlr4.Runtime;
using Zifro.Compiler.Lang.Python3.Grammar;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyCompiler : Python3BaseVisitor<ParserRuleContext>
    {
        public override ParserRuleContext VisitFile_input(Python3Parser.File_inputContext context)
        {
            return base.VisitFile_input(context);
        }
    }
}