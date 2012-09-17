namespace System.Collections.Generic
{
    using Visitors;
    using Properties;

    /// <summary>
    ///   A Matrix datastructure corresponding to the mathematical matrix used in linear algebra.
    /// </summary>
    public class Matrix : IVisitableCollection<double>, ICloneable, IMathematicalMatrix
    {
        private int noOfRows;
        private int noOfColumns;
        private double[,] data;

       
        /// <summary>
        ///   Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="rows_"> The number of rows in the matrix. </param>
        /// <param name="columns_"> The number of columns in the matrix. </param>
        public Matrix(int rows_, int columns_)
        {
            if ((rows_ <= 0) || (columns_ <= 0))
                throw new ArgumentException(Resources.RowsOrColumnsInvalid);
            noOfRows = rows_;
            noOfColumns = columns_;
            data = new double[noOfRows,noOfColumns];
        }
        

        #region IMatrix Members
        IMatrix<double> IMatrix<double>.GetSubMatrix(int rowStart, int columnStart, int rowCount, int columnCount)
        {
            return GetSubMatrix(rowStart, columnStart, rowCount, columnCount);
        }

        /// <summary>
        ///   Gets the number of rows in this matrix.
        /// </summary>
        /// <value> The rows. </value>
        public int Rows
        {
            get { return noOfRows; }
        }

        /// <summary>
        ///   Gets the number of columns in this matrix.
        /// </summary>
        /// <value> The columns. </value>
        public int Columns
        {
            get { return noOfColumns; }
        }

        /// <summary>
        ///   Gets a value indicating whether this matrix is square.
        /// </summary>
        /// <value> <c>true</c> if this matrix is square; otherwise, <c>false</c> . </value>
        public bool IsSquare
        {
            get { return (Rows == Columns); }
        }
        #endregion

        #region IMathematicalMatrix Members
        /// <summary>
        ///   Times the matrices according to the linear algebra operator *.
        /// </summary>
        /// <param name="matrix"> The result of the times operation. </param>
        /// <returns> The result of the times operation. </returns>
        IMathematicalMatrix IMathematicalMatrix.Times(IMathematicalMatrix matrix)
        {
            if (matrix.GetType() != typeof (Matrix))
                throw new ArgumentException(Resources.InvalidOperationWrongMatrixType);
            return Times((Matrix) matrix);
        }

        /// <summary>
        ///   Times the matrices according to the linear algebra operator *.
        /// </summary>
        /// <param name="number"> </param>
        /// <returns> The result of the times operation. </returns>
        IMathematicalMatrix IMathematicalMatrix.Times(double number)
        {
            return Times(number);
        }

        /// <summary>
        ///   Adds to matrices according to the linear algebra operator +.
        /// </summary>
        /// <param name="matrix"> The result of the addition. </param>
        /// <returns> The result of the plus operation. </returns>
        IMathematicalMatrix IMathematicalMatrix.Plus(IMathematicalMatrix matrix)
        {
            if (matrix.GetType() != typeof (Matrix))
                throw new ArgumentException(Resources.InvalidOperationWrongMatrixType);
            return Plus((Matrix) matrix);
        }

        /// <summary>
        ///   Inverts this instance.
        /// </summary>
        /// <returns> The inverted representation of this instance. </returns>
        IMathematicalMatrix IMathematicalMatrix.Invert()
        {
            return Invert();
        }

        /// <summary>
        ///   Subtracts the matrices according to the linear algebra operator -.
        /// </summary>
        /// <param name="matrix"> The result of the subtraction. </param>
        /// <returns> The result of the minus operation. </returns>
        IMathematicalMatrix IMathematicalMatrix.Minus(IMathematicalMatrix matrix)
        {
            if (matrix.GetType() != typeof (Matrix))
                throw new ArgumentException(Resources.InvalidOperationWrongMatrixType);
            return Minus((Matrix) matrix);
        }

        /// <summary>
        ///   Transposes the matrix.
        /// </summary>
        /// <returns> The transposed representation of this matrix. </returns>
        /// <value> The transposed matrix. </value>
        IMathematicalMatrix IMathematicalMatrix.Transpose()
        {
            return Transpose();
        }
        #endregion

        #region Public Members
        /// <summary>
        ///   Times the matrices according to the linear algebra operator *.
        /// </summary>
        /// <param name="matrix"> The matrix to multiply this matrix with. </param>
        /// <returns> The result of the times operation. </returns>
        public Matrix Times(Matrix matrix)
        {
            // Check the dimensions to make sure the operation is a valid one.
            if (noOfColumns != matrix.noOfRows)
                throw new ArgumentException(Resources.IncompatibleMatricesTimes);
            var ret = new Matrix(noOfRows, matrix.noOfColumns);
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < matrix.noOfColumns; j++)
                {
                    double sum = 0;
                    for (var k = 0; k < noOfColumns; k++)
                        sum += (data[i, k]*matrix.data[k, j]);
                    ret.data[i, j] = sum;
                }
            return ret;
        }

        /// <summary>
        ///   Times the matrices according to the linear algebra operator *.
        /// </summary>
        /// <param name="number"> The number to multiply this matrix with. </param>
        /// <returns> The result of the times operation. </returns>
        public Matrix Times(double number)
        {
            var ret = new Matrix(noOfRows, noOfColumns);
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    ret[i, j] = this[i, j]*number;
            return ret;
        }

        /// <summary>
        ///   Adds to matrices according to the linear algebra operator +.
        /// </summary>
        /// <param name="matrix"> The matrix to add to this matrix. </param>
        /// <returns> The result of the plus operation. </returns>
        public Matrix Plus(Matrix matrix)
        {
            if ((noOfRows != matrix.noOfRows) || (noOfColumns != matrix.noOfColumns))
                throw new ArgumentException(Resources.IncompatibleMatricesSameSize);
            var ret = new Matrix(noOfRows, noOfColumns);
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    ret[i, j] = this[i, j] + matrix[i, j];
            return ret;
        }

        /// <summary>
        ///   Inverts this instance.
        /// </summary>
        /// <returns> An inverted representation of the current matrix. </returns>
        public Matrix Invert()
        {
            return this*-1;
        }

        /// <summary>
        ///   Subtracts the matrices according to the linear algebra operator -.
        /// </summary>
        /// <param name="matrix"> The matrix to subtract from this matrix. </param>
        /// <returns> The result of the subtraction. </returns>
        public Matrix Minus(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(Resources.MatrixIsNull);
            return this + matrix.Invert();
        }

        /// <summary>
        ///   Transposes the matrix.
        /// </summary>
        /// <returns> The transposed matrix. </returns>
        /// <value> </value>
        public Matrix Transpose()
        {
            var ret = new Matrix(noOfColumns, noOfRows);
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    ret[j, i] = this[i, j];
            return ret;
        }

        /// <summary>
        ///   Interchanges (swap) one row with another.
        /// </summary>
        /// <param name="firstRow"> The index of the first row. </param>
        /// <param name="secondRow"> The index of the second row. </param>
        public void InterchangeRows(int firstRow, int secondRow)
        {
            if ((firstRow < 0) || (firstRow > noOfRows - 1))
                throw new ArgumentOutOfRangeException("firstRow");
            if ((secondRow < 0) || (secondRow > noOfRows - 1))
                throw new ArgumentOutOfRangeException("secondRow");
            /// Nothing to do
            if (firstRow == secondRow)
                return;
            double temp;
            for (var i = 0; i < noOfColumns; i++)
            {
                temp = data[firstRow, i];
                data[firstRow, i] = data[secondRow, i];
                data[secondRow, i] = temp;
            }
        }

        /// <summary>
        ///   Interchanges (swap) one column with another.
        /// </summary>
        /// <param name="firstColumn"> The first column. </param>
        /// <param name="secondColumn"> The second column. </param>
        public void InterchangeColumns(int firstColumn, int secondColumn)
        {
            if ((firstColumn < 0) || (firstColumn > noOfColumns - 1))
                throw new ArgumentOutOfRangeException("firstRow");
            if ((secondColumn < 0) || (secondColumn > noOfColumns - 1))
                throw new ArgumentOutOfRangeException("secondRow");
            // Nothing to do
            if (firstColumn == secondColumn)
                return;
            double temp;
            for (var i = 0; i < noOfRows; i++)
            {
                temp = data[i, firstColumn];
                data[i, firstColumn] = data[i, secondColumn];
                data[i, secondColumn] = temp;
            }
        }

        /// <summary>
        ///   Gets the row at the specified index.
        /// </summary>
        /// <param name="rowIndex"> Index of the row. </param>
        /// <returns> An array containing the values of the requested row. </returns>
        public double[] GetRow(int rowIndex)
        {
            if ((rowIndex < 0) || (rowIndex > noOfRows - 1))
                throw new ArgumentOutOfRangeException("rowIndex");
            var ret = new double[noOfColumns];
            for (var i = 0; i < noOfColumns; i++)
                ret[i] = data[rowIndex, i];
            return ret;
        }

        /// <summary>
        ///   Gets the column at the specified index.
        /// </summary>
        /// <param name="columnIndex"> Index of the column. </param>
        /// <returns> An array containing the values of the requested column. </returns>
        public double[] GetColumn(int columnIndex)
        {
            if ((columnIndex < 0) || (columnIndex > noOfColumns - 1))
                throw new ArgumentOutOfRangeException("columnIndex");
            var ret = new double[noOfRows];
            for (var i = 0; i < noOfRows; i++)
                ret[i] = data[columnIndex, i];
            return ret;
        }

        /// <summary>
        ///   Adds the specified number of rows to the matrix.
        /// </summary>
        /// <param name="rowCount"> The number of rows to add. </param>
        public void AddRows(int rowCount)
        {
            if (rowCount <= 0)
                throw new ArgumentOutOfRangeException("columnCount");
            var newRowCount = noOfRows + rowCount;
            // Create a new matrix of the specified size
            var newData = new double[newRowCount,noOfColumns];
            CopyData(newData);
            noOfRows = newRowCount;
            data = newData;
        }

        /// <summary>
        ///   Adds a single row to the matrix.
        /// </summary>
        public void AddRow()
        {
            AddRows(1);
        }

        /// <summary>
        ///   Adds a single row to the matrix, and populates the values
        ///   accordingly.
        /// </summary>
        /// <param name="values"> The values to populate the row with. </param>
        public void AddRowVal(params double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length > noOfColumns)
                throw new ArgumentException(Resources.NumberOfValuesDoNotAgreeWithNumberOfColumns);
            AddRow();
            for (var i = 0; i < values.Length; i++)
                data[noOfRows - 1, i] = values[i];
        }

        
        /// <summary>
        ///   Adds the specified number of rows to the matrix.
        /// </summary>
        /// <param name="columnCount"> The number of rows to add. </param>
        public void AddColumns(int columnCount)
        {
            if (columnCount <= 0)
                throw new ArgumentOutOfRangeException("columnCount");
            var newColumnCount = noOfColumns + columnCount;
            // Create a new matrix of the specified size
            var newData = new double[noOfRows,newColumnCount];
            CopyData(newData);
            noOfColumns = newColumnCount;
            data = newData;
        }

        /// <summary>
        ///   Adds a single row to the matrix.
        /// </summary>
        public void AddColumn()
        {
            AddColumns(1);
        }

        /// <summary>
        ///   Adds a single row to the matrix, and populates the values
        ///   accordingly.
        /// </summary>
        /// <param name="values"> The values to populate the row with. </param>
        public void AddColumnVal(params double[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length > noOfRows)
                throw new ArgumentException(Resources.NumberOfValuesDoNotAgreeWithNumberOfRows);
            AddColumn();
            for (var i = 0; i < values.Length; i++)
                data[i, noOfColumns - 1] = values[i];
        }


        /// <summary>
        ///   Resizes the matrix to the specified size.
        /// </summary>
        /// <param name="newNumberOfRows"> The new number of rows. </param>
        /// <param name="newNumberOfColumns"> The new number of columns. </param>
        public void Resize(int newNumberOfRows, int newNumberOfColumns)
        {
            if ((newNumberOfRows <= 0) || (newNumberOfColumns <= 0))
                throw new ArgumentException(Resources.RowsOrColumnsInvalid);
            var newData = new double[newNumberOfRows,newNumberOfColumns];
            // Find the minimum of the rows and the columns.
            // Case 1 : Target array is smaller than original - don't cross boundaries of target.
            // Case 2 : Original is smaller than target - don't cross boundaries of original.
            var minRows = Math.Min(noOfRows, newNumberOfRows);
            var minColumns = Math.Min(noOfColumns, newNumberOfColumns);
            for (var i = 0; i < minRows; i++)
                for (var j = 0; j < minColumns; j++) newData[i, j] = data[i, j];
            data = newData;
            noOfRows = newNumberOfRows;
            noOfColumns = newNumberOfColumns;
        }

        /// <summary>
        ///   Gets a value indicating whether this matrix instance is symmetric.
        /// </summary>
        /// <value> <c>true</c> if this matrix instance is symmetric; otherwise, <c>false</c> . </value>
        public bool IsSymmetric
        {
            get
            {
                if (noOfRows == noOfColumns)
                {
                    for (var i = 0; i < noOfRows; i++)
                        for (var j = 0; j <= i; j++)
                            if (data[i, j] != data[j, i])
                                return false;
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        ///   Gets the specified sub matrix of the current matrix
        /// </summary>
        /// <param name="rowStart"> The start row. </param>
        /// <param name="columnStart"> The start column. </param>
        /// <param name="rowCount"> The row count. </param>
        /// <param name="columnCount"> The number of columns. </param>
        /// <returns> The submatrix of the current instance. </returns>
        public Matrix GetSubMatrix(int rowStart, int columnStart, int rowCount, int columnCount)
        {
            if ((rowCount <= 0) || (columnCount <= 0))
                throw new ArgumentException(Resources.ColumnAndRowCountBiggerThan0);
            if ((rowStart < 0) || (columnStart < 0))
                throw new ArgumentOutOfRangeException();
            if (((rowStart + rowCount) > Rows) || ((columnStart + columnCount) > Columns))
                throw new ArgumentOutOfRangeException();
            var ret = new Matrix(rowCount, columnCount);
            for (var i = rowStart; i < rowStart + rowCount; i++)
                for (var j = columnStart; j < columnStart + columnCount; j++)
                    ret[i - rowStart, j - columnStart] = this[i, j];
            return ret;
        }

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a copy of this instance. </returns>
        public Matrix Clone()
        {
            var matrix = new Matrix(noOfRows, noOfColumns);
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    matrix[i, j] = this[i, j];
            return matrix;
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        ///   Gets or sets the value at the specified index.
        /// </summary>
        /// <value> </value>
        public double this[int i, int j]
        {
            get { return data[i, j]; }
            set { data[i, j] = value; }
        }

        /// <summary>
        ///   Overload of the operator + as in linear algebra.
        /// </summary>
        /// <param name="m1"> The left hand matrix. </param>
        /// <param name="m2"> The right hand matrix. </param>
        /// <returns> The result of the addition. </returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return m1.Plus(m2);
        }

        /// <summary>
        ///   Overload of the operator - as in linear algebra.
        /// </summary>
        /// <param name="m1"> The left hand matrix. </param>
        /// <param name="m2"> The right hand matrix. </param>
        /// <returns> The result of the subtraction. </returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return m1.Minus(m2);
        }

        /// <summary>
        ///   Overload of the operator * as in linear algebra.
        /// </summary>
        /// <param name="m1"> The left hand matrix. </param>
        /// <param name="m2"> The right hand matrix. </param>
        /// <returns> The result of the multiplication. </returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return m1.Times(m2);
        }

        /// <summary>
        ///   Overload of the operator * as in linear algebra.
        /// </summary>
        /// <param name="number"> The number. </param>
        /// <param name="m2"> The right hand matrix. </param>
        /// <returns> The result of the multiplication. </returns>
        public static Matrix operator *(double number, Matrix m2)
        {
            return m2.Times(number);
        }

        /// <summary>
        ///   Overload of the operator * as in linear algebra.
        /// </summary>
        /// <param name="m1"> The number to be multiplied with. </param>
        /// <param name="number"> The number. </param>
        /// <returns> The result of the multiplicaiton. </returns>
        public static Matrix operator *(Matrix m1, double number)
        {
            return m1.Times(number);
        }

        /// <summary>
        ///   Determines whether the specified matrix is equal to the current one (length, width, values).
        /// </summary>
        /// <param name="m"> The matrix to be compared to. </param>
        /// <returns> <c>true</c> if the specified matrix is equal to the current one; otherwise, <c>false</c> . </returns>
        public bool IsEqual(Matrix m)
        {
            if (m == null)
                return false;
            if (m.Rows != Rows)
                return false;
            if (m.Columns != Columns)
                return false;
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    if (this[i, j] != m[i, j])
                        return false;
            return true;
        }
        #endregion

        #region IVisitableCollection<double> Members
        /// <summary>
        ///   Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value> <c>true</c> if this instance is fixed size; otherwise, <c>false</c> . </value>
        public bool IsFixedSize
        {
            get { return true; }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value> <c>true</c> if this collection is empty; otherwise, <c>false</c> . </value>
        public bool IsEmpty
        {
            get { return false; }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value> <c>true</c> if this collection is full; otherwise, <c>false</c> . </value>
        public bool IsFull
        {
            get { return false; }
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.ICollection"></see> . </returns>
        public int Count
        {
            get { return noOfRows*noOfColumns; }
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(double item)
        {
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    if (this[i, j] == item)
                        return true;
            return false;
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<double> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    visitor.Visit(this[i, j]);
        }

        /// <summary>
        ///   Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see
        ///    cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array"> The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see
        ///    cref="T:System.Collections.Generic.ICollection`1"></see> . The <see cref="T:System.Array"></see> must have zero-based indexing. </param>
        /// <param name="arrayIndex"> The zero-based index in array at which copying begins. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(double[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if ((array.Length - arrayIndex) < Count)
                throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            var counter = arrayIndex;
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                {
                    array.SetValue(this[i, j], counter);
                    counter++;
                }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<double> GetEnumerator()
        {
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    yield return data[i, j];
        }

        /// <summary>
        ///   Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    data[i, j] = 0;
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        void ICollection<double>.Add(double item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. This method also returns false if item is not found in the original <see
        ///    cref="T:System.Collections.Generic.ICollection`1"></see> . </returns>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        bool ICollection<double>.Remove(double item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj"> An object to compare with this instance. </param>
        /// <returns> A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj. </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance.</exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj.GetType() == GetType())
            {
                var m = obj as Matrix;
                return Count.CompareTo(m.Count);
            }
            else
                return GetType().FullName.CompareTo(obj.GetType().FullName);
        }
        #endregion

        #region ICollection<double> Members
        /// <summary>
        ///   Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value> <c>true</c> if this instance is read only; otherwise, <c>false</c> . </value>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        ///   Gets the enumerator.
        /// </summary>
        /// <returns> An enumerator for enumerating through the current colleciton. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IClonable Members
        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a copy of this instance. </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Private Members
        private void CopyData(double[,] newData)
        {
            // Copy all the original data over the new matrix
            for (var i = 0; i < noOfRows; i++)
                for (var j = 0; j < noOfColumns; j++)
                    newData[i, j] = data[i, j];
        }
        #endregion
    }
}