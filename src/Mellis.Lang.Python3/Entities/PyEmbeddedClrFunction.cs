using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEmbeddedClrFunction : ScriptEmbeddedClrFunction
    {
        public PyEmbeddedClrFunction(
            IProcessor processor,
            IClrFunction definition)
            : base(processor, definition)
        {
        }

        public override IScriptType GetTypeDef()
        {
            return new PyClrFunctionType(Processor);
        }
    }
}