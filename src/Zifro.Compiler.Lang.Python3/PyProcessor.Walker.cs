using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor
    {
        public void ContinueYieldedValue(IScriptType value)
        {
            throw new System.NotImplementedException();
        }

        public void WalkLine()
        {
            throw new System.NotImplementedException();
        }

        public void WalkInstruction()
        {
            _instructionPointer++;
            _opCodes[_instructionPointer].Execute(this);
        }
    }
}