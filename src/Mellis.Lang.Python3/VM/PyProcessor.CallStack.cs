using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.VM
{
    public partial class PyProcessor
    {
        internal const int CALL_STACK_LIMIT = 255;

        internal int CallStackCount => _callStacks.Count;

        public void PushCallStack(CallStack callStack)
        {
            if (callStack is null)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_CallStack_PushNull),
                    Localized_Python3_Interpreter.Ex_CallStack_PushNull
                );
            }

            if (_callStacks.Count >= CALL_STACK_LIMIT)
            {
                throw new RuntimeStackOverflowException(callStack.FunctionName);
            }

            _callStacks.Push(callStack);
        }

        public CallStack PopCallStack()
        {
            if (_callStacks.Count == 0)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_CallStack_PopEmpty),
                    Localized_Python3_Interpreter.Ex_CallStack_PopEmpty
                );
            }

            return _callStacks.Pop();
        }

        internal CallStack PeekCallStack()
        {
            if (_callStacks.Count == 0)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_CallStack_PopEmpty),
                    Localized_Python3_Interpreter.Ex_CallStack_PopEmpty
                );
            }

            return _callStacks.Peek();
        }
    }
}