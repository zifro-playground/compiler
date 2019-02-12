using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Resources;

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
            switch (State)
            {
                case ProcessState.Ended:
                case ProcessState.Error:
                    throw new InternalException(
                        nameof(Localized_Python3_Interpreter.Ex_Process_Ended),
                        Localized_Python3_Interpreter.Ex_Process_Ended);

                case ProcessState.Yielded:
                    throw new InternalException(
                        nameof(Localized_Python3_Interpreter.Ex_Process_Yielded),
                        Localized_Python3_Interpreter.Ex_Process_Yielded);

                case ProcessState.NotStarted when _opCodes.Length == 0:
                    State = ProcessState.Ended;
                    break;

                default:
                    try
                    {
                        _instructionPointer++;
                        _opCodes[_instructionPointer].Execute(this);

                        State = _instructionPointer + 1 < _opCodes.Length
                            ? ProcessState.Running
                            : ProcessState.Ended;
                    }
                    catch (InterpreterException ex)
                    {
                        State = ProcessState.Error;
                        LastError = ex;
                    }
                    catch (Exception ex)
                    {
                        State = ProcessState.Error;

                        LastError = new InterpreterLocalizedException(
                            nameof(Localized_Python3_Interpreter.Ex_Unknown_Error),
                            Localized_Python3_Interpreter.Ex_Unknown_Error,
                            ex, ex.Message);
                    }
                    break;
            }
        }
    }
}