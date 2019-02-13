using System;
using System.Collections.Generic;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor : IProcessor
    {
        internal PyProcessor(params IOpCode[] opCodes)
        {
            Factory = new PyScriptTypeFactory(this);
            State = ProcessState.NotStarted;
            LastError = null;

            _valueStack = new Stack<IScriptType>();
            ProgramCounter = -1;
            _opCodes = opCodes ?? new IOpCode[0];
        }

        public IScriptTypeFactory Factory { get; }

        public IScopeContext GlobalScope { get; }

        public IScopeContext CurrentScope { get; }

        public ProcessState State { get; private set; }

        public SourceReference CurrentSource => GetSourceReference(ProgramCounter);

        public InterpreterException LastError { get; private set; }

        public int ProgramCounter { get; private set; }

        private readonly Stack<IScriptType> _valueStack;
        private readonly IOpCode[] _opCodes;
    }
}