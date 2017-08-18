using System;

namespace ErrorHandler
{
	public class TextFel : TextErrors
	{
		#region Errors from SyntaxCheck

		/// Should this error really exist? You can add strings to eachother without + in Python
		public string expectedPlusSignBetweenStrings (string[] arg)
		{
			return "Det förväntas vara ett \"+\" mellan textsträngarna";
		}



		#endregion



		#region Errors from Runtime
		public string expectedATextValue (string[] arg)
		{
			return "Det förväntades ett text värde";
		}

		public string expressionNeedsToEndWithAString (string[] arg)
		{
			return "Utrycket måste avslutas med en text";
		}
		#endregion
	}
}

