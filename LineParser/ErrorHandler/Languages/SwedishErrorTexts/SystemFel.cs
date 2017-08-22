using System;

namespace ErrorHandler
{
	public class SystemFel : SystemFailureErrors
	{
		#region Errors from SyntaxCheck

		/// Called if something goes wrong in the logic when parsing. A unique error code is added to know where it got triggered
		public string unknownLogic (string[] arg)
		{
			return "Okänd kombination av kod. Felkod: " + arg[0];
		}

		/// Called if something goes wrong with the parsing of scopes
		public string scopeParsingMalfunction (string[] arg)
		{
			return "Något gick fel vid parsing av scopes";
		}

		#endregion



		#region Errors from Runtime
		/// Called if (startIndex < logicOrder.Length && endIndex < logicOrder.Length) == true
		/// Error could be caused by corrupt And/Or statement
		public string corruptAndOrStatement (string[] arg)
		{
			return "Något är fel vid And/Or";
		}

		public string textParsingMalfunction (string[] arg)
		{
			return "Något gick galet med text-parsingen";
		}

		/// Called if parsing of a comparion in if statement fails and does not return other error
		public string possibleComparissonStatements (string[] arg)
		{
			return "Okänt format av jämförelsen i  if-satsen";
		}

		// Called if the variableType is not supported (eg unknown, unsigned)
		public string addOrChangeUnsupportedVariableType(string[] arg){
			return "Typen av data du försöker lägga in i variabeln stöds inte i detta moment";
		}
		#endregion
	}
}

