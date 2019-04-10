using System;
using System.Collections.Generic;
using System.Linq;

namespace Mellis.Tools
{
    public static class StringUtilities
    {
        public static int LevenshteinDistance(in string a, in string b)
        {
            // Copied from old compiler
            // https://github.com/zardan/compiler/blob/6b11a0cb4ccbe97cfec1470ab61935e31db276b9/LineParser/ErrorHandler/LevenshteinDist.cs
            if (a is null)
            {
                return b?.Length ?? 0;
            }

            if (b is null)
            {
                return a.Length;
            }

            int n = a.Length;
            int m = b.Length;

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            int[,] d = new int[n + 1, m + 1];

            // Step 2
            for (int i = 0; i <= n; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,
                            d[i, j - 1] + 1
                        ),
                        d[i - 1, j - 1] + cost
                    );
                }
            }

            // Step 7
            return d[n, m];
        }

        public static LevenshteinMatch LevenshteinBestMatch(in string needle, in string[] haystack)
        {
            if (haystack == null)
            {
                return LevenshteinMatch.Null;
            }

            if (needle is null || needle.Length == 0)
            {
                return LevenshteinMatch.Null;
            }

            string bestMatch = null;
            int bestDistance = int.MaxValue;

            foreach (string item in haystack.OrderBy(o => o, StringComparer.OrdinalIgnoreCase))
            {
                int distance = LevenshteinDistance(in needle, in item);
                if (distance >= bestDistance)
                {
                    continue;
                }

                bestDistance = distance;
                bestMatch = item;
            }

            if (bestMatch is null)
            {
                return LevenshteinMatch.Null;
            }

            return new LevenshteinMatch(bestMatch, bestDistance);
        }

        public static LevenshteinMatch LevenshteinBestMatchFiltered(in string needle, in string[] haystack)
        {
            LevenshteinMatch result = LevenshteinBestMatch(in needle, in haystack);

            if (result.IsNull)
            {
                return LevenshteinMatch.Null;
            }

            // threshold: average length / 4 + 1 [using floor division]
            int threshold = (needle.Length + result.value.Length) / 8 + 1;
            if (result.distance > threshold)
            {
                return LevenshteinMatch.Null;
            }

            return result;
        }
    }
}