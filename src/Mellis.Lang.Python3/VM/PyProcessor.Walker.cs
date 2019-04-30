using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        // Oliver & Fredrik approved ✔
        internal const int JUMPS_THRESHOLD = 102 + 137;

        /// <summary>
        ///     No particular walk status. Nothing happened.
        /// </summary>
        internal const WalkStatus NULL_WALK_STATUS = (WalkStatus)(-1);

        private YieldData _currentYield;
        private int _numOfJumpsThisWalk;

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

        public WalkStatus WalkLine()
        {
            _numOfJumpsThisWalk = 0;

            // First walk? Only get on op [0]
            if (State == ProcessState.NotStarted)
            {
                WalkInstruction();
                return WalkStatus.NewLine;
            }

            int instructionsThisWalk = 0;
            int instructionLimit = CompilerSettings.InstructionLimit;
            bool hasInstructionLimit = CompilerSettings.BreakOn.HasFlag(BreakCause.InstructionLimitReached) &&
                                       instructionLimit > 0;

            int jumpLimit = CompilerSettings.JumpLimit;
            bool hasJumpLimitBreak = CompilerSettings.BreakOn.HasFlag(BreakCause.JumpLimitReached) &&
                                     jumpLimit > 0;


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

                    if (hasJumpLimitBreak && _numOfJumpsThisWalk >= jumpLimit)
                    {
                        // Too many jumps, abort
                        return WalkStatus.Break;
                    }

                    if (hasInstructionLimit && ++instructionsThisWalk >= instructionLimit)
                    {
                        // Too many instructions, abort
                        return WalkStatus.Break;
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

                    if (hasJumpLimitBreak && _numOfJumpsThisWalk >= jumpLimit)
                    {
                        // Too many jumps, abort
                        return WalkStatus.Break;
                    }

                    if (hasInstructionLimit && ++instructionsThisWalk >= instructionLimit)
                    {
                        // Too many instructions, abort
                        return WalkStatus.Break;
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

                    if (opCode is Breakpoint)
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

        internal void Yield(YieldData yieldData)
        {
            _currentYield = yieldData;
            State = ProcessState.Yielded;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private WalkStatus _WalkUntilInstructionLimit()
        {
            int instructionLimit = CompilerSettings.InstructionLimit;

            int jumpLimit = CompilerSettings.JumpLimit;
            bool hasJumpLimitBreak = CompilerSettings.BreakOn.HasFlag(BreakCause.JumpLimitReached) &&
                                     jumpLimit > 0;

            for (int i = 0; i < instructionLimit; i++)
            {
                WalkStatus walkStatus = WalkInstruction();
                if (walkStatus != NULL_WALK_STATUS)
                {
                    return walkStatus;
                }

                if (hasJumpLimitBreak && _numOfJumpsThisWalk >= jumpLimit)
                {
                    // Too many jumps, abort
                    return WalkStatus.Break;
                }
            }

            return WalkStatus.Break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private WalkStatus _WalkUntilEnd()
        {
            int jumpLimit = CompilerSettings.JumpLimit;
            bool hasJumpLimitBreak = CompilerSettings.BreakOn.HasFlag(BreakCause.JumpLimitReached) &&
                                     jumpLimit > 0;

            while (true)
            {
                WalkStatus walkStatus = WalkInstruction();
                if (walkStatus != NULL_WALK_STATUS)
                {
                    return walkStatus;
                }

                if (hasJumpLimitBreak && _numOfJumpsThisWalk >= jumpLimit)
                {
                    // Too many jumps, abort
                    return WalkStatus.Break;
                }
            }
        }

        private SourceReference GetSourceReference(int opCodeIndex)
        {
            if (opCodeIndex >= 0 && opCodeIndex < _opCodes.Length)
            {
                return _opCodes[opCodeIndex].Source;
            }

            return SourceReference.ClrSource;
        }
    }
}