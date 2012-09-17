namespace System
{
    /// <summary>
    /// Contains extension methods for the <see cref="System.Decimal"/> class
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// Rounds the supplied decimal to the specified amount of decimal points
        /// </summary>
        /// <param name="value">The decimal to round</param>
        /// <param name="decimalPoints">The number of decimal points to round the output value to</param>
        /// <returns>A rounded decimal</returns>
        public static Decimal RoundDecimalPoints(this Decimal value, int decimalPoints = 4) { return Math.Round(value, decimalPoints); }

        /// <summary>
        /// Rounds the supplied decimal value to two decimal points
        /// </summary>
        /// <param name="value">The decimal to round</param>
        /// <returns>A decimal value rounded to two decimal points</returns>
        public static Decimal RoundToTwoDecimalPoints(this Decimal value) { return Math.Round(value, 2); }
    }
}