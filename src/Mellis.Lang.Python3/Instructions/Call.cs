using System.Collections.Generic;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
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

        public void Execute(VM.PyProcessor processor)
        {
            // Load arguments in reverse because of popping
            var arguments = new IScriptType[ArgumentCount];
            for (int i = ArgumentCount - 1; i >= 0; i--)
            {
                arguments[i] = processor.PopValue();
            }

            // Get value to call
            IScriptType function = processor.PopValue();


            if (function is PyClrFunction clrFunction)
            {
                // Push stack
                processor.PushCallStack(new CallStack(Source, clrFunction.Definition.FunctionName, ReturnAddress));

                // Invoke it
                IScriptType result = clrFunction.Definition.Invoke(arguments);

                // Push value
                processor.PushValue(result ?? processor.Factory.Null);

                // Return
                processor.JumpToInstruction(ReturnAddress);
            }
            else
            {
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "user function");
            }
        }

        public override string ToString()
        {
            return $"call#{ArgumentCount}@{ReturnAddress}";
        }
    }
}