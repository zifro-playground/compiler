using System;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.VM;
using Mellis.Resources;

namespace Mellis.Lang.Python3.Syntax.Literals
{
    public class LiteralNone : Literal
    {
        public LiteralNone(SourceReference source)
            : base(source)
        {
        }

        public override void Compile(PyCompiler compiler)
        {
            compiler.Push(new PushLiteral(this));
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Null_Name;
        }

        public override IScriptType ToScriptType(PyProcessor processor)
        {
            return new PyNone(processor);
        }

        public override string ToString()
        {
            return "None";
        }
    }
}