using System.Collections.Generic;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor : IProcessor
    {
        public PyProcessor()
        {
            Factory = new PyScriptTypeFactory(this);
        }

        public IScriptTypeFactory Factory { get; }

        public IScopeContext GlobalScope { get; }
        public IScopeContext CurrentScope { get; }

        private int _instructionPointer;
        private Stack<IScriptType> _valueStack;

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

        }
    }
}