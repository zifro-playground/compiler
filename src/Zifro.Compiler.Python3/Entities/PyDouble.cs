using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Entities;

namespace Zifro.Compiler.Lang.Python3.Entities
{
    public class PyDouble : DoubleBase
    {
        public PyDouble(double value, PyProcessor processor)
            : base(processor)
        {
            Value = value;
        }

        /// <inheritdoc />
        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }
    }
}