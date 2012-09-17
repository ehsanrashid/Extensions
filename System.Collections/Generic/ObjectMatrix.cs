namespace System.Collections.Generic
{
    using Visitors;

    /// <summary>
    ///   A data structure representing a matrix of objects.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class ObjectMatrix<T> : IMatrix<T>, IVisitableCollection<T>
    {
        /// <summary>
        ///   Gets the number of noOfColumns in this matrix.
        /// </summary>
        /// <value> The nunber of Columns. </value>
        public int Columns { get; private set; }

        /// <summary>
        ///   Gets the number of rows in this matrix.
        /// </summary>
        /// <value> The nunber of Rows. </value>
        public int Rows { get; private set; }

        T[,] mtx;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ObjectMatrix&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="rows"> The rows. </param>
        /// <param name="columns"> The columns. </param>
        public ObjectMatrix(int rows, int columns)
        {
            if ((rows <= 0) || (columns <= 0))
                throw new ArgumentException(Properties.Resources.RowsOrColumnsInvalid);
            Columns = columns;
            Rows = rows;
            mtx = new T[Rows, Columns];
        }

        #region IMatrix<T> Members
        /// <summary>
        ///   Gets a value indicating whether this matrix is square.
        /// </summary>
        /// <value> <c>true</c> if this matrix is square; otherwise, <c>false</c> . </value>
        public bool IsSquare
        {
            get { return Rows == Columns; }
        }

        /// <summary>
        ///   Gets or sets the value at the specified row and column.
        /// </summary>
        /// <value> </value>
        public T this[int i, int j]
        {
            get
            {
                CheckIndexValid(i, j);
                return mtx[i, j];
            }
            set
            {
                CheckIndexValid(i, j);
                mtx[i, j] = value;
            }
        }

        /// <summary>
        ///   Gets the sub matrix.
        /// </summary>
        /// <param name="rowStart"> The row start. </param>
        /// <param name="noOfColumnstart"> The no of columnstart. </param>
        /// <param name="rowCount"> The row count. </param>
        /// <param name="columnCount"> The column count. </param>
        /// <returns> The submatrix of the current matrix. </returns>
        IMatrix<T> IMatrix<T>.GetSubMatrix(int rowStart, int noOfColumnstart, int rowCount, int columnCount)
        {
            return GetSubMatrix(rowStart, noOfColumnstart, rowCount, columnCount);
        }

        /// <summary>
        ///   Interchanges (swap) one row with another.
        /// </summary>
        /// <param name="firstRow"> The index of the first row. </param>
        /// <param name="secondRow"> The index of the second row. </param>
        public void InterchangeRows(int firstRow, int secondRow)
        {
            if ((firstRow < 0) || (firstRow > Rows - 1)) throw new ArgumentOutOfRangeException("firstRow");
            if ((secondRow < 0) || (secondRow > Rows - 1)) throw new ArgumentOutOfRangeException("secondRow");
            // Nothing to do
            if (firstRow == secondRow) return;
            for (var i = 0; i < Columns; i++)
            {
                var temp = mtx[firstRow, i];
                mtx[firstRow, i] = mtx[secondRow, i];
                mtx[secondRow, i] = temp;
            }
        }

        /// <summary>
        ///   Interchanges (swap) one column with another.
        /// </summary>
        /// <param name="firstColumn"> The index of the first column. </param>
        /// <param name="secondColumn"> The index of the second column. </param>
        public void InterchangeColumns(int firstColumn, int secondColumn)
        {
            if ((firstColumn < 0) || (firstColumn > Columns - 1)) throw new ArgumentOutOfRangeException("firstRow");
            if ((secondColumn < 0) || (secondColumn > Columns - 1)) throw new ArgumentOutOfRangeException("secondRow");
            // Nothing to do
            if (firstColumn == secondColumn) return;
            for (var i = 0; i < Rows; i++)
            {
                var temp = mtx[i, firstColumn];
                mtx[i, firstColumn] = mtx[i, secondColumn];
                mtx[i, secondColumn] = temp;
            }
        }

        /// <summary>
        ///   Gets the row at the specified index.
        /// </summary>
        /// <param name="rowIndex"> Index of the row. </param>
        /// <returns> An array containing the values of the requested row. </returns>
        public T[] GetRow(int rowIndex)
        {
            if ((rowIndex < 0) || (rowIndex > Rows - 1)) throw new ArgumentOutOfRangeException("rowIndex");
            var ret = new T[Columns];
            for (var i = 0; i < Columns; i++)
                ret[i] = mtx[rowIndex, i];
            return ret;
        }

        /// <summary>
        ///   Gets the column at the specified index.
        /// </summary>
        /// <param name="columnIndex"> Index of the column. </param>
        /// <returns> An array containing the values of the requested column. </returns>
        public T[] GetColumn(int columnIndex)
        {
            if ((columnIndex < 0) || (columnIndex > Columns - 1))
                throw new ArgumentOutOfRangeException("columnIndex");
            var ret = new T[Rows];
            for (var i = 0; i < Rows; i++)
                ret[i] = mtx[columnIndex, i];
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
            var newRowCount = Rows + rowCount;
            // Create a new matrix of the specified size
            var newData = new T[newRowCount, Columns];
            CopyData(newData);
            Rows = newRowCount;
            mtx = newData;
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
        public void AddRowVal(params T[] values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Length > Columns) throw new ArgumentException(Properties.Resources.NumberOfValuesDoNotAgreeWithNumberOfColumns);
            AddRow();
            for (var i = 0; i < values.Length; i++)
                mtx[Rows - 1, i] = values[i];
        }

        /// <summary>
        ///   Adds the specified number of columns to the matrix.
        /// </summary>
        /// <param name="columnCount"> The number of rows to add. </param>
        public void AddColumns(int columnCount)
        {
            if (columnCount <= 0) throw new ArgumentOutOfRangeException("columnCount");
            var newColumnCount = Columns + columnCount;
            // Create a new matrix of the specified size
            var newData = new T[Rows, newColumnCount];
            CopyData(newData);
            Columns = newColumnCount;
            mtx = newData;
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
        public void AddColumnVal(params T[] values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Length > Rows)
                throw new ArgumentException(Properties.Resources.NumberOfValuesDoNotAgreeWithNumberOfRows);
            AddColumn();
            for (var i = 0; i < values.Length; i++)
                mtx[i, Columns - 1] = values[i];
        }

        /// <summary>
        ///   Resizes the matrix to the specified size.
        /// </summary>
        /// <param name="newRows"> The new number of rows. </param>
        /// <param name="newColumns"> The new number of columns. </param>
        public void Resize(int newRows, int newColumns)
        {
            if ((newRows <= 0) || (newColumns <= 0)) throw new ArgumentException(Properties.Resources.RowsOrColumnsInvalid);
            var newData = new T[newRows, newColumns];
            // Find the minimum of the rows and the columns.
            // Case 1 : Target array is smaller than original - don't cross boundaries of target.
            // Case 2 : Original is smaller than target - don't cross boundaries of original.
            var minRows = Math.Min(Rows, newRows);
            var minColumns = Math.Min(Columns, newColumns);
            for (var i = 0; i < minRows; i++)
                for (var j = 0; j < minColumns; j++)
                    newData[i, j] = mtx[i, j];
            mtx = newData;
            Rows = newRows;
            Columns = newColumns;
        }
        #endregion

        #region IVisitableCollection<T> Members
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
            get { return true; }
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The Object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        public void Clear()
        {
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    mtx[i, j] = default(T);
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The Object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(T item)
        {
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    if (mtx[i, j].Equals(item))
                        return true;
            return false;
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
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if ((array.Length - arrayIndex) < Count)
                throw new ArgumentException(Properties.Resources.NotEnoughSpaceInTargetArray);
            var counter = arrayIndex;
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                {
                    array.SetValue(this[i, j], counter);
                    counter++;
                }
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </returns>
        public int Count
        {
            get { return Rows * Columns; }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value> </value>
        /// <returns> true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false. </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///   Removes the first occurrence of a specific Object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The Object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. This method also returns false if item is not found in the original <see
        ///    cref="T:System.Collections.Generic.ICollection`1"></see> . </returns>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    yield return mtx[i, j];
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="T:System.Collections.IEnumerator"></see> Object that can be used to iterate through the collection. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///   Compares the current instance with another Object of the same type.
        /// </summary>
        /// <param name="obj"> An Object to compare with this instance. </param>
        /// <returns> A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj. </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance.</exception>
        public int CompareTo(Object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (obj.GetType() == GetType())
            {
                var matrix = obj as ObjectMatrix<T>;
                return Count.CompareTo(matrix.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    visitor.Visit(mtx[i, j]);
        }
        #endregion

        /// <summary>
        ///   Gets the sub matrix.
        /// </summary>
        /// <param name="rowStart"> The row start. </param>
        /// <param name="columnStart"> The column start. </param>
        /// <param name="rowCount"> The row count. </param>
        /// <param name="columnCount"> The column count. </param>
        /// <returns> The sub matrix of the current matrix. </returns>
        public ObjectMatrix<T> GetSubMatrix(int rowStart, int columnStart, int rowCount, int columnCount)
        {
            if ((rowCount <= 0) || (columnCount <= 0))
                throw new ArgumentException(Properties.Resources.ColumnAndRowCountBiggerThan0);
            if ((rowStart < 0) || (columnStart < 0))
                throw new ArgumentOutOfRangeException();
            if (((rowStart + rowCount) > Rows) || ((columnStart + columnCount) > Columns))
                throw new ArgumentOutOfRangeException();
            var ret = new ObjectMatrix<T>(rowCount, columnCount);
            for (var i = rowStart; i < rowStart + rowCount; i++)
                for (var j = columnStart; j < columnStart + columnCount; j++)
                    ret[i - rowStart, j - columnStart] = this[i, j];
            return ret;
        }

        /// <summary>
        ///   Checks if the index is in range.
        /// </summary>
        /// <param name="i"> The i. </param>
        /// <param name="j"> The j. </param>
        bool CheckIndexValid(int i, int j)
        {
            return (i >= 0 && i <= Rows) && (j >= 0 && j <= Columns);
        }

        void CopyData(T[,] newData)
        {
            // Copy all the original data over the new matrix
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    newData[i, j] = mtx[i, j];
        }
    }
}