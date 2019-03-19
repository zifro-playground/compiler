using System;
using System.Globalization;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities.Classes
{
    public class PyDoubleType : PyType<PyDouble>
    {
        public PyDoubleType(
            IProcessor processor,
            string name = null)
            : base(
                processor: processor,
                className: Localized_Base_Entities.Type_Double_Name,
                name: name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            return new PyDoubleType(Processor, newName);
        }

        public override IScriptType Invoke(IScriptType[] arguments)
        {
            if (arguments.Length > 1)
            {
                throw new RuntimeTooManyArgumentsException(
                    ClassName, 1, arguments.Length);
            }

            if (arguments.Length == 0)
            {
                return new PyDouble(Processor, 0);
            }

            IScriptType arg1 = arguments[0];
            switch (arg1)
            {
                case PyString strType:
                    string trimmed = strType.Value.Trim();

                    // Empty string?
                    if (trimmed.Length == 0)
                    {
                        throw new RuntimeException(
                            nameof(Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_EmptyString),
                            Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_EmptyString
                        );
                    }

                    // Same as localized representation of special values?
                    if (trimmed == Localized_Base_Entities.Type_Double_PosInfinity)
                    {
                        return new PyDouble(Processor, double.PositiveInfinity);
                    }
                    else if (trimmed == Localized_Base_Entities.Type_Double_NegInfinity)
                    {
                        return new PyDouble(Processor, double.NegativeInfinity);
                    }
                    else if (trimmed == Localized_Base_Entities.Type_Double_NaN)
                    {
                        return new PyDouble(Processor, double.NaN);
                    }

                    // Python predefined
                    switch (trimmed.ToLowerInvariant())
                    {
                        case "infinity":
                        case "+infinity":
                        case "inf":
                        case "+inf":
                            return new PyDouble(Processor, double.PositiveInfinity);

                        case "-infinity":
                        case "-inf":
                            return new PyDouble(Processor, double.NegativeInfinity);

                        case "nan":
                        case "+nan":
                        case "-nan":
                            return new PyDouble(Processor, double.NaN);
                    }

                    // Convert number, 0.1 or 1e10 format
                    const NumberStyles numberStyles = NumberStyles.AllowExponent |
                                                      NumberStyles.AllowDecimalPoint |
                                                      NumberStyles.AllowLeadingSign;

                    try
                    {
                        double result = double.Parse(trimmed, numberStyles, CultureInfo.InvariantCulture);
                        return new PyDouble(Processor, result);
                    }
                    catch (OverflowException) when (trimmed[0] == '-')
                    {
                        return new PyDouble(Processor, double.NegativeInfinity);
                    }
                    catch (OverflowException)
                    {
                        return new PyDouble(Processor, double.PositiveInfinity);
                    }
                    catch
                    {
                        // What is the meaning of this value?
                        throw new RuntimeException(
                            nameof(Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_InvalidString),
                            Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_InvalidString,
                            strType.ToString()
                        );
                    }

                case PyBoolean boolType:
                    return new PyDouble(Processor, boolType.Value ? 1d : 0d);

                case PyDouble doubleType:
                    return doubleType.Copy(null);

                case PyInteger intType:
                    return new PyDouble(Processor, intType.Value);

                default:
                    throw new RuntimeException(
                        nameof(Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_Type),
                        Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_Type,
                        arg1.GetTypeName()
                    );
            }
        }
    }
}