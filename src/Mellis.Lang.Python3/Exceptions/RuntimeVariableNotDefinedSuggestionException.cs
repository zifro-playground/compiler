using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Exceptions
{
    public class RuntimeVariableNotDefinedSuggestionException : RuntimeException
    {
        public string VariableSuggestion { get; }

        public RuntimeVariableNotDefinedSuggestionException(string undefined, string suggestion)
            : base(
                nameof(Localized_Python3_Runtime.Ex_Variable_NotDefined_Suggestion),
                Localized_Python3_Runtime.Ex_Variable_NotDefined_Suggestion,
                undefined,
                suggestion
            )
        {
            VariableSuggestion = suggestion;
        }
    }
}