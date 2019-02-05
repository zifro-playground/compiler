using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Entities;

namespace Zifro.Compiler.Lang.Python3.Entities
{
    public class PyInteger : IntegerBase
    {
        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }
    }
}