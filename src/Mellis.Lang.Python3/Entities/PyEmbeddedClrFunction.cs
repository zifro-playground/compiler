using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEmbeddedClrFunction : EmbeddedClrFunctionBase
    {
        public PyEmbeddedClrFunction(
            IProcessor processor,
            IClrFunction definition,
            string name = null)
            : base(processor, definition, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyEmbeddedClrFunction(Processor, Definition, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyClrFunctionType(Processor);
        }
    }
}