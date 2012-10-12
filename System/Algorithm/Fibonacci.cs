namespace System.Algorithm
{
    using Properties;

    /// <summary>
    /// A class generating Fibonacci numbers.  Note that this class is far from complete, or efficient - it
    /// will need to be implemented with the golden ratio, as seen at http://www.math.utah.edu/~beebe/software/java/fibonacci/.
    /// </summary>
    public static class Fibonacci
    {
        /// <summary>
        /// Generates the Nth Fibonacci number.
        /// </summary>
        /// <param name="n">The Nth Fibonacci number.</param>
        /// <returns>The Nth number in the Fibonacci sequence.</returns>
        public static long GenerateNthFibonacci(int n)
        {
            return GenerateFibonacciSequence(n)[n];
        }

        /// <summary>
        /// Generates the Nth Fibonacci number.
        /// </summary>
        /// <param name="upperBoundN">The upper bound N.</param>
        /// <returns>A series of Fibonacci numbers until the upperBoundN number.</returns>
        public static long[] GenerateFibonacciSequence(int upperBoundN)
        {
            if (upperBoundN < 0) throw new ArgumentOutOfRangeException(Resources.SetIndexMustBePostive);
            var numbers = new long[upperBoundN + 1];
            numbers[0] = 0;
            if (upperBoundN >= 1)
            {
                numbers[1] = 1;
                for (var i = 2; i <= upperBoundN; ++i)
                {
                    numbers[i] = numbers[i - 1] + numbers[i - 2];
                }
            }

            return numbers;
        }
    }
}
