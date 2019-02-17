using System;
using System.ComponentModel;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3
{
    public partial class PyProcessor
    {
        private int _numOfJumpsThisWalk = 0;

        // Oliver & Fredrik approved ✔️
        private const int JUMPS_THRESHOLD = 102+137;

        public void ContinueYieldedValue(IScriptType value)
        {
            throw new System.NotImplementedException();
        }

        public void WalkLine()
        {
            _numOfJumpsThisWalk = 0;

            WalkInstruction();

            // Because counter starts at -1
            int? initialRow = GetRow(ProgramCounter);

            if (initialRow.HasValue)
            {
                // Initial is row => walk until next is other row
                while (!(GetRow(ProgramCounter + 1) > initialRow.Value) &&
                       State == ProcessState.Running &&
                       _numOfJumpsThisWalk < JUMPS_THRESHOLD)
                    WalkInstruction();
            }
            else
            {
                // Initial is clr => walk until next is line
                while (GetRow(ProgramCounter + 1) == null &&
                       State == ProcessState.Running &&
                       _numOfJumpsThisWalk < JUMPS_THRESHOLD)
                    WalkInstruction();
            }

            int? GetRow(int i)
            {
                var source = GetSourceReference(i);
                return source.IsFromClr
                    ? (int?) null
                    : source.FromRow;
            }
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
                    OnProcessEnded(State);
                    break;

                case ProcessState.NotStarted:
                case ProcessState.Running:
                    try
                    {
                        ProgramCounter++;
                        _opCodes[ProgramCounter].Execute(this);

                        if (ProgramCounter + 1 < _opCodes.Length)
                            State = ProcessState.Running;
                        else
                        {
                            State = ProcessState.Ended;
                            OnProcessEnded(State);
                        }
                    }
                    catch (InterpreterException ex)
                    {
                        State = ProcessState.Error;
                        LastError = ex;

                        OnProcessEnded(State);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        State = ProcessState.Error;

                        LastError = new InterpreterLocalizedException(
                            nameof(Localized_Python3_Interpreter.Ex_Unknown_Error),
                            Localized_Python3_Interpreter.Ex_Unknown_Error,
                            ex, ex.Message);

                        OnProcessEnded(State);
                        throw LastError;
                    }

                    break;

                default:
                    throw new InvalidEnumArgumentException(nameof(State), (int)State, typeof(ProcessState));
            }
        }

        private SourceReference GetSourceReference(int opCodeIndex)
        {
            if (opCodeIndex >= 0 && opCodeIndex < _opCodes.Length)
                return _opCodes[opCodeIndex].Source;

            return SourceReference.ClrSource;
        }

        internal void JumpToInstruction(int index)
        {
            ProgramCounter = index - 1;
            _numOfJumpsThisWalk++;
        }
    }
}