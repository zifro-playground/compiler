using System;
using System.Collections.Generic;
using System.Linq;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor : IProcessor
    {
        internal PyProcessor(params IOpCode[] opCodes)
            : this(CompilerSettings.DefaultSettings, opCodes)
        {
        }

        internal PyProcessor(CompilerSettings compilerSettings, params IOpCode[] opCodes)
        {
            CompilerSettings = compilerSettings;
            Factory = new PyScriptTypeFactory(this);
            State = ProcessState.NotStarted;
            LastError = null;

            _valueStack = new Stack<IScriptType>();
            _scopesStack = new List<PyScope>();

            ProgramCounter = -1;
            _opCodes = opCodes ?? new IOpCode[0];

            _builtins = new PyScope(null);
            _globalScope = new PyScope(_builtins);
            _callStacks = new Stack<CallStack>();

            _disposables = new Stack<IDisposable>();

            AddBuiltinsInternal();
        }

        public IScriptTypeFactory Factory { get; }

        public IScopeContext GlobalScope => _globalScope;

        public IScopeContext CurrentScope => _scopesStack.LastOrDefault()
                                             ?? GlobalScope;

        public ProcessState State { get; private set; }

        public SourceReference CurrentSource => GetSourceReference(ProgramCounter);

        public InterpreterException LastError { get; private set; }

        public BreakCause LastBreakCause { get; private set; }

        public CompilerSettings CompilerSettings { get; }

        public int ProgramCounter { get; private set; }

        private readonly Stack<IScriptType> _valueStack;
        private readonly List<PyScope> _scopesStack;
        private readonly PyScope _globalScope;
        private readonly IOpCode[] _opCodes;
        private readonly PyScope _builtins;
        private readonly Stack<CallStack> _callStacks;
        private readonly Stack<IDisposable> _disposables;

        private static InterpreterException ConvertException(Exception e)
        {
            while (true)
            {
                switch (e)
                {
                case InterpreterException ex:
                    return ex;

                case AggregateException ex when ex.InnerExceptions.FirstOrDefault() is InterpreterException intEx:
                    return intEx;

                case AggregateException ex:
                    e = ex.InnerExceptions.First();
                    continue;

                default:
                    return new InterpreterLocalizedException(nameof(Localized_Python3_Interpreter.Ex_Unknown_Error), Localized_Python3_Interpreter.Ex_Unknown_Error, e, e.Message);
                }
            }
        }

        ~PyProcessor()
        {
            DisposeAllDisposables();
        }
    }
}