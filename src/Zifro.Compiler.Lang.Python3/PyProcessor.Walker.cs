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
            try
            {
                _instructionPointer++;
                _opCodes[_instructionPointer].Execute(this);
            }
            catch (InterpreterException ex)
            {
                LastError = ex;
                State = ProcessState.Error;
            }
            catch (Exception ex)
            {
                LastError = new InterpreterLocalizedException(
                    nameof(Localized_Python3_Interpreter.Ex_Unknown_Error),
                    Localized_Python3_Interpreter.Ex_Unknown_Error,
                    ex, ex.Message);
            }
        }
    }
}