namespace Mellis.Tools
{
    public struct LevenshteinMatch
    {
        public readonly string value;
        public readonly int distance;

        public bool IsNull => value is null;

        public LevenshteinMatch(string value, int distance)
        {
            this.value = value;
            this.distance = distance;
        }

        public static readonly LevenshteinMatch Null = new LevenshteinMatch(null, -1);
    }
}