using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        private int _numOfJumpsThisWalk;

        private YieldData _currentYield;

        /// <summary>
        /// No particular walk status. Nothing happened.
        /// </summary>
        internal const WalkStatus NULL_WALK_STATUS = (WalkStatus) (-1);

        // Oliver & Fredrik approved ✔
        internal const int JUMPS_THRESHOLD = 102 + 137;

        public void ResolveYield(IScriptType returnValue)
        {
            if (State != ProcessState.Yielded)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_Yield_ResolveWhenNotYielded),
                    Localized_Python3_Interpreter.Ex_Yield_ResolveWhenNotYielded
                );
            }

            if (_currentYield is null)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_Yield_ResolveNoYieldData),
                    Localized_Python3_Interpreter.Ex_Yield_ResolveNoYieldData
                );
            }

            CallStack callStack = PeekCallStack();

            // Perform the exit invoke & return transformation
            returnValue = _currentYield.Definition.InvokeExit(_currentYield.Arguments,
                              returnValue ?? Factory.Null)
                          ?? Factory.Null;

            PushValue(returnValue);

            // Return to sender
            PopCallStack();
            State = ProcessState.Running;
            JumpToInstruction(callStack.ReturnAddress);
            _currentYield = null;
        }

        public void ResolveYield()
        {
            ResolveYield(Factory.Null);
        }

        internal void Yield(YieldData yieldData)
        {
            _currentYield = yieldData;
            State = ProcessState.Yielded;
        }

        public WalkStatus WalkLine()
        {
            _numOfJumpsThisWalk = 0;

            // First walk? Only get on op [0]
            if (State == ProcessState.NotStarted)
            {
                WalkInstruction();
                return WalkStatus.NewLine;
            }

            int? initialRow = GetRow(ProgramCounter);

            if (initialRow.HasValue)
            {
                // Initial is row => walk until current is different row
                int? nextRow;
                do
                {
                    WalkStatus walkStatus = WalkInstruction();
                    if (walkStatus != NULL_WALK_STATUS)
                    {
                        return walkStatus;
                    }

                    nextRow = GetRow(ProgramCounter);
                } while ((nextRow == null || nextRow.Value == initialRow.Value) &&
                         State == ProcessState.Running &&
                         _numOfJumpsThisWalk < JUMPS_THRESHOLD);
            }
            else
            {
                // Initial is clr => walk until current is not clr
                do
                {
                    WalkStatus walkStatus = WalkInstruction();
                    if (walkStatus != NULL_WALK_STATUS)
                    {
                        return walkStatus;
                    }

                } while (GetRow(ProgramCounter) == null &&
                         State == ProcessState.Running &&
                         _numOfJumpsThisWalk < JUMPS_THRESHOLD);
            }

            int? GetRow(int i)
            {
                SourceReference source = GetSourceReference(i);
                return source.IsFromClr
                    ? (int?)null
                    : source.FromRow;
            }

            return WalkStatus.NewLine;
        }

        public WalkStatus Walk()
        {
            _numOfJumpsThisWalk = 0;

            if (CompilerSettings.InstructionLimit > 0 &&
                CompilerSettings.BreakOn.HasFlag(BreakCause.InstructionLimitReached))
            {
                return _WalkUntilInstructionLimit();
            }

            return _WalkUntilEnd();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private WalkStatus _WalkUntilInstructionLimit()
        {
            int limit = CompilerSettings.InstructionLimit;

            for (int i = 0; i < limit; i++)
            {
                WalkStatus walkStatus = _WalkInstructionWithJumpLimit();
                if (walkStatus != NULL_WALK_STATUS)
                {
                    return walkStatus;
                }
            }

            return WalkStatus.Break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private WalkStatus _WalkUntilEnd()
        {
            while (true)
            {
                WalkStatus walkStatus = _WalkInstructionWithJumpLimit();
                if (walkStatus != NULL_WALK_STATUS)
                {
                    return walkStatus;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private WalkStatus _WalkInstructionWithJumpLimit()
        {
            if (CompilerSettings.BreakOn.HasFlag(BreakCause.JumpLimitReached) &&
                CompilerSettings.JumpLimit > 0 &&
                _numOfJumpsThisWalk >= CompilerSettings.JumpLimit)
            {
                // Too many jumps, abort
                return WalkStatus.Break;
            }

            // Free to go
            return WalkInstruction();
        }

        public WalkStatus WalkInstruction()
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
                ProgramCounter = 0;
                State = ProcessState.Running;
                break;

            case ProcessState.Running when ProgramCounter < 0:
                ProgramCounter = 0;
                break;

            case ProcessState.Running:
                try
                {
                    int startCounter = ProgramCounter;
                    
                    IOpCode opCode = _opCodes[ProgramCounter++];

                    if (opCode is Instructions.Breakpoint)
                    {
                        State = ProcessState.Running;
                        return WalkStatus.Break;
                    }

                    opCode.Execute(this);

                    if (State == ProcessState.Yielded)
                    {
                        ProgramCounter = startCounter;
                        return WalkStatus.Yielded;
                    }

                    if (ProgramCounter < _opCodes.Length)
                    {
                        State = ProcessState.Running;
                    }
                    else
                    {
                        State = ProcessState.Ended;
                        OnProcessEnded(State);
                        return WalkStatus.Ended;
                    }
                }
                catch (Exception ex)
                {
                    State = ProcessState.Error;

                    LastError = ConvertException(ex);

                    OnProcessEnded(State);
                    throw LastError;
                }

                break;

            default:
                throw new InvalidEnumArgumentException(nameof(State), (int)State, typeof(ProcessState));
            }

            return NULL_WALK_STATUS;
        }

        private SourceReference GetSourceReference(int opCodeIndex)
        {
            if (opCodeIndex >= 0 && opCodeIndex < _opCodes.Length)
            {
                return _opCodes[opCodeIndex].Source;
            }

            return SourceReference.ClrSource;
        }

        internal void JumpToInstruction(int index)
        {
            ProgramCounter = index;

            if (ProgramCounter >= _opCodes.Length)
            {
                State = ProcessState.Ended;
                OnProcessEnded(ProcessState.Ended);
            }

            _numOfJumpsThisWalk++;
        }
    }
}