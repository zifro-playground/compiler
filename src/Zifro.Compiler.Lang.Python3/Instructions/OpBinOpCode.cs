using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class OpBinOpCode : IOpCode
    {
        public OperatorCode Code { get; }

        public SourceReference Source { get; }

        public OpBinOpCode(SourceReference source, OperatorCode code)
        {
            Source = source;
            Code = code;
        }

        public void Execute(PyProcessor processor)
        {
            IScriptType result = GetResult(processor);
            processor.PushValue(result);
        }

        private IScriptType GetResult(PyProcessor processor)
        {
            var rhs = processor.PopValue<IScriptType>();
            var lhs = processor.PopValue<IScriptType>();
            switch (Code)
            {
                case OperatorCode.Add:
                    return lhs.ArithmeticAdd(rhs);
                case OperatorCode.Sub:
                    return lhs.ArithmeticSubtract(rhs);
                case OperatorCode.Mul:
                    return lhs.ArithmeticMultiply(rhs);
                case OperatorCode.Div:
                    return lhs.ArithmeticDivide(rhs);
                case OperatorCode.Flr:
                    return lhs.ArithmeticFloorDivide(rhs);
                case OperatorCode.Mod:
                    return lhs.ArithmeticModulus(rhs);
                case OperatorCode.Pow:
                    return lhs.ArithmeticExponent(rhs);
                default:
                    throw new SyntaxNotYetImplementedException(Source);
            }
        }

        public override string ToString()
        {
            return Code.ToString().ToLowerInvariant();
        }
    }
}