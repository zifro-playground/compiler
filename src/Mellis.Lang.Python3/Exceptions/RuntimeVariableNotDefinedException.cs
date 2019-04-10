using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public class RuntimeVariableNotDefinedException : RuntimeException
    {
        public string VariableUndefined { get; }

        public RuntimeVariableNotDefinedException(string undefined)
            : this(
                nameof(Localized_Python3_Runtime.Ex_Variable_NotDefined),
                Localized_Python3_Runtime.Ex_Variable_NotDefined,
                undefined: undefined
            )
        {
        }

        internal RuntimeVariableNotDefinedException(
            string localizeKey,
            string localizedMessageFormat,
            string undefined,
            params object[] extraFormatArgs)
            : base(
                localizeKey,
                localizedMessageFormat,
                extraFormatArgs.Prepend(undefined).ToArray()
            )
        {
            VariableUndefined = undefined;
        }
    }
}