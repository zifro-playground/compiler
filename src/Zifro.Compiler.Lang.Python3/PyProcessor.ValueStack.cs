using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Resources;

namespace Zifro.Compiler.Lang.Python3
{
    public partial class PyProcessor
    {
        internal int ValueStackCount => _valueStack.Count;

        internal IScriptType PopValue()
        {
            if (_valueStack.Count == 0)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_ValueStack_PopEmpty),
                    Localized_Python3_Interpreter.Ex_ValueStack_PopEmpty);
            }

            return _valueStack.Pop();
        }

        internal void PushValue(IScriptType value)
        {
            if (value is null)
            {
                throw new InternalException(
                    nameof(Localized_Python3_Interpreter.Ex_ValueStack_PushNull),
                    Localized_Python3_Interpreter.Ex_ValueStack_PushNull);
            }

            _valueStack.Push(value);
        }
    }
}