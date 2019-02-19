using System;
using System.ComponentModel;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Instructions
{
    public class BasicOperator : IOpCode
    {
        public OperatorCode Code { get; }

        public SourceReference Source { get; }

        public BasicOperator(SourceReference source, OperatorCode code)
        {
            Source = source;
            Code = code;
        }

        public void Execute(PyProcessor processor)
        {
            IScriptType result = Code.IsBinary()
                ? GetBinaryResult(processor)
                : GetUnaryResult(processor);

            processor.PushValue(result);
        }

        private IScriptType GetBinaryResult(PyProcessor processor)
        {
            var rhs = processor.PopValue();
            var lhs = processor.PopValue();

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

                case OperatorCode.CIn: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "in");
                case OperatorCode.CNIn: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not in");
                case OperatorCode.CIs: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is");
                case OperatorCode.CIsN: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is not");

                case OperatorCode.ANeg: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "-");
                case OperatorCode.APos: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "+");
                case OperatorCode.BNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "~");
                case OperatorCode.LNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not");

                default:
                    throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(OperatorCode));
            }
        }

        private IScriptType GetUnaryResult(PyProcessor processor)
        {
            var lhs = processor.PopValue();

            switch (Code)
            {
                // Unary operators (op rhs)
                case OperatorCode.ANeg: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "+");
                case OperatorCode.APos: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "-");
                case OperatorCode.BNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "~");
                case OperatorCode.LNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "!");

                default:
                    throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(OperatorCode));
            }
        }

        public override string ToString()
        {
            // BAnd => band
            // AAdd => add
            // CEq => eq
            string name = Code.ToString().ToLowerInvariant();
            return name[0] != 'b'
                ? name.Substring(1)
                : name;
        }
    }
}