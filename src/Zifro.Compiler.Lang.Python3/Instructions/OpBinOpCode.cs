using System;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Exceptions;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Exceptions;
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
                case OperatorCode.AAdd:
                    return lhs.ArithmeticAdd(rhs);
                case OperatorCode.ASub:
                    return lhs.ArithmeticSubtract(rhs);
                case OperatorCode.AMul:
                    return lhs.ArithmeticMultiply(rhs);
                case OperatorCode.ADiv:
                    return lhs.ArithmeticDivide(rhs);
                case OperatorCode.AFlr:
                    return lhs.ArithmeticFloorDivide(rhs);
                case OperatorCode.AMod:
                    return lhs.ArithmeticModulus(rhs);
                case OperatorCode.APow:
                    return lhs.ArithmeticExponent(rhs);

                // Binary operators (lhs op rhs)
                case OperatorCode.BAnd: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "&");
                case OperatorCode.BLsh: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "<<");
                case OperatorCode.BRsh: throw new SyntaxNotYetImplementedExceptionKeyword(Source, ">>");
                case OperatorCode.BOr: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "|");
                case OperatorCode.BXor: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "^");

                case OperatorCode.CEq: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "==");
                case OperatorCode.CNEq: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "!=");
                case OperatorCode.CGt: throw new SyntaxNotYetImplementedExceptionKeyword(Source, ">");
                case OperatorCode.CGtEq: throw new SyntaxNotYetImplementedExceptionKeyword(Source, ">=");
                case OperatorCode.CLt: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "<");
                case OperatorCode.CLtEq: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "<=");

                case OperatorCode.LAnd: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "&&");
                case OperatorCode.LOr: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "||");

                // Unary operators (op rhs)
                case OperatorCode.ANeg: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "+");
                case OperatorCode.APos: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "-");
                case OperatorCode.BNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "~");
                case OperatorCode.LNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "!");

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