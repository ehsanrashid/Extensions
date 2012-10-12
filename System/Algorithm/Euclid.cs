namespace System.Algorithm
{
    using Properties;

    /// <summary>
    /// A class using Euclid's algorithm for providing the greatest common divisor.
    /// </summary>
    public static class Euclid
    {

        /// <summary>
        /// Finds the greatest common divisor.
        /// </summary>
        /// <param name="x">The first number.  Must be larger than y.</param>
        /// <param name="y">The second number</param>
        /// <returns>The greatest common divisor between the two integers supplied.</returns>
        public static int FindGreatestCommonDivisor(int x, int y)
        {
            if ((y < 0) || (x < 0)) throw new ArgumentException(Resources.NumbersGreaterThanZero);
            // This algorithm only works if x >= y.
            // If x < y, swap the two variables.
            if (x < y)
            {
                var tmp = x;
                x = y;
                y = tmp;
            }

            return FindGreatestCommonDivisorR(x, y);
        }


        private static int FindGreatestCommonDivisorR(int x, int y)
        {
            return (y == 0) ? x : FindGreatestCommonDivisorR(y, x % y);
        }


    }
}
