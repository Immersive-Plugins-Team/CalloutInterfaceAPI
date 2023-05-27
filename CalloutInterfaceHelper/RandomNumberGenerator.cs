namespace CalloutInterfaceAPI
{
    /// <summary>
    /// A helper class for generating random numbers.
    /// </summary>
    internal static class RandomNumberGenerator
    {
        private static readonly System.Random Random = new System.Random();

        /// <summary>
        /// Gets the next random number.
        /// </summary>
        /// <param name="minValue">The beginning of the range (inclusive).</param>
        /// <param name="maxValue">The end of the range (exclusive).</param>
        /// <returns>A random number.</returns>
        public static int Next(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }
    }
}
