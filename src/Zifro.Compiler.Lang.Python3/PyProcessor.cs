using System;
using System.Collections.Generic;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor : IProcessor
    {
        internal PyProcessor(params IOpCode[] opCodes)
        {
            Factory = new PyScriptTypeFactory(this);
            _valueStack = new Stack<IScriptType>();
            _instructionPointer = -1;
            _opCodes = opCodes ?? new IOpCode[0];
        }

        public IScriptTypeFactory Factory { get; }

        public IScopeContext GlobalScope { get; }

        public IScopeContext CurrentScope { get; }

        private int _instructionPointer;
        private readonly Stack<IScriptType> _valueStack;
        private IOpCode[] _opCodes;

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