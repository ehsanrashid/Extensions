namespace System.Collections.Generic
{
    /// <summary>
    /// An interface for a Mathematical matrix as in Linear Algebra.
    /// </summary>
	public interface IMathematicalMatrix : IMatrix<double>
	{
		/// <summary>
		/// Gets a value indicating whether this matrix instance is symmetric.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this matrix instance is symmetric; otherwise, <c>false</c>.
		/// </value>
		bool IsSymmetric { get; }
		
		/// <summary>
		/// Inverts this instance.  All values are multiplied with -1.
		/// </summary>
		/// <returns>The inverted representation of this instance.</returns>
		IMathematicalMatrix Invert();

		/// <summary>
		/// Subtracts the matrices according to the linear algebra operator -.
		/// </summary>
		/// <param name="matrix">The result of the subtraction.</param>
		/// <returns>The result of the minus operation.</returns>
		IMathematicalMatrix Minus(IMathematicalMatrix matrix);

		/// <summary>
		/// Adds to matrices according to the linear algebra operator +.
		/// </summary>
		/// <param name="matrix">The result of the addition.</param>
		/// <returns>The result of the plus operation.</returns>
		IMathematicalMatrix Plus(IMathematicalMatrix matrix);

        /// <summary>
        /// Times the matrices according to the linear algebra operator *.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The result of the times operation.</returns>
		IMathematicalMatrix Times(IMathematicalMatrix matrix);

		/// <summary>
		/// Times the matrices according to the linear algebra operator *.
		/// </summary>
		/// <param name="number">The number to multiply this matrix with.</param>
		/// <returns>The result of the times operation.</returns>
		IMathematicalMatrix Times(double number);

		/// <summary>
		/// Transposes the matrix.
		/// </summary>
		/// <returns>The transposed representation of this matrix.</returns>
		/// <value>The transposed matrix.</value>
		IMathematicalMatrix Transpose();
	}
}
