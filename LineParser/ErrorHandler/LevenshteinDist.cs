using System;
using System.Collections.Generic;
using System.Linq;
using Runtime;

namespace Compiler
{
	public class LevenshteinDist
	{
		private static int variableMaxDist = 2;

		public static void checkForClosesVariable(string notFoundName, int lineNumber, Scope currentScope){
			List<LeveshteinResult> results = new List<LeveshteinResult> ();

			foreach (Variable v in currentScope.scopeVariables.variableList)
				if (v.name != notFoundName)
					results.Add (new LeveshteinResult(CalcEditDist (notFoundName, v.name), v.name));
			
			results = results.OrderBy (x => x.dist).ToList ();


			if (results.Count > 0 && results [0].dist <= variableMaxDist)
				ErrorHandler.ErrorMessage.sendErrorMessage (lineNumber, string.Format("Hittar inte variabeln \"{0}\" i minnet.\nMenade du \"{1}\"?", notFoundName, results[0].word));
		}

		private class LeveshteinResult{
			public int dist;
			public string word;

			public LeveshteinResult(int dist, string word){
				this.dist = dist;
				this.word = word;
			}
		}


		/// Compute the distance between two strings.
		public static int CalcEditDist(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
				return m;

			if (m == 0)
				return n;

			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					// Step 6
					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			// Step 7
			return d[n, m];
		}
	}
}

