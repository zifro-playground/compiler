using System;
using System.ComponentModel;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.VM;

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

        public void Execute(PyProcessor processor)
        {
            IScriptType result = Code.IsBinary()
                ? GetBinaryResult(processor)
                : GetUnaryResult(processor);

            processor.PushValue(result);
        }

        private Func<IScriptType, IScriptType> GetBinaryMethod(IScriptType value)
        {
            switch (Code)
            {
            case BasicOperatorCode.AAdd: return value.ArithmeticAdd;
            case BasicOperatorCode.ASub: return value.ArithmeticSubtract;
            case BasicOperatorCode.AMul: return value.ArithmeticMultiply;
            case BasicOperatorCode.ADiv: return value.ArithmeticDivide;
            case BasicOperatorCode.AFlr: return value.ArithmeticFloorDivide;
            case BasicOperatorCode.AMod: return value.ArithmeticModulus;
            case BasicOperatorCode.APow: return value.ArithmeticExponent;

            case BasicOperatorCode.BAnd: return value.BinaryAnd;
            case BasicOperatorCode.BLsh: return value.BinaryLeftShift;
            case BasicOperatorCode.BRsh: return value.BinaryRightShift;
            case BasicOperatorCode.BOr: return value.BinaryOr;
            case BasicOperatorCode.BXor: return value.BinaryXor;

            case BasicOperatorCode.CEq: return value.CompareEqual;
            case BasicOperatorCode.CNEq: return value.CompareNotEqual;
            case BasicOperatorCode.CGt: return value.CompareGreaterThan;
            case BasicOperatorCode.CGtEq: return value.CompareGreaterThanOrEqual;
            case BasicOperatorCode.CLt: return value.CompareLessThan;
            case BasicOperatorCode.CLtEq: return value.CompareLessThanOrEqual;

            case BasicOperatorCode.CIn:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "in");
            case BasicOperatorCode.CNIn:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not in");
            case BasicOperatorCode.CIs:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is");
            case BasicOperatorCode.CIsN:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is not");

            default:
                throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(BasicOperatorCode));
            }
        }

        private Func<IScriptType, IScriptType> GetBinaryMethodReverse(IScriptType value)
        {
            switch (Code)
            {
            case BasicOperatorCode.AAdd: return value.ArithmeticAddReverse;
            case BasicOperatorCode.ASub: return value.ArithmeticSubtractReverse;
            case BasicOperatorCode.AMul: return value.ArithmeticMultiplyReverse;
            case BasicOperatorCode.ADiv: return value.ArithmeticDivideReverse;
            case BasicOperatorCode.AFlr: return value.ArithmeticFloorDivideReverse;
            case BasicOperatorCode.AMod: return value.ArithmeticModulusReverse;
            case BasicOperatorCode.APow: return value.ArithmeticExponentReverse;

            case BasicOperatorCode.BAnd: return value.BinaryAndReverse;
            case BasicOperatorCode.BLsh: return value.BinaryLeftShiftReverse;
            case BasicOperatorCode.BRsh: return value.BinaryRightShiftReverse;
            case BasicOperatorCode.BOr: return value.BinaryOrReverse;
            case BasicOperatorCode.BXor: return value.BinaryXorReverse;

            case BasicOperatorCode.CEq: return value.CompareEqual;
            case BasicOperatorCode.CNEq: return value.CompareNotEqual;
            case BasicOperatorCode.CGt: return value.CompareLessThan;
            case BasicOperatorCode.CGtEq: return value.CompareLessThanOrEqual;
            case BasicOperatorCode.CLt: return value.CompareGreaterThan;
            case BasicOperatorCode.CLtEq: return value.CompareGreaterThanOrEqual;

            case BasicOperatorCode.CIn:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "in");
            case BasicOperatorCode.CNIn:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "not in");
            case BasicOperatorCode.CIs:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is");
            case BasicOperatorCode.CIsN:
                throw new SyntaxNotYetImplementedExceptionKeyword(Source, "is not");

            default:
                throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(BasicOperatorCode));
            }
        }

        private IScriptType GetBinaryResult(PyProcessor processor)
        {
            IScriptType rhs = processor.PopValue();
            IScriptType lhs = processor.PopValue();

            IScriptType result = GetBinaryMethod(lhs).Invoke(rhs) ??
                                 GetBinaryMethodReverse(rhs).Invoke(lhs);

            if (result == null)
            {
                throw new RuntimeException(
                    nameof(Localized_Base_Entities.Ex_Base_OperatorInvalidType),
                    Localized_Base_Entities.Ex_Base_OperatorInvalidType,
                    lhs.GetTypeName(),
                    rhs.GetTypeName(),
                    GetBasicOperatorString(Code)
                );
            }

            return result;
        }

        private IScriptType GetUnaryResult(PyProcessor processor)
        {
            IScriptType lhs = processor.PopValue();

            switch (Code)
            {
            // Unary operators (op rhs)
            case BasicOperatorCode.ANeg:
                return lhs.ArithmeticUnaryNegative();
            case BasicOperatorCode.APos:
                return lhs.ArithmeticUnaryPositive();
            case BasicOperatorCode.BNot:
                return lhs.BinaryNot();

            case BasicOperatorCode.LNot:
                return processor.Factory.Create(!lhs.IsTruthy());

            default:
                throw new InvalidEnumArgumentException(nameof(Code), (int)Code, typeof(BasicOperatorCode));
            }
        }
        
        private static string GetBasicOperatorString(BasicOperatorCode code)
        {
            switch (code)
            {
            case BasicOperatorCode.AAdd: return "+";
            case BasicOperatorCode.ASub: return "-";
            case BasicOperatorCode.AMul: return "*";
            case BasicOperatorCode.ADiv: return "/";
            case BasicOperatorCode.AFlr: return "//";
            case BasicOperatorCode.AMod: return "%";
            case BasicOperatorCode.APow: return "**";

            case BasicOperatorCode.BAnd: return "&";
            case BasicOperatorCode.BLsh: return "<<";
            case BasicOperatorCode.BRsh: return ">>";
            case BasicOperatorCode.BOr: return "|";
            case BasicOperatorCode.BXor: return "^";

            case BasicOperatorCode.CEq: return "==";
            case BasicOperatorCode.CNEq: return "!=";
            case BasicOperatorCode.CGt: return ">";
            case BasicOperatorCode.CGtEq: return ">=";
            case BasicOperatorCode.CLt: return "<";
            case BasicOperatorCode.CLtEq: return "<=";

            case BasicOperatorCode.CIn: return "in";
            case BasicOperatorCode.CNIn: return "not in";
            case BasicOperatorCode.CIs: return "is";
            case BasicOperatorCode.CIsN: return "is not";

            case BasicOperatorCode.ANeg: return "-x";
            case BasicOperatorCode.APos: return "+x";
            case BasicOperatorCode.BNot: return "~x";
            case BasicOperatorCode.LNot: return "not x";
            default:
                throw new InvalidEnumArgumentException(nameof(code), (int)code, typeof(BasicOperatorCode));
            }
        }

        public override string ToString()
        {
            // BAnd => op->band
            // AAdd => op->add
            // CEq => op->eq
            string name = Code.ToString().ToLowerInvariant();
            return "op->" + (name[0] != 'b'
                       ? name.Substring(1)
                       : name);
        }
    }
}