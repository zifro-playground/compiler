using System;
using System.Collections.Generic;
using System.Linq;
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
            _globalScope = new PyScope(null);
            _scopesStack = new List<PyScope>();
            ProgramCounter = -1;
            _opCodes = opCodes ?? new IOpCode[0];
        }

        public IScriptTypeFactory Factory { get; }

        public IScopeContext GlobalScope => _globalScope;

        public IScopeContext CurrentScope => _scopesStack.LastOrDefault()
                                             ?? GlobalScope;

        public ProcessState State { get; private set; }

        public SourceReference CurrentSource => GetSourceReference(ProgramCounter);

        public InterpreterException LastError { get; private set; }

        public int ProgramCounter { get; private set; }

        private readonly Stack<IScriptType> _valueStack;
        private readonly List<PyScope> _scopesStack;
        private readonly PyScope _globalScope;
        private readonly IOpCode[] _opCodes;
    }
}