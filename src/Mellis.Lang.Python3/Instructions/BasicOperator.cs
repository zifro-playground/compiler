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
        public BasicOperatorCode Code { get; }

        public SourceReference Source { get; }

        public BasicOperator(SourceReference source, BasicOperatorCode code)
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
                case BasicOperatorCode.AAdd:
                    return lhs.ArithmeticAdd(rhs);
                case BasicOperatorCode.ASub:
                    return lhs.ArithmeticSubtract(rhs);
                case BasicOperatorCode.AMul:
                    return lhs.ArithmeticMultiply(rhs);
                case BasicOperatorCode.ADiv:
                    return lhs.ArithmeticDivide(rhs);
                case BasicOperatorCode.AFlr:
                    return lhs.ArithmeticFloorDivide(rhs);
                case BasicOperatorCode.AMod:
                    return lhs.ArithmeticModulus(rhs);
                case BasicOperatorCode.APow:
                    return lhs.ArithmeticExponent(rhs);

                case BasicOperatorCode.BAnd:
                    return lhs.BinaryAnd(rhs);
                case BasicOperatorCode.BLsh:
                    return lhs.BinaryLeftShift(rhs);
                case BasicOperatorCode.BRsh:
                    return lhs.BinaryRightShift(rhs);
                case BasicOperatorCode.BOr:
                    return lhs.BinaryOr(rhs);
                case BasicOperatorCode.BXor:
                    return lhs.BinaryXor(rhs);

                case BasicOperatorCode.CEq:
                    return lhs.CompareEqual(rhs);
                case BasicOperatorCode.CNEq:
                    return lhs.CompareNotEqual(rhs);
                case BasicOperatorCode.CGt:
                    return lhs.CompareGreaterThan(rhs);
                case BasicOperatorCode.CGtEq:
                    return lhs.CompareGreaterThanOrEqual(rhs);
                case BasicOperatorCode.CLt:
                    return lhs.CompareLessThan(rhs);
                case BasicOperatorCode.CLtEq:
                    return lhs.CompareLessThanOrEqual(rhs);

                case BasicOperatorCode.LAnd: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "and");
                case BasicOperatorCode.LOr: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "or");

                case BasicOperatorCode.CIn: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "in");
                case BasicOperatorCode.CNIn: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not in");
                case BasicOperatorCode.CIs: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is");
                case BasicOperatorCode.CIsN: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is not");

                default:
                    throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(BasicOperatorCode));
            }
        }

        private IScriptType GetUnaryResult(VM.PyProcessor processor)
        {
            IScriptType lhs = processor.PopValue();

            switch (Code)
            {
                // Unary operators (op rhs)
                case BasicOperatorCode.ANeg: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "+");
                case BasicOperatorCode.APos: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "-");
                case BasicOperatorCode.BNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "~");
                case BasicOperatorCode.LNot: throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not");

                default:
                    throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(BasicOperatorCode));
            }
        }

        public override string ToString()
        {
            // BAnd => op->band
            // AAdd => op->add
            // CEq => op->eq
            string name = Code.ToString().ToLowerInvariant();
            return "op->"+(name[0] != 'b'
                ? name.Substring(1)
                : name);
        }
    }
}