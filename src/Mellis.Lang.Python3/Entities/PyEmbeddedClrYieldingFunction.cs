using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Entities.Classes;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEmbeddedClrYieldingFunction : ScriptEmbeddedClrYieldingFunction
    {
        public PyEmbeddedClrYieldingFunction(
            IProcessor processor,
            IClrYieldingFunction definition,
            string name = null)
            : base(processor, definition, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyEmbeddedClrYieldingFunction(Processor, Definition, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyClrFunctionType(Processor);
        }
    }
}