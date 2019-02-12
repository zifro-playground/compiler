using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Base.Entities;

namespace Zifro.Compiler.Lang.Python3.Entities
{
    public class PyString : StringBase
    {
        public PyString(PyProcessor processor, string value)
            : base(processor, value)
        {
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }
    }
}