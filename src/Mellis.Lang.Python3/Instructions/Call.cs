using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3.Instructions
{
    public class Call : IOpCode
    {
        public int ArgumentCount { get; }

        public int ReturnAddress { get; }

        public SourceReference Source { get; }

        public Call(SourceReference source, int argumentCount, int returnAddress)
        {
            Source = source;
            ArgumentCount = argumentCount;
            ReturnAddress = returnAddress;
        }

        public void Execute(PyProcessor processor)
        {
            // Load arguments in reverse because of popping
            var arguments = new IScriptType[ArgumentCount];
            for (int i = ArgumentCount - 1; i >= 0; i--)
            {
                arguments[i] = processor.PopValue();
            }

            // Get value to call
            IScriptType function = processor.PopValue();

            if (function is IClrYieldingFunction yielding)
            {
                CallYieldingClr(processor, arguments, yielding);
            }
            else if (function is IClrFunction clrFunction)
            {
                CallClr(processor, arguments, clrFunction);
            }
            else
            {
                throw new RuntimeException(
                    nameof(Localized_Base_Entities.Ex_Base_Invoke),
                    Localized_Base_Entities.Ex_Base_Invoke,
                    function.GetTypeName()
                );
            }
        }

        private void CallClr(PyProcessor processor, IScriptType[] arguments, IClrFunction clrFunction)
        {
            // Push stack
            processor.PushCallStack(new CallStack(Source, clrFunction.FunctionName, ReturnAddress));

            // Invoke it
            IScriptType result = clrFunction.Invoke(arguments);

            // Push value
            processor.PushValue(result ?? processor.Factory.Null);

            // Pop stack
            processor.PopCallStack();

            // Return
            processor.JumpToInstruction(ReturnAddress);
        }

        private void CallYieldingClr(PyProcessor processor, IScriptType[] arguments, IClrYieldingFunction clrFunction)
        {
            // Push stack
            processor.PushCallStack(new CallStack(Source, clrFunction.FunctionName, ReturnAddress));

            // Yield the processor
            processor.Yield(new YieldData(
                arguments, clrFunction
            ));

            // Invoke start
            clrFunction.InvokeEnter(arguments);
        }

        public override string ToString()
        {
            return $"call#{ArgumentCount}@{ReturnAddress}";
        }
    }
}