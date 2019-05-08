using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyClrFunctionProxy : ScriptClrFunctionProxy
    {
        public PyClrFunctionProxy(
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