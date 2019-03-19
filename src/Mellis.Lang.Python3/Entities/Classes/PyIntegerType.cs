using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyIntegerType : PyType<PyInteger>
    {
        public PyIntegerType(
            IProcessor processor,
            string name = null)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_Int_Name,
                name: name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyIntegerType(Processor, newName);
        }

        public override IScriptType Invoke(IScriptType[] arguments)
        {
            if (arguments.Length > 2)
            {
                throw new RuntimeTooManyArgumentsException(
                    ClassName, 2, arguments.Length);
            }

            if (arguments.Length == 0)
            {
                return new PyInteger(Processor, 0);
            }

            IScriptType arg1 = arguments[0];

            // Two args: int(x, y)
            if (arguments.Length == 2)
            {
                IScriptType arg2 = arguments[1];

                if (!(arg2 is PyInteger arg2Int))
                {
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg2_Type),
                        Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg2_Type,
                        arg2.GetTypeName()
                    );
                }

                if (!(arg1 is PyString arg1Str))
                {
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NotString_Arg2_Int),
                        Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NotString_Arg2_Int,
                        arg1.GetTypeName()
                    );
                }

                return arg2Int.Value == 0
                    ? new PyInteger(Processor, ParseZeroBase(arg1Str))
                    : new PyInteger(Processor, ParseWithNonZeroBase(arg1Str, arg2Int.Value));
            }

            // One arg: int(x)
            switch (arg1)
            {
                case PyDouble posInf when double.IsPositiveInfinity(posInf.Value):
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_PosInf),
                        Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_PosInf
                    );

                case PyDouble negInf when double.IsNegativeInfinity(negInf.Value):
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NegInf),
                        Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NegInf
                    );

                case PyDouble nan when double.IsNaN(nan.Value):
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NaN),
                        Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NaN
                    );

                case PyDouble arg1Double:
                    try
                    {
                        return new PyInteger(Processor, checked((int) arg1Double.Value));
                    }
                    catch (OverflowException)
                    {
                        throw new RuntimeException(
                            nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_DoubleOutOfBounds),
                            Localized_Python3_Entities.Ex_IntegerType_Ctor_DoubleOutOfBounds,
                            arg1Double.ToString()
                        );
                    }

                case PyString arg1Str:
                    return new PyInteger(Processor, ParseWithNonZeroBase(arg1Str, 10));

                default:
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_Type),
                        Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_Type,
                        arg1.GetTypeName()
                    );
            }
        }

        private static int ParseZeroBase(PyString strType)
        {
            string trimmed = strType.Value.Trim().ToLowerInvariant();

            // Empty string?
            if (trimmed.Length == 0)
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_EmptyString),
                    Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_EmptyString
                );
            }

            try
            {
                LiteralInteger literal = LiteralInteger.Parse(SourceReference.ClrSource, trimmed);

                return literal.Value;
            }
            catch (SyntaxNotYetImplementedException)
            {
                throw;
            }
            catch
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_InvalidString),
                    Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_InvalidString,
                    strType.ToString()
                );
            }
        }

        private static int ParseWithNonZeroBase(PyString strType, int numBase)
        {
            try
            {
                return LiteralInteger.ParseWithNonZeroBase(strType.Value, numBase);
            }
            catch (ArgumentOutOfRangeException e) when (e.ParamName == "numBase")
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg2_OutOfRange),
                    Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg2_OutOfRange,
                    numBase
                );
            }
            catch (ArgumentException e) when (e.ParamName == "text")
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_EmptyString),
                    Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_EmptyString
                );
            }
            catch (FormatException)
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_InvalidString),
                    Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_InvalidString,
                    strType.ToString()
                );
            }
            catch (OverflowException)
            {
                throw new RuntimeException(
                    nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_StringOutOfBounds),
                    Localized_Python3_Entities.Ex_IntegerType_Ctor_StringOutOfBounds,
                    strType.ToString()
                );
            }
        }
    }
}