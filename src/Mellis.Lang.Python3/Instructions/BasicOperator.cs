using System.ComponentModel;
using Mellis.Core.Entities;
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

        public void Execute(VM.PyProcessor processor)
        {
            IScriptType result = Code.IsBinary()
                ? GetBinaryResult(processor)
                : GetUnaryResult(processor);

            processor.PushValue(result);
        }

        private IScriptType GetBinaryResult(VM.PyProcessor processor)
        {
            IScriptType rhs = processor.PopValue();
            IScriptType lhs = processor.PopValue();

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

                case OperatorCode.BAnd:
                    return lhs.BinaryAnd(rhs);
                case OperatorCode.BLsh:
                    return lhs.BinaryLeftShift(rhs);
                case OperatorCode.BRsh:
                    return lhs.BinaryRightShift(rhs);
                case OperatorCode.BOr:
                    return lhs.BinaryOr(rhs);
                case OperatorCode.BXor:
                    return lhs.BinaryXor(rhs);

                case OperatorCode.CEq:
                    return lhs.CompareEqual(rhs);
                case OperatorCode.CNEq:
                    return lhs.CompareNotEqual(rhs);
                case OperatorCode.CGt:
                    return lhs.CompareGreaterThan(rhs);
                case OperatorCode.CGtEq:
                    return lhs.CompareGreaterThanOrEqual(rhs);
                case OperatorCode.CLt:
                    return lhs.CompareLessThan(rhs);
                case OperatorCode.CLtEq:
                    return lhs.CompareLessThanOrEqual(rhs);

                case OperatorCode.LAnd: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "and");
                case OperatorCode.LOr: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "or");

                case OperatorCode.CIn: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "in");
                case OperatorCode.CNIn: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not in");
                case OperatorCode.CIs: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is");
                case OperatorCode.CIsN: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is not");

                default:
                    throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(OperatorCode));
            }
        }

        private IScriptType GetUnaryResult(VM.PyProcessor processor)
        {
            IScriptType lhs = processor.PopValue();

            switch (Code)
            {
                // Unary operators (op rhs)
                case OperatorCode.ANeg: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "+");
                case OperatorCode.APos: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "-");
                case OperatorCode.BNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "~");
                case OperatorCode.LNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not");

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