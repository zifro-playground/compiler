using System;

namespace ErrorHandler
{
	public class TextFel : TextErrors
	{
		#region TextErrors implementation

		public string expectedPlusSignBetweenStrings (string[] arg)
		{
			return "Det förväntas vara ett \"+\" mellan textsträngarna";
		}

		public string expectedATextValue (string[] arg)
		{
			return "Det förväntades ett text värde";
		}

		public string expressionNeedsToEndWithAString (string[] arg)
		{
			return "Utrycket måste avslutas med en text";
		}

		public string textParsingMalfunction (string[] arg)
		{
			return "Något gick galet med text parsingen";
		}

		#endregion

	}
}

