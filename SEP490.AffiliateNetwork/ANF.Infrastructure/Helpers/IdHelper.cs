namespace ANF.Infrastructure.Helpers
{
    /// <summary>
    /// IdHelper
    /// </summary>
    public static class IdHelper
    {
        private static Random _random = new Random();

        public static long GenerateRandomLong()
        {
            // The maximum value for a 10-digit number is 9,999,999,999
            const long maxValue = 9999999999L;

            // Using NextLong which is available in .NET 6 and later
            return _random.NextInt64(1, maxValue + 1);
        }

    }
}