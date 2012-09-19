using System.Collections.Generic;

namespace System
{
    using Collections;

    class ArrayMatrix
    {
        /// <summary>
        /// Contains the rows of the matrix as elements, which
        /// are ArrayLists as well.
        /// </summary>
        readonly ArrayList Values;

        /// <summary>
        /// Number of rows of the matrix.
        /// </summary>
        public int RowCount { get; private set; }

        /// <summary>
        /// Number of columns of the matrix.
        /// </summary>
        public int ColumnCount { get; private set; }

        #region Constructors

        /// <summary>
        /// Inits empty matrix 
        /// </summary>
        public ArrayMatrix()
        {
            Values = new ArrayList();
            RowCount = 0;
            ColumnCount = 0;
        }

        /// <summary>
        /// Creates m by n matrix filled with zeros; same as Zeros(m, n).
        /// </summary>
        /// <param name="m">Number of rows</param>
        /// <param name="n">Number of columns</param>
        public ArrayMatrix(int m, int n)
        {
            RowCount = m;
            ColumnCount = n;

            Values = new ArrayList(m);

            for (var i = 0; i < m; i++)
            {
                Values.Add(new ArrayList(n));

                for (var j = 0; j < n; j++) ((ArrayList) Values[i]).Add(Complex.Zero);
            }
        }

        /// <summary>
        /// Inits square matrix
        /// </summary>
        /// <param name="n"></param>
        public ArrayMatrix(int n)
        {
            RowCount = n;
            ColumnCount = n;

            Values = new ArrayList(n);

            for (var i = 0; i < n; i++)
            {
                Values.Add(new ArrayList(n));

                for (var j = 0; j < n; j++) ((ArrayList) Values[i]).Add(Complex.Zero);
            }
        }

        /// <summary>
        /// Creates one by one matrix containing x
        /// </summary>
        /// <param name="x"></param>
        public ArrayMatrix(Complex x)
        {
            RowCount = 1;
            ColumnCount = 1;

            Values = new ArrayList(1);

            Values.Add(new ArrayList(1));

            ((ArrayList) Values[0]).Add(x);
        }

        /// <summary>
        /// Creates matrix from 2-d Complex array.
        /// </summary>
        /// <param name="values"></param>
        public ArrayMatrix(Complex[,] values)
        {
            if (null == values)
            {
                Values = new ArrayList();
                ColumnCount = 0;
                RowCount = 0;
            }

            RowCount = (int) values.GetLongLength(0);
            ColumnCount = (int) values.GetLongLength(1);

            Values = new ArrayList(RowCount);

            for (var i = 0; i < RowCount; i++)
            {
                Values.Add(new ArrayList(ColumnCount));

                for (var j = 0; j < ColumnCount; j++) ((ArrayList) Values[i]).Add(values[i, j]);
            }
        }

        /// <summary>
        /// Creates column vector from Complex array.
        /// </summary>
        /// <param name="values"></param>
        public ArrayMatrix(IList<Complex> values)
        {
            if (values == null)
            {
                Values = new ArrayList();
                ColumnCount = 0;
                RowCount = 0;
            }

            RowCount = values.Count;
            ColumnCount = 1;

            Values = new ArrayList(RowCount);

            for (var i = 0; i < RowCount; i++)
            {
                Values.Add(new ArrayList(1));

                ((ArrayList) Values[i]).Add(values[i]);
            }
        }

        /// <summary>
        /// Creates one by one matrix containing x
        /// </summary>
        /// <param name="x"></param>
        public ArrayMatrix(double x)
        {
            RowCount = 1;
            ColumnCount = 1;

            Values = new ArrayList(1);

            Values.Add(new ArrayList(1));

            ((ArrayList) Values[0]).Add(new Complex(x));
        }

        /// <summary>
        /// Creates matrix from 2-d double array.
        /// </summary>
        /// <param name="values"></param>
        public ArrayMatrix(double[,] values)
        {
            if (values == null)
            {
                Values = new ArrayList();
                ColumnCount = 0;
                RowCount = 0;
            }

            RowCount = (int) values.GetLongLength(0);
            ColumnCount = (int) values.GetLongLength(1);

            Values = new ArrayList(RowCount);

            for (var i = 0; i < RowCount; i++)
            {
                Values.Add(new ArrayList(ColumnCount));

                for (var j = 0; j < ColumnCount; j++) ((ArrayList) Values[i]).Add(new Complex(values[i, j]));
            }
        }

        /// <summary>
        /// Creates column vector from double array.
        /// </summary>
        /// <param name="values"></param>
        public ArrayMatrix(double[] values)
        {
            if (values == null)
            {
                Values = new ArrayList();
                ColumnCount = 0;
                RowCount = 0;
            }

            RowCount = values.Length;
            ColumnCount = 1;

            Values = new ArrayList(RowCount);

            for (var i = 0; i < RowCount; i++)
            {
                Values.Add(new ArrayList(1));

                ((ArrayList) Values[i]).Add(new Complex(values[i]));
            }
        }

        /// <summary>
        /// Creates real matrix from String, e.g. "1,0;0,1" gives the 2 by 2 identity matrix.
        /// Not fast, but easy to use, if matrices are to be entered by hand or read from text files.
        /// </summary>
        /// <param name="matrix">Matrix coded as String. Lines are separated by a semicolon, column elements by a comma.</param>
        public ArrayMatrix(String matrix_string)
        {
            // remove spaces
            matrix_string = matrix_string.Replace(" ", "");

            // split String into rows, use ';' as separator
            var rows = matrix_string.Split(new char[] {';'});

            // init Values, RowCount, ColumnCount
            RowCount = rows.Length;
            Values = new ArrayList(RowCount);
            ColumnCount = 0;

            for (var i = 0; i < RowCount; i++) Values.Add(new ArrayList());

            for (var i = 1; i <= RowCount; i++)
            {
                var curcol = rows[i - 1].Split(new char[] {','});

                for (var j = 1; j <= curcol.Length; j++) this[i, j] = new Complex(Convert.ToDouble(curcol[j - 1]));
            }
        }

        #endregion

        #region Static func

        /// <summary>
        /// Retrieves the j-th canoncical basis vector of the IR^n.
        /// </summary>
        /// <param name="n">Dimension of the basis.</param>
        /// <param name="j">Index of canonical basis vector to be retrieved.</param>
        /// <returns></returns>
        public static ArrayMatrix E(int n, int j)
        {
            var e = Zeros(n, 1);
            e[j] = Complex.One;

            return e;
        }

        /// <summary>
        /// Returns 1 if i = j, and 0 else.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static Complex KroneckerDelta(int i, int j)
        {
            return new Complex(Math.Min(Math.Abs(i - j), 1));
        }

        /// <summary>
        /// Creates m by n chessboard matrix with interchangíng ones and zeros.
        /// 
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of columns.</param>
        /// <param name="even">Indicates, if matrix entry (1,1) equals zero.</param>
        /// <returns></returns>
        public static ArrayMatrix ChessboardMatrix(int m, int n, bool even)
        {
            var M = new ArrayMatrix(m, n);

            if (even) for (var i = 1; i <= m; i++) for (var j = 1; j <= n; j++) M[i, j] = KroneckerDelta((i + j)%2, 0);
            else for (var i = 1; i <= m; i++) for (var j = 1; j <= n; j++) M[i, j] = KroneckerDelta((i + j)%2, 1);

            return M;
        }

        /// <summary>
        /// Creates m by n chessboard matrix with interchangíng ones and zeros.
        /// 
        /// </summary>        
        /// <param name="n">Number of columns.</param>
        /// <param name="even">Indicates, if matrix entry (1,1) equals zero.</param>
        /// <returns></returns>
        public static ArrayMatrix ChessboardMatrix(int n, bool even)
        {
            var M = new ArrayMatrix(n);

            if (even) for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) M[i, j] = KroneckerDelta((i + j)%2, 0);
            else for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) M[i, j] = KroneckerDelta((i + j)%2, 1);

            return M;
        }

        /// <summary>
        /// Creates m by n matrix filled with zeros.
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of columns.</param>
        /// <returns>m by n matrix filled with zeros.</returns>
        public static ArrayMatrix Zeros(int m, int n)
        {
            return new ArrayMatrix(m, n);
        }

        /// <summary>
        /// Creates n by n matrix filled with zeros.
        /// </summary>       
        /// <param name="n">Number of rows and columns, resp.</param>
        /// <returns>n by n matrix filled with zeros.</returns>
        public static ArrayMatrix Zeros(int n)
        {
            return new ArrayMatrix(n);
        }

        /// <summary>
        /// Creates m by n matrix filled with ones.
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of columns.</param>
        /// <returns>m by n matrix filled with ones.</returns>        
        public static ArrayMatrix Ones(int m, int n)
        {
            var M = new ArrayMatrix(m, n);

            for (var i = 0; i < m; i++) for (var j = 0; j < n; j++) ((ArrayList) M.Values[i])[j] = Complex.One;

            return M;
        }

        /// <summary>
        /// Creates n by n matrix filled with ones.
        /// </summary>        
        /// <param name="n">Number of columns.</param>
        /// <returns>n by n matrix filled with ones.</returns>        
        public static ArrayMatrix Ones(int n)
        {
            var M = new ArrayMatrix(n);

            for (var i = 0; i < n; i++) for (var j = 0; j < n; j++) ((ArrayList) M.Values[i])[j] = Complex.One;

            return M;
        }

        /// <summary>
        /// Creates n by n identity matrix.
        /// </summary>
        /// <param name="n">Number of rows and columns respectively.</param>
        /// <returns>n by n identity matrix.</returns>
        public static ArrayMatrix Identity(int n)
        {
            return Diag(Ones(n, 1));
        }

        /// <summary>
        /// Creates teh n by n identity matrix.
        /// </summary>
        /// <param name="n">Number of rows and columns, resp.</param>
        /// <returns></returns>
        public static ArrayMatrix Eye(int n)
        {
            return Identity(n);
        }

        /// <summary>
        /// Vertically concats matrices A and B, which do not have to be of the same height. 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>Matrix [A|B]</returns>
        public static ArrayMatrix VerticalConcat(ArrayMatrix A, ArrayMatrix B)
        {
            var C = A.Column(1);

            for (var j = 2; j <= A.ColumnCount; j++) C.InsertColumn(A.Column(j), j);

            for (var j = 1; j <= B.ColumnCount; j++) C.InsertColumn(B.Column(j), C.ColumnCount + 1);

            return C;
        }

        public static ArrayMatrix VerticalConcat(ArrayMatrix[] A)
        {
            if (A == null) throw new ArgumentNullException();
            else if (A.Length == 1) return A[0];
            else
            {
                var C = VerticalConcat(A[0], A[1]);

                for (var i = 2; i < A.Length; i++) C = VerticalConcat(C, A[i]);

                return C;
            }
        }

        public static ArrayMatrix HorizontalConcat(ArrayMatrix A, ArrayMatrix B)
        {
            var C = A.Row(1);

            for (var i = 2; i <= A.RowCount; i++) C.InsertRow(A.Row(i), i);

            for (var i = 1; i <= B.RowCount; i++) C.InsertRow(B.Row(i), C.RowCount + 1);

            return C;
        }

        public static ArrayMatrix HorizontalConcat(ArrayMatrix[] A)
        {
            if (A == null) throw new ArgumentNullException();
            else if (A.Length == 1) return A[0];
            else
            {
                var C = HorizontalConcat(A[0], A[1]);

                for (var i = 2; i < A.Length; i++) C = HorizontalConcat(C, A[i]);

                return C;
            }
        }

        /// <summary>
        /// Generates diagonal matrix
        /// </summary>
        /// <param name="diag_vector">column vector containing the diag elements</param>
        /// <returns></returns>
        public static ArrayMatrix Diag(ArrayMatrix diag_vector)
        {
            var dim = diag_vector.VectorLength();

            if (dim == 0) throw new ArgumentException("diag_vector must be 1xN or Nx1");

            var M = new ArrayMatrix(dim, dim);

            for (var i = 1; i <= dim; i++) M[i, i] = diag_vector[i];

            return M;
        }

        /// <summary>
        /// Generates diagonal matrix
        /// </summary>
        /// <param name="diag_vector">column vector containing the diag elements</param>
        /// <returns></returns>
        public static ArrayMatrix Diag(ArrayMatrix diag_vector, int offset)
        {
            var dim = diag_vector.VectorLength();

            if (dim == 0) throw new ArgumentException("diag_vector must be 1xN or Nx1.");

            //if (Math.Abs(offset) >= dim)
            //    throw new ArgumentException("Absolute value of offset must be less than length of diag_vector.");

            var M = new ArrayMatrix(dim + Math.Abs(offset), dim + Math.Abs(offset));
            dim = M.RowCount;

            if (offset >= 0) for (var i = 1; i <= dim - offset; i++) M[i, i + offset] = diag_vector[i];
            else for (var i = 1; i <= dim + offset; i++) M[i - offset, i] = diag_vector[i];

            return M;
        }

        /// <summary>
        /// Generates tri-diagonal square matrix with constant values on main
        /// and secondary diagonals.
        /// </summary>
        /// <param name="l">Value of lower secondary diagonal.</param>
        /// <param name="d">Value of main diagonal.</param>
        /// <param name="u">Value of upper secondary diagonal.</param>
        /// <param name="n">Dimension of the output matrix.</param>
        /// <returns>nxn tri-diagonal matrix.</returns>
        public static ArrayMatrix TriDiag(Complex l, Complex d, Complex u, int n)
        {
            if (n <= 1) throw new ArgumentException("Matrix dimension must greater than one.");

            return Diag(l*Ones(n - 1, 1), -1) + Diag(d*Ones(n, 1)) + Diag(u*Ones(n - 1, 1), 1);
        }

        /// <summary>
        /// Generates tri-diagonal square matrix with overloaded vectors
        /// as main and secondary diagonals. The dimension of the output
        /// matrix is determined by the length of d.
        /// </summary>
        /// <param name="l">Lower secondary diagonal vector.</param>
        /// <param name="d">Main diagonal vector.</param>
        /// <param name="u">Upper secondary diagonal vector.</param>
        /// <returns></returns>
        public static ArrayMatrix TriDiag(ArrayMatrix l, ArrayMatrix d, ArrayMatrix u)
        {
            var sizeL = l.VectorLength();
            var sizeD = d.VectorLength();
            var sizeU = u.VectorLength();

            if (sizeL*sizeD*sizeU == 0) throw new ArgumentException("At least one of the paramter matrices is not a vector.");

            if (sizeL != sizeU) throw new ArgumentException("Lower and upper secondary diagonal must have the same length.");

            if (sizeL + 1 != sizeD) throw new ArgumentException("Main diagonal must have exactly one element more than the secondary diagonals.");

            return Diag(l, -1) + Diag(d) + Diag(u, 1);
        }

        /// <summary>
        /// Implements the dot product of two vectors.
        /// </summary>
        /// <param name="v">Row or column vector.</param>
        /// <param name="w">Row or column vector.</param>
        /// <returns>Dot product.</returns>
        public static Complex Dot(ArrayMatrix v, ArrayMatrix w)
        {
            var m = v.VectorLength();
            var n = w.VectorLength();

            if (m == 0 || n == 0) throw new ArgumentException("Arguments need to be vectors.");
            else if (m != n) throw new ArgumentException("Vectors must be of the same length.");

            var buf = Complex.Zero;

            for (var i = 1; i <= m; i++) buf += v[i]*w[i];

            return buf;
        }

        /// <summary>
        /// Calcs the n-th Fibonacci-number in O(n)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Complex Fib(int n)
        {
            var M = Ones(2, 2);
            M[2, 2] = Complex.Zero;

            return (M ^ (n - 1))[1, 1];
        }

        /// <summary>
        /// Creates n by n matrix filled with random values in [0,1];
        /// all entries on the main diagonal are zero.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ArrayMatrix RandomGraph(int n)
        {
            var buf = Random(n, n);

            buf -= Diag(buf.DiagVector());

            return buf;
        }

        /// <summary>
        /// Creates n by n matrix filled with random values in [0,1];
        /// all entries on the main diagonal are zero.
        /// A specified random percentage of edges has weight positive infinity.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="p">Defines probability for an edge being less than +infty. Should be in [0,1],
        /// p = 1 gives complete directed graph; p = 0 gives no edges.</param>
        /// <returns></returns>
        public static ArrayMatrix RandomGraph(int n, double p)
        {
            var buf = new ArrayMatrix(n);

            var r = new Random();

            for (var i = 1; i <= n; i++)
                for (var j = 1; j <= n; j++)
                    if (i == j) buf[i, j] = Complex.Zero;
                    else if (r.NextDouble() < p) buf[i, j] = new Complex(r.NextDouble());
                    else buf[i, j] = new Complex(double.PositiveInfinity);

            return buf;
        }

        /// <summary>
        /// Creates m by n matrix filled with random values in [0,1].
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ArrayMatrix Random(int m, int n)
        {
            var M = new ArrayMatrix(m, n);
            var r = new Random();

            for (var i = 1; i <= m; i++) for (var j = 1; j <= n; j++) M[i, j] = new Complex(r.NextDouble());

            return M;
        }

        /// <summary>
        /// Creates n by n matrix filled with random values in [0,1].
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ArrayMatrix Random(int n)
        {
            var M = new ArrayMatrix(n);
            var r = new Random();

            for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) M[i, j] = new Complex(r.NextDouble());

            return M;
        }

        /// <summary>
        /// Creates n by n matrix filled with random values in {lo,...,hi-1}.
        /// </summary>
        ///<param name="lo">Inclusive lower bound.</param>
        /// <param name="hi">Exclusive upper bound</param>
        /// <param name="n">Number of rows and columns each.</param>
        /// <returns></returns>
        public static ArrayMatrix Random(int n, int lo, int hi)
        {
            var M = new ArrayMatrix(n);
            var r = new Random();

            for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) M[i, j] = new Complex((double) r.Next(lo, hi));

            return M;
        }

        /// <summary>
        /// Creates m by n random zero one matrix with probability p for a one.
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of columns.</param>
        /// <param name="p">Probability fro an entry to be one, expecting a value in [0,1].</param>
        /// <returns></returns>
        public static ArrayMatrix RandomZeroOne(int m, int n, double p)
        {
            var M = new ArrayMatrix(m, n);
            var r = new Random();

            for (var i = 1; i <= m; i++) for (var j = 1; j <= n; j++) if (r.NextDouble() <= p) M[i, j] = Complex.One;

            return M;
        }

        /// <summary>
        /// Creates n by n random zero one matrix with probability p for a one.
        /// </summary>        
        /// <param name="n">Number of rows and columns, resp.</param>
        /// <param name="p">Probability fro an entry to be one, expecting a value in [0,1].</param>
        /// <returns></returns>
        public static ArrayMatrix RandomZeroOne(int n, double p)
        {
            var M = new ArrayMatrix(n, n);
            var r = new Random();

            for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) if (r.NextDouble() <= p) M[i, j] = Complex.One;

            return M;
        }

        /// <summary>
        /// Creates m by n matrix filled with random values in {lo,...,hi-1}.
        /// </summary>
        ///<param name="lo">Inclusive lower bound.</param>
        /// <param name="hi">Exclusive upper bound</param>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of columns.</param>
        /// <returns></returns>
        public static ArrayMatrix Random(int m, int n, int lo, int hi)
        {
            var M = new ArrayMatrix(m, n);
            var r = new Random();

            for (var i = 1; i <= m; i++) for (var j = 1; j <= n; j++) M[i, j] = new Complex((double) r.Next(lo, hi));

            return M;
        }

        public static ArrayMatrix Vandermonde(Complex[] x)
        {
            if (x == null || x.Length < 1) throw new ArgumentNullException();

            var n = x.Length - 1;

            var V = new ArrayMatrix(n + 1);

            for (var i = 0; i <= n; i++) for (var p = 0; p <= n; p++) V[i + 1, p + 1] = Complex.Pow(x[i], p);

            return V;
        }

        /// <summary>
        /// Computes all shortest distance between any vertices in a given graph.
        /// </summary>
        /// <param name="adjacence_matrix">Square adjacence matrix. The main diagonal
        /// is expected to consist of zeros, any non-existing edges should be marked
        /// positive infinity.</param>
        /// <returns>Two matrices D and P, where D[u,v] holds the distance of the shortest
        /// path between u and v, and P[u,v] holds the shortcut vertex on the way from
        /// u to v.</returns>
        public static ArrayMatrix[] Floyd(ArrayMatrix adjacence_matrix)
        {
            if (!adjacence_matrix.IsSquare()) throw new ArgumentException("Expected square matrix.");
            else if (!adjacence_matrix.IsReal()) throw new ArgumentException("Adjacence matrices are expected to be real.");

            var n = adjacence_matrix.RowCount;

            var D = adjacence_matrix.Clone(); // distance matrix
            var P = new ArrayMatrix(n);

            double buf;

            for (var k = 1; k <= n; k++)
                for (var i = 1; i <= n; i++)
                    for (var j = 1; j <= n; j++)
                    {
                        buf = D[i, k].Real + D[k, j].Real;
                        if (buf < D[i, j].Real)
                        {
                            D[i, j].Real = buf;
                            P[i, j].Real = k;
                        }
                    }

            return new ArrayMatrix[] {D, P};
        }

        /// <summary>
        /// Returns the shortest path between two given vertices i and j as
        /// int array.
        /// </summary>
        /// <param name="P">Path matrix as returned from Floyd().</param>
        /// <param name="i">One-based index of start vertex.</param>
        /// <param name="j">One-based index of end vertex.</param>
        /// <returns></returns>
        public static ArrayList FloydPath(ArrayMatrix P, int i, int j)
        {
            if (!P.IsSquare()) throw new ArgumentException("Path matrix must be square.");
            else if (!P.IsReal()) throw new ArgumentException("Adjacence matrices are expected to be real.");

            var path = new ArrayList();
            path.Add(i);

            //int borderliner = 0;
            //int n = P.Size()[0] + 1; // shortest path cannot have more than n vertices! 

            while (P[i, j] != 0)
            {
                i = Convert.ToInt32(P[i, j]);
                path.Add(i);

                //borderliner++;

                //if (borderliner == n)
                //    throw new FormatException("P was not a Floyd path matrix.");
            }

            path.Add(j);

            return path;
        }

        /// <summary>
        /// Performs depth-first search for a graph given by its adjacence matrix.
        /// </summary>
        /// <param name="adjacence_matrix">A[i,j] = 0 or +infty, if there is no edge from i to j; any non-zero value otherwise.</param>
        /// <param name="root">The vertex to begin the search.</param>
        /// <returns>Adjacence matrix of the computed spanning tree.</returns>
        public static ArrayMatrix DFS(ArrayMatrix adjacence_matrix, int root)
        {
            if (!adjacence_matrix.IsSquare()) throw new ArgumentException("Adjacence matrices are expected to be square.");
            else if (!adjacence_matrix.IsReal()) throw new ArgumentException("Adjacence matrices are expected to be real.");

            var n = adjacence_matrix.RowCount;

            if (root < 1 || root > n) throw new ArgumentException("Root must be a vertex of the graph, e.i. in {1, ..., n}.");

            var spanTree = new ArrayMatrix(n);

            var marked = new bool[n + 1];

            var todo = new Stack();
            todo.Push(root);
            marked[root] = true;

            // adajacence lists for each vertex
            var A = new ArrayList[n + 1];

            for (var i = 1; i <= n; i++)
            {
                A[i] = new ArrayList();

                for (var j = 1; j <= n; j++) if (adjacence_matrix[i, j].Real != 0 && adjacence_matrix[i, j].Imag != double.PositiveInfinity) A[i].Add(j);
            }

            int v, w;

            while (todo.Count > 0)
            {
                v = (int) todo.Peek();

                if (A[v].Count > 0)
                {
                    w = (int) A[v][0];

                    if (!marked[w])
                    {
                        marked[w] = true; // mark w
                        spanTree[v, w].Real = 1; // mark vw
                        todo.Push(w); // one more to search
                    }

                    A[v].RemoveAt(0);
                }
                else todo.Pop();
            }

            return spanTree;
        }

        /// <summary>
        /// Performs broad-first search for a graph given by its adjacence matrix.
        /// </summary>
        /// <param name="adjacence_matrix">A[i,j] = 0 or +infty, if there is no edge from i to j; any non-zero value otherwise.</param>
        /// <param name="root">The vertex to begin the search.</param>
        /// <returns>Adjacence matrix of the computed spanning tree.</returns>
        public static ArrayMatrix BFS(ArrayMatrix adjacence_matrix, int root)
        {
            if (!adjacence_matrix.IsSquare()) throw new ArgumentException("Adjacence matrices are expected to be square.");
            else if (!adjacence_matrix.IsReal()) throw new ArgumentException("Adjacence matrices are expected to be real.");

            var n = adjacence_matrix.RowCount;

            if (root < 1 || root > n) throw new ArgumentException("Root must be a vertex of the graph, e.i. in {1, ..., n}.");

            var spanTree = new ArrayMatrix(n);

            var marked = new bool[n + 1];

            var todo = new Queue();
            todo.Enqueue(root);
            marked[root] = true;

            // adajacence lists for each vertex
            var A = new ArrayList[n + 1];

            for (var i = 1; i <= n; i++)
            {
                A[i] = new ArrayList();

                for (var j = 1; j <= n; j++) if (adjacence_matrix[i, j].Real != 0 && adjacence_matrix[i, j].Real != double.PositiveInfinity) A[i].Add(j);
            }

            int v, w;

            while (todo.Count > 0)
            {
                v = (int) todo.Peek();

                if (A[v].Count > 0)
                {
                    w = (int) A[v][0];

                    if (!marked[w])
                    {
                        marked[w] = true; // mark w
                        spanTree[v, w].Real = 1; // mark vw
                        todo.Enqueue(w); // one more to search
                    }

                    A[v].RemoveAt(0);
                }
                else todo.Dequeue();
            }

            return spanTree;
        }

        /// <summary>
        /// Creates a random matrix filled with zeros and ones.
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of columns.</param>
        /// <param name="p">Probability of each entry being 1.</param>
        /// <returns></returns>
        public static ArrayMatrix ZeroOneRandom(int m, int n, double p)
        {
            var r = new Random();

            var buf = Zeros(m, n);

            for (var i = 1; i <= m; i++) for (var j = 1; j <= n; j++) if (r.NextDouble() <= p) buf[i, j] = Complex.One;

            return buf;
        }

        /// <summary>
        /// Creates a random matrix filled with zeros and ones.
        /// </summary>        
        /// <param name="n">Number of rows and columns.</param>
        /// <param name="p">Probability of each entry being 1.</param>
        /// <returns></returns>
        public static ArrayMatrix ZeroOneRandom(int n, double p)
        {
            var r = new Random();

            var buf = Zeros(n);

            for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) if (r.NextDouble() <= p) buf[i, j] = Complex.One;

            return buf;
        }

        /// <summary>
        /// Computes the Householder vector.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static ArrayMatrix[] HouseholderVector(ArrayMatrix x)
        {
            //throw new NotImplementedException("Supposingly buggy!");

            //if (!x.IsReal())
            //    throw new ArgumentException("Cannot compute housholder vector of non-real vector.");

            var n = x.VectorLength();

            if (n == 0) throw new InvalidOperationException("Expected vector as argument.");

            var y = x/x.Norm();
            var buf = y.Extract(2, n, 1, 1);
            var s = Dot(buf, buf);

            var v = Zeros(n, 1);
            v[1] = Complex.One;

            v.Insert(2, 1, buf);

            double beta = 0;

            if (s != 0)
            {
                var mu = Complex.Sqrt(y[1]*y[1] + s);
                if (y[1].Real <= 0) v[1] = y[1] - mu;
                else v[1] = -s/(y[1] + mu);

                beta = 2*v[1].Real*v[1].Real/(s.Real + v[1].Real*v[1].Real);
                v = v/v[1];
            }

            return new ArrayMatrix[] {v, new ArrayMatrix(beta)};
        }

        /// <summary>
        /// Constructs block matrix [A, B; C, D].
        /// </summary>
        /// <param name="A">Upper left sub matrix.</param>
        /// <param name="B">Upper right sub matrix.</param>
        /// <param name="C">Lower left sub matrix.</param>
        /// <param name="D">Lower right sub matrix.</param>
        /// <returns></returns>
        public static ArrayMatrix BlockMatrix(ArrayMatrix A, ArrayMatrix B, ArrayMatrix C, ArrayMatrix D)
        {
            if (A.RowCount != B.RowCount || C.RowCount != D.RowCount
                || A.ColumnCount != C.ColumnCount || B.ColumnCount != D.ColumnCount) throw new ArgumentException("Matrix dimensions must agree.");

            var R = new ArrayMatrix(A.RowCount + C.RowCount, A.ColumnCount + B.ColumnCount);

            for (var i = 1; i <= R.RowCount; i++)
                for (var j = 1; j <= R.ColumnCount; j++)
                    if (i <= A.RowCount)
                        if (j <= A.ColumnCount) R[i, j] = A[i, j];
                        else R[i, j] = B[i, j - A.ColumnCount];
                    else if (j <= C.ColumnCount) R[i, j] = C[i - A.RowCount, j];
                    else R[i, j] = D[i - A.RowCount, j - C.ColumnCount];

            return R;
        }

        /// <summary>
        /// For this matrix A, this method solves Ax = b via LU factorization with
        /// column pivoting.
        /// </summary>
        /// <param name="b">Vector of appropriate length.</param>
        /// <remarks>Approximately n^3/3 + 2n^2 dot operations ~> O(n^3)</remarks>
        public static ArrayMatrix Solve(ArrayMatrix A, ArrayMatrix b)
        {
            var A2 = A.Clone();
            var b2 = b.Clone();

            if (!A2.IsSquare()) throw new InvalidOperationException("Cannot uniquely solve non-square equation system.");

            var n = A2.RowCount;

            var P = A2.LUSafe();

            // We know: PA = LU => [ Ax = b <=> P'LUx = b <=> L(Ux) = (Pb)] since P is orthogonal
            // set y := Ux, solve Ly = Pb by forward insertion
            // and Ux = y by backward insertion

            b2 = P*b2;

            // this solves Ly = Pb
            (A2.ExtractLowerTrapeze() - Diag(A2.DiagVector()) + Identity(n)).ForwardInsertion(b2);

            // this solves Ux = y
            (A2.ExtractUpperTrapeze()).BackwardInsertion(b2);

            return b2;
        }

        #endregion

        #region Dynamic funcs

        #region Matrix manipulations, extractions and decompositions

        /// <summary>
        /// Returns the matrix of the real parts of the entries of this matrix.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix Re()
        {
            var M = new ArrayMatrix(RowCount, ColumnCount);

            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= ColumnCount; j++) M[i, j] = new Complex(this[i, j].Real);

            return M;
        }

        /// <summary>
        /// Returns the matrix of the imaginary parts of the entries of this matrix.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix Im()
        {
            var M = new ArrayMatrix(RowCount, ColumnCount);

            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= ColumnCount; j++) M[i, j] = new Complex(this[i, j].Imag);

            return M;
        }

        /// <summary>
        /// Performs Hessenberg-Householder reduction, where {H, Q}
        /// is returned, with H Hessenbergian, Q orthogonal and H = Q'AQ.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix[] HessenbergHouseholder()
        {
            //throw new NotImplementedException("Still buggy!");

            if (!IsSquare())
                throw new InvalidOperationException(
                    "Cannot perform Hessenberg Householder decomposition of non-square matrix.");

            var n = RowCount;
            var Q = Identity(n);
            var H = Clone();
            ArrayMatrix I, N, R, P;
            var vbeta = new ArrayMatrix[2];
            int m;

            // don't try to understand from the code alone.
            // this is pure magic to me - mathematics, reborn as code.
            for (var k = 1; k <= n - 2; k++)
            {
                vbeta = HouseholderVector(H.Extract(k + 1, n, k, k));
                I = Identity(k);
                N = Zeros(k, n - k);

                m = vbeta[0].VectorLength();
                R = Identity(m) - vbeta[1][1, 1]*vbeta[0]*vbeta[0].Transpose();

                H.Insert(k + 1, k, R*H.Extract(k + 1, n, k, n));
                H.Insert(1, k + 1, H.Extract(1, n, k + 1, n)*R);

                P = BlockMatrix(I, N, N.Transpose(), R);

                Q = Q*P;
            }

            return new ArrayMatrix[] {H, Q};
        }

        /// <summary>
        /// Extract sub matrix.
        /// </summary>
        /// <param name="i1">Start row.</param>
        /// <param name="i2">End row.</param>
        /// <param name="j1">Start column.</param>
        /// <param name="j2">End column.</param>
        /// <returns></returns>
        public ArrayMatrix Extract(int i1, int i2, int j1, int j2)
        {
            if (i2 < i1 || j2 < j1 || i1 <= 0 || j2 <= 0 || i2 > RowCount || j2 > ColumnCount) throw new ArgumentException("Index exceeds matrix dimension.");

            var B = new ArrayMatrix(i2 - i1 + 1, j2 - j1 + 1);

            for (var i = i1; i <= i2; i++) for (var j = j1; j <= j2; j++) B[i - i1 + 1, j - j1 + 1] = this[i, j];

            return B;
        }

        /// <summary>
        /// Extracts lower trapeze matrix of this matrix.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix ExtractLowerTrapeze()
        {
            var buf = new ArrayMatrix(RowCount, ColumnCount);

            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= i; j++) buf[i, j] = this[i, j];

            return buf;
        }

        /// <summary>
        /// Extracts upper trapeze matrix of this matrix.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix ExtractUpperTrapeze()
        {
            var buf = new ArrayMatrix(RowCount, ColumnCount);

            for (var i = 1; i <= RowCount; i++) for (var j = i; j <= ColumnCount; j++) buf[i, j] = this[i, j];

            return buf;
        }

        /// <summary>
        /// Splits matrix into its column vectors.
        /// </summary>
        /// <returns>Array of column vectors.</returns>
        public ArrayMatrix[] ColumnVectorize()
        {
            var buf = new ArrayMatrix[ColumnCount];

            for (var j = 1; j <= buf.Length; j++) buf[j] = Column(j);

            return buf;
        }

        /// <summary>
        /// Splits matrix into its row vectors.
        /// </summary>
        /// <returns>Array of row vectors.</returns>
        public ArrayMatrix[] RowVectorize()
        {
            var buf = new ArrayMatrix[RowCount];

            for (var i = 1; i <= buf.Length; i++) buf[i] = Row(i);

            return buf;
        }

        /// <summary>
        /// Flips matrix vertically.
        /// </summary>
        public void VerticalFlip()
        {
            Values.Reverse();
        }

        /// <summary>
        /// Flips matrix horizontally.
        /// </summary>
        public void HorizontalFlip()
        {
            for (var i = 0; i < RowCount; i++) ((ArrayList) Values[i]).Reverse();
        }

        /// <summary>
        /// Swaps columns at specified indices. The latter do not have to be ordered.
        /// When equal, nothing is done.
        /// </summary>
        /// <param name="j1">One-based index of first col.</param>
        /// <param name="j2">One-based index of second col.</param>       
        public void SwapColumns(int j1, int j2)
        {
            if (j1 <= 0 || j1 > ColumnCount || j2 <= 0 || j2 > ColumnCount) throw new ArgumentException("Indices must be positive and <= number of cols.");

            if (j1 == j2) return;

            // ArrayList indices are zero-based
            j1--;
            j2--;
            object buf;

            for (var i = 0; i < RowCount; i++)
            {
                buf = ((ArrayList) Values[i])[j1];
                ((ArrayList) Values[i])[j1] = ((ArrayList) Values[i])[j2];
                ((ArrayList) Values[i])[j2] = buf;
            }
        }

        /// <summary>
        /// Swaps rows at specified indices. The latter do not have to be ordered.
        /// When equal, nothing is done.
        /// </summary>
        /// <param name="i1">One-based index of first row.</param>
        /// <param name="i2">One-based index of second row.</param>        
        public void SwapRows(int i1, int i2)
        {
            if (i1 <= 0 || i1 > RowCount || i2 <= 0 || i2 > RowCount) throw new ArgumentException("Indices must be positive and <= number of rows.");

            if (i1 == i2) return;

            var buf = (ArrayList) Values[--i1];
            Values[i1] = Values[--i2];
            Values[i2] = buf;
        }

        /// <summary>
        /// Deletes row at specifies index.
        /// </summary>
        /// <param name="i">One-based index at which to delete.</param>
        public void DeleteRow(int i)
        {
            if (i <= 0 || i > RowCount) throw new ArgumentException("Index must be positive and <= number of rows.");

            Values.RemoveAt(i - 1);
            RowCount--;
        }

        /// <summary>
        /// Deletes column at specifies index.
        /// </summary>
        /// <param name="j">One-based index at which to delete.</param>
        public void DeleteColumn(int j)
        {
            if (j <= 0 || j > ColumnCount) throw new ArgumentException("Index must be positive and <= number of cols.");

            for (var i = 0; i < RowCount; i++) ((ArrayList) Values[i]).RemoveAt(j - 1);

            ColumnCount--;
        }

        /// <summary>
        /// Retrieves row vector at specfifed index and deletes it from matrix.
        /// </summary>
        /// <param name="i">One-based index at which to extract.</param>
        /// <returns>Row vector.</returns>
        public ArrayMatrix ExtractRow(int i)
        {
            var buf = Row(i);
            DeleteRow(i);

            return buf;
        }

        /// <summary>
        /// Retrieves column vector at specfifed index and deletes it from matrix.
        /// </summary>
        /// <param name="j">One-based index at which to extract.</param>
        /// <returns>Row vector.</returns>
        public ArrayMatrix ExtractColumn(int j)
        {
            if (j <= 0 || j > ColumnCount) throw new ArgumentException("Index must be positive and <= number of cols.");

            var buf = Column(j);
            DeleteColumn(j);

            return buf;
        }

        /// <summary>
        /// Inserts row at specified index.
        /// </summary>
        /// <param name="row">Vector to insert</param>
        /// <param name="i">One-based index at which to insert</param>
        public void InsertRow(ArrayMatrix row, int i)
        {
            var size = row.VectorLength();

            if (size == 0) throw new InvalidOperationException("Row must be a vector of length > 0.");

            if (i <= 0) throw new ArgumentException("Row index must be positive.");

            if (i > RowCount) this[i, size] = Complex.Zero;

            else if (size > ColumnCount)
            {
                this[i, size] = Complex.Zero;
                RowCount++;
            }
            else RowCount++;

            Values.Insert(--i, new ArrayList(size));
            //Debug.WriteLine(Values.Count.ToString());

            for (var k = 1; k <= size; k++) ((ArrayList) Values[i]).Add(row[k]);

            // fill w/ zeros if vector row is too short
            for (var k = size; k < ColumnCount; k++) ((ArrayList) Values[i]).Add(Complex.Zero);
        }

        /// <summary>
        /// Inserts a sub matrix M at row i and column j.
        /// </summary>
        /// <param name="i">One-based row number to insert.</param>
        /// <param name="j">One-based column number to insert.</param>
        /// <param name="M">Sub matrix to insert.</param>
        public void Insert(int i, int j, ArrayMatrix M)
        {
            for (var m = 1; m <= M.RowCount; m++) for (var n = 1; n <= M.ColumnCount; n++) this[i + m - 1, j + n - 1] = M[m, n];
        }

        /// <summary>
        /// Inserts column at specified index.
        /// </summary>
        /// <param name="col">Vector to insert</param>
        /// <param name="j">One-based index at which to insert</param>
        public void InsertColumn(ArrayMatrix col, int j)
        {
            var size = col.VectorLength();

            if (size == 0) throw new InvalidOperationException("Row must be a vector of length > 0.");

            if (j <= 0) throw new ArgumentException("Row index must be positive.");

            if (j > ColumnCount) this[size, j] = Complex.Zero;
            else ColumnCount++;

            if (size > RowCount) this[size, j] = Complex.Zero;

            j--;

            for (var k = 0; k < size; k++) ((ArrayList) Values[k]).Insert(j, col[k + 1]);

            // fill w/ zeros if vector col too short
            for (var k = size; k < RowCount; k++) ((ArrayList) Values[k]).Insert(j, 0);
        }

        /// <summary>
        /// Inverts square matrix as long as det != 0.
        /// </summary>
        /// <returns>Inverse of matrix.</returns>
        public ArrayMatrix Inverse()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot invert non-square matrix.");

            var det = Determinant();

            if (det == Complex.Zero) throw new InvalidOperationException("Cannot invert (nearly) singular matrix.");

            var n = ColumnCount;

            if (n == 1) return new ArrayMatrix(1/det);

            if (IsReal() && IsOrthogonal()) return Transpose();
            else if (IsUnitary()) return ConjTranspose();

            if (IsDiagonal())
            {
                var d = DiagVector();

                for (var i = 1; i <= n; i++) d[i] = 1/d[i];

                return Diag(d);
            }

            var buf = new Complex[n,n];

            for (var i = 0; i < n; i++) for (var j = 0; j < n; j++) buf[i, j] = Math.Pow(-1, i + j)*Minor(j + 1, i + 1).Determinant();

            return (new ArrayMatrix(buf)/det);
        }

        /// <summary>
        /// Alternative matrix inversion using Leverrier's formula
        /// </summary>
        /// <returns>Inverse of matrix.</returns>
        public ArrayMatrix InverseLeverrier()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot invert non-square matrix.");
            //else if (this.Determinant() == 0)
            //    throw new InvalidOperationException("Cannot invert (nearly) singular matrix.");

            var n = RowCount;
            var Id = Identity(n);
            var B = Id;
            Complex alpha;

            for (var k = 1; k < n; k++)
            {
                var buf = (this*B); // DEBUG                
                var buf2 = buf.Trace(); // DEBUG
                alpha = ((double) 1/k)*buf.Trace();
                B = alpha*Id - buf;
            }

            var buf3 = (this*B); // DEBUG                
            var buf4 = buf3.Trace(); // DEBUG
            alpha = (this*B).Trace()/n;
            if (alpha != Complex.Zero) return B/alpha;
            else throw new InvalidOperationException("WARNING: Matrix nearly singular or badly scaled.");
        }

        /// <summary>
        /// Calcs the matrix that results in the clearing of a
        /// specified row and a specified column
        /// </summary>        
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public ArrayMatrix Minor(int i, int j)
        {
            // THIS IS THE LOW-LEVEL SOLUTION ~ O(n^2)
            //Complex[,] buf = new Complex[RowCount - 1, ColumnCount - 1];
            //int r = 0;
            //int c = 0;

            //for (int i = 1; i <= RowCount; i++)
            //{
            //    if (i != row)
            //    {
            //        for (int j = 1; j <= ColumnCount; j++)
            //        {
            //            if (j != col)
            //            {
            //                buf[r, c] = this[i, j];
            //                c++;
            //            }
            //        }

            //        c = 0;
            //        r++;
            //    }
            //}

            //return new Matrix(buf);  

            // THIS IS THE HIGH-LEVEL SOLUTION ~ O(n)

            var A = Clone();

            A.DeleteRow(i);
            A.DeleteColumn(j);

            return A;
        }

        /// <summary>
        /// Provides a shallow copy of this matrix in O(m).
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix Clone()
        {
            var A = new ArrayMatrix();
            A.RowCount = RowCount;
            A.ColumnCount = ColumnCount;

            for (var i = 0; i < RowCount; i++) A.Values.Add(((ArrayList) Values[i]).Clone());

            return A;
        }

        /// <summary>
        /// Extracts main diagonal vector of the matrix as a column vector.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix DiagVector()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot get diagonal of non-square matrix.");

            var v = new ArrayMatrix(ColumnCount, 1);

            for (var i = 1; i <= ColumnCount; i++) v[i] = this[i, i];

            return v;
        }

        /// <summary>
        /// Retrieves column with one-based index j.
        /// </summary>
        /// <param name="j"></param>
        /// <returns>j-th column...</returns>
        public ArrayMatrix Column(int j)
        {
            var buf = new ArrayMatrix(RowCount, 1);

            for (var i = 1; i <= RowCount; i++) buf[i] = this[i, j];

            return buf;
        }

        /// <summary>
        /// Retrieves row with one-based index i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>i-th row...</returns>
        public ArrayMatrix Row(int i)
        {
            if (i <= 0 || i > RowCount) throw new ArgumentException("Index exceed matrix dimension.");

            //return (new Matrix((Complex[])((ArrayList)Values[i - 1]).ToArray(typeof(Complex)))).Transpose();

            var buf = new ArrayMatrix(ColumnCount, 1);

            for (var j = 1; j <= ColumnCount; j++) buf[j] = this[i, j];

            return buf;
        }

        /// <summary>
        /// Swaps each matrix entry A[i, j] with A[j, i].
        /// </summary>
        /// <returns>A transposed matrix.</returns>
        public ArrayMatrix Transpose()
        {
            var M = new ArrayMatrix(ColumnCount, RowCount);

            for (var i = 1; i <= ColumnCount; i++) for (var j = 1; j <= RowCount; j++) M[i, j] = this[j, i];

            return M;
        }

        /// <summary>
        /// Replaces each matrix entry z = x + iy with x - iy.
        /// </summary>
        /// <returns>Conjugated matrix.</returns>
        public ArrayMatrix Conjugate()
        {
            var M = new ArrayMatrix(RowCount, ColumnCount);

            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= ColumnCount; j++) M[i, j] = Complex.Conjugate(this[i, j]);

            return M;
        }

        /// <summary>
        /// Conjuagtes and transposes a matrix.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix ConjTranspose()
        {
            return Transpose().Conjugate();
        }

        /// <summary>
        /// Performs LU-decomposition of this instance and saves L and U
        /// within, where the diagonal elements belong to U
        /// (the ones of L are ones...)
        /// </summary>
        public void LU()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot perform LU-decomposition of non-square matrix.");

            var n = ColumnCount;

            for (var j = 1; j <= n; j++)
            {
                if (this[j, j] == 0)
                    throw new DivideByZeroException(
                        "Warning: Matrix badly scaled or close to singular. Try LUSafe() instead. Check if det != 0.");

                for (var k = 1; k < j; k++) for (var i = k + 1; i <= n; i++) this[i, j] = this[i, j] - this[i, k]*this[k, j];

                for (var i = j + 1; i <= n; i++) this[i, j] = this[i, j]/this[j, j];
            }
        }

        /// <summary>
        /// Performs safe LU-decomposition of this instance with column pivoting 
        /// and saves L and U
        /// within, where the diagonal elements belong to U
        /// (the ones of L are ones...)
        /// </summary>
        /// <returns>Permutation matrix P with P*this = L*U</returns>
        /// <remarks>This needs additional time O(n^2).</remarks>
        public ArrayMatrix LUSafe()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot perform LU-decomposition of non-square matrix.");

            var n = ColumnCount;

            var P = Identity(n); // permutation matrix
            int m;

            for (var j = 1; j <= n; j++)
            {
                //--> this test means probably deceleration
                //if (j < n && this.Extract(j + 1, n, j, j) == Zeros(n - j, 1))
                //    continue;

                #region Column pivoting

                // find index m with |this[m,j]| >= |this[i,j]| for all i in {j,...,n}
                if (j < n)
                {
                    m = j;

                    for (var i = j + 1; i <= n; i++) if (Complex.Abs(this[i, j]) > Complex.Abs(this[m, j])) m = i;

                    if (m > j) // <=> j2 != j
                    {
                        P.SwapRows(j, m);
                        SwapRows(j, m);
                    }

                    if (this[j, j] == 0) throw new DivideByZeroException("Warning: Matrix close to singular.");
                }

                #endregion

                for (var k = 1; k < j; k++) for (var i = k + 1; i <= n; i++) this[i, j] = this[i, j] - this[i, k]*this[k, j];

                for (var i = j + 1; i <= n; i++) this[i, j] = this[i, j]/this[j, j];
            }

            return P;
        }

        /// <summary>
        /// Performs Cholesky decomposition of square, symmetric and positive definite
        /// matrix A = LL', where L is a lower triangular matrix. L is saved in the
        /// lower triangular part of A.</summary>
        /// <remarks>
        /// The diagonal elements can be retrieved
        /// by a_{11} = h_{11}^2, a_{ii} = h_{ii}^2 + \sum_{k=1}^{i-1}h_{ik}^2 (i = 2..n).
        /// Use CholeskyUndo() for convenience.
        /// WARNING: Cholesky decomposition only works for symmetric positive definite matrices!
        /// </remarks>        
        public void Cholesky()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot perform Cholesky decomposition of non-square matrix.");

            if (!IsSymmetricPositiveDefinite())
                throw new InvalidOperationException(
                    "Cannot perform Cholesky decomposition of matrix not being symmetric positive definite.");

            var n = RowCount;

            for (var k = 1; k < n; k++)
            {
                this[k, k] = Complex.Sqrt(this[k, k]);

                for (var i = 1; i <= n - k; i++) this[k + i, k] = this[k + i, k]/this[k, k];

                for (var j = k + 1; j <= n; j++) for (var i = 0; i <= n - j; i++) this[j + i, j] = this[j + i, j] - this[j + i, k]*this[j, k];
            }

            this[n, n] = Complex.Sqrt(this[n, n]);
        }

        /// <summary>
        /// Since the cholesky decomposition is saved within the symmetric matrix to be
        /// decomposited, it can be undone to restore the initial matrix.
        /// </summary>
        public void CholeskyUndo()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot undo cholesky decomposition on non-square matrix.");

            this[1, 1] = Sqr(this[1, 1]);

            Complex buf;

            for (var i = 2; i <= RowCount; i++)
            {
                buf = Complex.Zero;

                for (var k = 1; k <= i - 1; k++) buf += Sqr(this[i, k]);

                this[i, i] = Sqr(this[i, i]) + buf;
            }

            SymmetrizeDown();
        }

        /// <summary>
        /// Performs forward insertion for regular lower triangular matrix
        /// and right side b, such that the solution is saved right within b.
        /// The matrix is not changed.
        /// </summary>
        /// <param name="b">Vector of height n, if matrix is n by n.</param>
        public void ForwardInsertion(ArrayMatrix b)
        {
            if (!IsLowerTriangular()) throw new InvalidOperationException("Cannot perform forward insertion for matrix not being lower triangular.");

            if ( /*this.Determinant*/DiagProd() == 0) throw new InvalidOperationException("Warning: Matrix is nearly singular.");

            var n = RowCount;

            if (b.VectorLength() != n) throw new ArgumentException("Parameter must vector of the same height as matrix.");

            for (var j = 1; j <= n - 1; j++)
            {
                b[j] /= this[j, j];

                for (var i = 1; i <= n - j; i++) b[j + i] -= b[j]*this[j + i, j];
            }

            b[n] /= this[n, n];
        }

        /// <summary>
        /// Performs backward insertion for regular upper triangular matrix
        /// and right side b, such that the solution is saved right within b.
        /// The matrix is not changed.
        /// </summary>
        /// <param name="b">Vector of height n, if matrix is n by n.</param>
        public void BackwardInsertion(ArrayMatrix b)
        {
            if (!IsUpperTriangular())
                throw new InvalidOperationException(
                    "Cannot perform backward insertion for matrix not being upper triangular.");

            if ( /*this.Determinant*/DiagProd() == 0) throw new InvalidOperationException("Warning: Matrix is nearly singular.");

            var n = RowCount;

            if (b.VectorLength() != n) throw new ArgumentException("Parameter must vector of the same height as matrix.");

            for (var j = n; j >= 2; j--)
            {
                b[j] /= this[j, j];

                for (var i = 1; i <= j - 1; i++) b[i] -= b[j]*this[i, j];
            }

            b[1] /= this[1, 1];
        }

        /// <summary>
        /// Makes square matrix symmetric by copying the upper half to the lower half.
        /// </summary>
        public void SymmetrizeDown()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot symmetrize non-square matrix.");

            for (var j = 1; j <= ColumnCount; j++) for (var i = j + 1; i <= ColumnCount; i++) this[i, j] = this[j, i];
        }

        /// <summary>
        /// Makes square matrix symmetric by copying the lower half to the upper half.
        /// </summary>
        public void SymmetrizeUp()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot symmetrize non-square matrix.");

            for (var i = 1; i <= RowCount; i++) for (var j = i + 1; j <= ColumnCount; j++) this[i, j] = this[j, i];
        }

        /// <summary>
        /// Gram-Schmidtian orthogonalization of an m by n matrix A, such that
        /// {Q, R} is returned, where A = QR, Q is m by n and orthogonal, R is
        /// n by n and upper triangular matrix.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix[] QRGramSchmidt()
        {
            var m = RowCount;
            var n = ColumnCount;

            var A = Clone();

            var Q = new ArrayMatrix(m, n);
            var R = new ArrayMatrix(n, n);

            // the first column of Q equals the first column of this matrix
            for (var i = 1; i <= m; i++) Q[i, 1] = A[i, 1];

            R[1, 1] = Complex.One;

            for (var k = 1; k <= n; k++)
            {
                R[k, k] = new Complex(A.Column(k).Norm());

                for (var i = 1; i <= m; i++) Q[i, k] = A[i, k]/R[k, k];

                for (var j = k + 1; j <= n; j++)
                {
                    R[k, j] = Dot(Q.Column(k), A.Column(j));

                    for (var i = 1; i <= m; i++) A[i, j] = A[i, j] - Q[i, k]*R[k, j];
                }
            }

            return new ArrayMatrix[] {Q, R};
        }

        /// <summary>
        /// Computes approximates of the eigenvalues of this matrix. WARNING: Computation
        /// uses basic QR iteration with Gram-Schmidtian orthogonalization. This implies that
        /// (1) only real matrices can be examined; (2) if the matrix has a multiple eigenvalue
        /// or complex eigenvalues, partial junk is returned. This is due to the eigenvalues having
        /// to be like |L1| > |L2| > ... > |Ln| for QR iteration to work properly.
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix Eigenvalues()
        {
            return QRIterationBasic(40).DiagVector();
        }

        /// <summary>
        /// Computes eigenvector from eigenvalue.
        /// </summary>
        /// <param name="eigenvalue"></param>
        /// <returns></returns>
        public ArrayMatrix Eigenvector(Complex eigenvalue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Solves equation this*x = b via conjugate gradient method.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public ArrayMatrix SolveCG(ArrayMatrix b)
        {
            //throw new NotImplementedException("Still buggy!");

            if (!IsSymmetricPositiveDefinite()) throw new InvalidOperationException("CG method only works for spd matrices.");
            else if (!IsReal()) throw new InvalidOperationException("CG method only works for real matrices.");

            var n = RowCount;
            var max_iterations = 150;
            var tolerance = 1e-6;

            var x = Ones(n, 1); // x will contain the solution
            var r = b - this*x; // residual approaches zero as x converges to the solution
            var d = r; // dir = direction of descence
            var delta = r.Norm(); // delta denotes the current error
            delta *= delta;
            tolerance *= tolerance;

            var h = Zeros(n, 1);
            double alpha, gamma;
            double old_delta;

            if (delta <= tolerance) return x;
            else
            {
                for (var i = 0; i < max_iterations; i++)
                {
                    h = this*d;
                    gamma = Dot(h, d).Real;

                    if (Math.Abs(gamma) <= tolerance) return x;

                    alpha = delta/gamma;

                    x += alpha*d; // compute new approximation of solution
                    r -= alpha*h; // compute new residual

                    old_delta = delta; // buffer delta

                    delta = r.Norm();
                    delta *= delta;

                    if (delta <= tolerance) return x;

                    d = r + delta/old_delta*d; // compute new direction of descence
                }

                return x;
            }
        }

        /// <summary>
        /// Executes the QR iteration.
        /// </summary>
        /// <param name="max_iterations"></param>
        /// <returns></returns>
        public ArrayMatrix QRIterationBasic(int max_iterations)
        {
            if (!IsReal()) throw new InvalidOperationException("Basic QR iteration is possible only for real matrices.");

            var T = Clone();
            var QR = new ArrayMatrix[2];

            for (var i = 0; i < max_iterations; i++)
            {
                QR = T.QRGramSchmidt();
                T = QR[1]*QR[0];
            }

            return T;
        }

        /// <summary>
        /// QR iteration using Hessenberg-Householder reduction.
        /// </summary>
        /// <param name="max_iterations"></param>
        /// <returns></returns>
        public ArrayMatrix QRIterationHessenberg(int max_iterations)
        {
            //throw new NotImplementedException("Still buggy!");

            if (!IsSquare()) throw new InvalidOperationException("Cannot perform QR iteration of non-square matrix.");

            var n = RowCount;

            var TQ = HessenbergHouseholder();
            var T = TQ[0];

            for (var j = 1; j <= max_iterations; j++)
            {
                var QRcs = T.QRGivens();
                T = QRcs[1];

                for (var k = 1; k <= n - 1; k++) T.Gacol(QRcs[2][k], QRcs[3][k], 1, k + 1, k, k + 1);
            }

            return T;
        }

        /// <summary>
        /// QR factorization avec Givens rotations.
        /// </summary>
        /// <param name="H"></param>
        /// <returns></returns>
        public ArrayMatrix[] QRGivens()
        {
            //throw new NotImplementedException("Still buggy!");

            var H = Clone();
            var m = H.RowCount;
            var n = H.ColumnCount;

            var c = Zeros(n - 1, 1);
            var s = Zeros(n - 1, 1);
            Complex[] cs;

            for (var k = 1; k <= n - 1; k++)
            {
                cs = GivensCS(H[k, k], H[k + 1, k]);
                c[k] = cs[0];
                s[k] = cs[1];
                Garow(c[k], s[k], 1, k + 1, k, k + 1);
            }

            return new ArrayMatrix[] {GivProd(c, s, n), H, c, s};
        }

        /// <summary>
        /// Givens product. Internal use for QRGivens.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        ArrayMatrix GivProd(ArrayMatrix c, ArrayMatrix s, int n)
        {
            var n1 = n - 1;
            var n2 = n - 2;

            var Q = Eye(n);
            Q[n1, n1] = c[n1];
            Q[n, n] = c[n1];
            Q[n1, n] = s[n1];
            Q[n, n1] = -s[n1];

            for (var k = n2; k >= 1; k--)
            {
                var k1 = k + 1;
                Q[k, k] = c[k];
                Q[k1, k] = -s[k];
                var q = Q.Extract(k1, k1, k1, n);
                Q.Insert(k, k1, s[k]*q);
                Q.Insert(k1, k1, c[k]*q);
            }

            return Q;
        }

        /// <summary>
        /// Product G(i,k,theta)'*this. Internal use for QRGivens.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <param name="j1"></param>
        /// <param name="j2"></param>
        /// <returns></returns>
        void Garow(Complex c, Complex s, int i, int k, int j1, int j2)
        {
            for (var j = j1; j <= j2; j++)
            {
                var t1 = this[i, j];
                var t2 = this[k, j];
                this[i, j] = c*t1 - s*t2;
                this[k, j] = s*t1 + c*t2;
            }
        }

        /// <summary>
        /// Product M*G(i,k,theta). Internal use for QRGivens.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="j1"></param>
        /// <param name="j2"></param>
        /// <param name="i"></param>
        /// <param name="k"></param>
        public void Gacol(Complex c, Complex s, int j1, int j2, int i, int k)
        {
            for (var j = j1; j <= j2; j++)
            {
                var t1 = this[j, i];
                var t2 = this[j, k];

                this[j, i] = c*t1 - s*t2;
                this[j, k] = s*t1 + c*t2;
            }
        }

        /// <summary>
        /// Computes Givesn sine and cosine.
        /// </summary>
        /// <param name="xi"></param>
        /// <param name="xk"></param>
        /// <returns></returns>
        Complex[] GivensCS(Complex xi, Complex xk)
        {
            var c = Complex.Zero;
            var s = Complex.Zero;

            if (xk == 0) c = Complex.One;
            else if (Complex.Abs(xk) > Complex.Abs(xi))
            {
                var t = -xi/xk;
                s = 1/(Complex.Sqrt(1 + t*t));
                c = s*t;
            }
            else
            {
                var t = -xk/xi;
                c = 1/(Complex.Sqrt(1 + t*t));
                s = c*t;
            }

            return new Complex[] {c, s};
        }

        #endregion

        #region Numbers

        /// <summary>
        /// Calcs determinant of square matrix
        /// </summary>
        /// <returns></returns>
        public Complex Determinant()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot calc determinant of non-square matrix.");
            else if (ColumnCount == 1) return this[1, 1];
            else if (IsTrapeze()) // is square, therefore triangular
                return DiagProd();
            else
            {
                // perform LU-decomposition & return product of diagonal elements of U
                var X = Clone();

                // for speed concerns, use this
                //X.LU();
                //return X.DiagProd();

                // this is slower and needs more memory... .
                var P = X.LUSafe();
                return (double) P.Signum()*X.DiagProd();
            }
        }

        public double ColumnSumNorm()
        {
            return TaxiNorm();
        }

        public double RowSumNorm()
        {
            return MaxNorm();
        }

        /// <summary>
        /// Computes the permanent of the current instance. WARNING: This algorithm has exponential runtime.
        /// Don't use for any but very small instances.
        /// </summary>
        /// <returns></returns>
        public Complex Permanent()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot compute permanent of non-square matrix.");

            if (HasZeroRowOrColumn()) return Complex.Zero;

            if (this == Ones(RowCount)) return new Complex(Factorial(RowCount));

            if (IsPermutation()) return Complex.One;

            var buf = Complex.Zero;

            var minRow = GetMinRow();
            var minColumn = GetMinColumn();

            if (AbsRowSum(minRow) < AbsColumnSum(minColumn))
            {
                for (var j = 1; j <= ColumnCount; j++) if (this[minRow, j] != 0) buf += this[minRow, j]*Minor(minRow, j).Permanent();
            }
            else for (var i = 1; i <= RowCount; i++) if (this[i, minColumn] != 0) buf += this[i, minColumn]*Minor(i, minColumn).Permanent();

            return buf;
        }

        /// <summary>
        /// Finds index of row with minimal AbsRowSum.
        /// </summary>
        /// <returns></returns>
        public int GetMinRow()
        {
            var buf = AbsRowSum(1);
            var index = 1;

            double buf2;

            for (var i = 2; i <= RowCount; i++)
            {
                buf2 = AbsRowSum(i);
                if (buf2 < buf)
                {
                    buf = buf2;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// Finds index of column with minimal AbsColumnSum.
        /// </summary>
        /// <returns></returns>
        public int GetMinColumn()
        {
            var buf = AbsColumnSum(1);
            var index = 1;

            double buf2;

            for (var j = 2; j <= ColumnCount; j++)
            {
                buf2 = AbsColumnSum(j);
                if (buf2 < buf)
                {
                    buf = buf2;
                    index = j;
                }
            }

            return index;
        }

        double Factorial(int x)
        {
            double buf = 1;
            for (var i = 2; i <= x; i++) buf *= i;

            return buf;
        }

        /// <summary>
        /// Computes signum of a permutation matrix, which is 1 for an even
        /// number of swaps and -1 for an odd number of swaps. WARNING: 
        /// if *this is not a permutation matrix (e.i. a permutation of Id),
        /// garbage is returned.
        /// </summary>
        /// <returns></returns>
        public double Signum()
        {
            double buf = 1;

            var n = RowCount;
            double fi, fj;

            for (var i = 1; i < n; i++)
            {
                for (fi = 1; fi < n && this[i, (int) fi] != Complex.One; fi++) ;

                for (var j = i + 1; j <= n; j++)
                {
                    for (fj = 1; fj <= n && this[j, (int) fj] != Complex.One; fj++) ;

                    buf *= (fi - fj)/(i - j);
                }
            }

            return buf;
        }

        /// <summary>
        ///  Calcs condition number with respect to inversion
        /// by using |A|*|inv(A)| and 1-Norm.
        /// </summary>
        /// <returns></returns>
        public double Condition()
        {
            return TaxiNorm()*Inverse().TaxiNorm();
        }

        /// <summary>
        ///  Calcs condition number with respect to inversion
        /// by using |A|*|inv(A)| and p norm.
        /// </summary>
        /// <param name="p">Specifies the norm to be used. Can be one or positive infinity.</param>
        /// <returns></returns>
        public double Condition(int p)
        {
            return PNorm(p)*Inverse().PNorm(p);
        }

        /// <summary>
        ///  Calcs condition number with respect to inversion
        /// by using |A|*|inv(A)| and frobenius norm.
        /// </summary>
        /// <returns></returns>
        public double ConditionFro()
        {
            return FrobeniusNorm()*Inverse().FrobeniusNorm();
        }

        /// <summary>
        /// Calcs p-norm of given matrix: p-th root of the sum
        /// of the p-th powers of the absolute values of all matrix entries.
        /// </summary>
        /// <param name="p">Which norm to compute; can be positive infinity.</param>
        /// <returns></returns>
        /// <remarks>If p not in {i, +infty}, *this must be a vector.</remarks>
        public double PNorm(double p)
        {
            if (p <= 0) throw new ArgumentException("Argument must be greater than zero.");

            if (p == 1) return TaxiNorm();
            else if (p == double.PositiveInfinity) return MaxNorm();

            var dim = VectorLength();
            if (dim == 0) throw new InvalidOperationException("Cannot calc p-norm of matrix.");

            double buf = 0;

            for (var i = 1; i <= dim; i++) buf += Math.Pow(Complex.Abs(this[i]), p);

            return Math.Pow(buf, 1/p);
        }

        /// <summary>
        /// 2-Norm for vectors. If *this is a matrix, you might want to choose
        /// FrobeniusNorm().
        /// </summary>
        /// <returns></returns>
        public double Norm()
        {
            return PNorm(2);
        }

        /// <summary>
        /// Frobenius norm of a square matrix. If *this is a vector, this method
        /// is equivalent to Norm() and PNorm(2).
        /// </summary>
        /// <returns></returns>
        public double FrobeniusNorm()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot compute frobenius norm of non-square matrix.");

            var n = ColumnCount;
            double buf = 0;

            for (var i = 1; i <= n; i++) for (var j = 1; j <= n; j++) buf += (this[i, j]*Complex.Conjugate(this[i, j])).Real;

            return Math.Sqrt(buf);
        }

        /// <summary>
        /// Also known as column-sum norm.
        /// </summary>
        /// <returns>Maximal AbsColumnSum</returns>
        public double TaxiNorm()
        {
            double buf = 0;

            var dim = VectorLength();

            if (dim != 0) // vector case
                for (var i = 1; i <= dim; i++) buf += Complex.Abs(this[i]);
            else // general case
            {
                double buf2 = 0;

                for (var j = 1; j <= ColumnCount; j++)
                {
                    buf2 = AbsColumnSum(j);
                    if (buf2 > buf) buf = buf2;
                }
            }

            return buf;
        }

        /// <summary>
        /// Also known as row-sum norm.
        /// </summary>
        /// <returns>Maximal AbsRowSum</returns>
        public double MaxNorm()
        {
            double buf = 0;
            double buf2 = 0;

            var dim = VectorLength();

            if (dim != 0) // vector case
                for (var i = 1; i <= dim; i++)
                {
                    buf2 = Complex.Abs(this[i]);
                    if (buf2 > buf) buf = buf2;
                }
            else // general case
                for (var i = 1; i <= RowCount; i++)
                {
                    buf2 = AbsRowSum(i);
                    if (buf2 > buf) buf = buf2;
                }

            return buf;
        }

        /// <summary>
        /// Calcs sum of the elements of a certain col.
        /// </summary>
        /// <param name="i">One-based index of the col to consider.</param>
        /// <returns></returns>
        public Complex ColumnSum(int j)
        {
            if (j <= 0 || j > ColumnCount) throw new ArgumentException("Index out of range.");

            var buf = Complex.Zero;

            j--;

            for (var i = 0; i < RowCount; i++) buf += (Complex) (((ArrayList) Values[i])[j]);

            return buf;
        }

        /// <summary>
        /// Calcs sum of the absolute values of the elements of a certain col.
        /// </summary>
        /// <param name="i">One-based index of the col to consider.</param>
        /// <returns></returns>
        public double AbsColumnSum(int j)
        {
            if (j <= 0 || j > ColumnCount) throw new ArgumentException("Index out of range.");

            double buf = 0;

            for (var i = 1; i <= RowCount; i++) buf += Complex.Abs(this[i, j]);

            return buf;
        }

        /// <summary>
        /// Calcs sum of the elements of a certain row.
        /// </summary>
        /// <param name="i">One-based index of the row to consider.</param>
        /// <returns></returns>
        public Complex RowSum(int i)
        {
            if (i <= 0 || i > RowCount) throw new ArgumentException("Index out of range.");

            var buf = Complex.Zero;

            var row = (ArrayList) Values[i - 1];

            for (var j = 0; j < ColumnCount; j++) buf += (Complex) (row[j]);

            return buf;
        }

        /// <summary>
        /// Calcs sum of the absolute values of the elements of a certain row.
        /// </summary>
        /// <param name="i">One-based index of the row to consider.</param>
        /// <returns></returns>
        public double AbsRowSum(int i)
        {
            if (i <= 0 || i > RowCount) throw new ArgumentException("Index out of range.");

            double buf = 0;

            for (var j = 1; j <= ColumnCount; j++) buf += Complex.Abs(this[i, j]);

            return buf;
        }

        /// <summary>
        /// Computes product of main diagonal entries.
        /// </summary>
        /// <returns>Product of diagonal elements</returns>
        public Complex DiagProd()
        {
            var buf = Complex.One;
            var dim = Math.Min(RowCount, ColumnCount);

            for (var i = 1; i <= dim; i++) buf *= this[i, i];

            return buf;
        }

        /// <summary>
        /// Calcs trace of the matrix.
        /// </summary>
        /// <returns>Sum of diagonal elements.</returns>
        public Complex Trace()
        {
            if (!IsSquare()) throw new InvalidOperationException("Cannot calc trace of non-square matrix.");

            var buf = Complex.Zero;

            for (var i = 1; i <= RowCount; i++) buf += this[i, i];

            return buf;
        }

        Complex Sqr(Complex x)
        {
            return x*x;
        }

        #endregion

        #region Checks

        /// <summary>
        /// Matrix is normal, iff A*A^H = A^H*A, where A is the conjugated transposed of A.
        /// </summary>
        /// <returns></returns>
        public bool IsNormal()
        {
            return (this*ConjTranspose() == ConjTranspose()*this);
        }

        /// <summary>
        /// Matrix is unitary, iff A^H*A = id, where A^H is the conjugated transpose of A.
        /// </summary>
        /// <returns>True iff matrix is unitary.</returns>
        public bool IsUnitary()
        {
            if (!IsSquare()) return false;

            return (ConjTranspose()*this == Identity(RowCount));
        }

        /// <summary>
        /// Matrix A is Hermitian iff A^H = A, where A^H is the conjugated transposed of A.
        /// </summary>
        /// <returns>True iff matrix is Hermitian.</returns>
        public bool IsHermitian()
        {
            if (!IsSquare()) return false;

            return ConjTranspose() == this;
        }

        /// <summary>
        /// Checks if matrix consists only of real entries.
        /// </summary>
        /// <returns>True iff all entries are real.</returns>
        public bool IsReal()
        {
            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= ColumnCount; j++) if (!this[i, j].IsReal) return false;

            return true;
        }

        /// <summary>
        /// Checks for symmetric positive definiteness.
        /// </summary>
        /// <returns>True iff matrix is symmetrix positive definite.</returns>
        public bool IsSymmetricPositiveDefinite()
        {
            return (IsSymmetric() && Definiteness() == DefinitenessType.PositiveDefinite);
        }

        /// <summary>
        /// Finds out the type of definiteness of a symmetric square matrix.
        /// </summary>
        /// <returns></returns>
        public DefinitenessType Definiteness()
        {
            if (!IsSquare()) throw new InvalidOperationException("Definiteness undefined for non-square matrices.");
            else if (this == Zeros(RowCount, ColumnCount)) return DefinitenessType.Indefinite;
            else if (!IsSymmetric()) throw new InvalidOperationException("This test works only for symmetric matrices.");
            else if (!IsReal()) throw new InvalidOperationException("This test only works for real matrices.");

            // step 1: construct orthogonal basis for A
            // using Gram-Schmidt orthogonalization
            var n = RowCount;

            var y = new ArrayMatrix[n + 1];
            for (var i = 0; i <= n; i++) y[i] = ArrayMatrix.Zeros(n, 1);

            y[1] = Column(1);

            ArrayMatrix xk; // to buffer this.Column(k)
            ArrayMatrix buf;

            // Gram-Schmidt:
            for (var k = 2; k <= n; k++)
            {
                xk = Column(k);

                buf = Zeros(n, 1);

                for (var i = 1; i < k; i++) buf += y[i]*Dot(xk, this*y[i])/Dot(y[i], this*y[i]);

                y[k] = xk - buf;
            }

            // step 2: test for definiteness; 
            // e.g. A pos. def. <=> A > 0 <=> y[i]'Ay[i] > 0 for all i (same for neg. def., ...)

            var strict = true; // pos. def || neg. def.
            Complex res;

            for (var i = 1; i < n; i++)
            {
                res = Dot(y[i], this*y[i])*Dot(y[i + 1], this*y[i + 1]);

                if (res == 0) strict = false;
                else if (res.Real < 0) return DefinitenessType.Indefinite;
            }

            if (Dot(y[1], this*y[1]).Real >= 0)
                if (strict) return DefinitenessType.PositiveDefinite;
                else return DefinitenessType.PositiveSemidefinite;
            else if (strict) return DefinitenessType.NegativeDefinite;
            else return DefinitenessType.NegativeSemidefinite;
        }

        /// <summary>
        /// Checks if matrix has a row or column consisting of zeros.
        /// </summary>
        /// <returns>True iff so.</returns>
        public bool HasZeroRowOrColumn()
        {
            for (var i = 1; i <= RowCount; i++) if (AbsRowSum(i) == 0) return true;

            for (var i = 1; i <= ColumnCount; i++) if (AbsColumnSum(i) == 0) return true;

            return false;
        }

        /// <summary>
        /// Checks if matrix consists only of zeros and ones.
        /// </summary>
        /// <returns></returns>
        public bool IsZeroOneMatrix()
        {
            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= ColumnCount; j++) if (this[i, j] != Complex.Zero && this[i, j] != Complex.One) return false;

            return true;
        }

        /// <summary>
        /// Checks if matrix is permutation of the identity matrix.
        /// </summary>
        /// <returns>True iff matrix is permutation matrix.</returns>
        public bool IsPermutation()
        {
            return (!IsSquare() && IsZeroOneMatrix() && IsInvolutary());
        }

        /// <summary>
        /// Checks if matrix is diagonal matrix.
        /// </summary>
        /// <returns>True iff matrix is diagonal.</returns>
        public bool IsDiagonal()
        {
            return (Clone() - Diag(DiagVector()) == Zeros(RowCount, ColumnCount));
        }

        /// <summary>
        /// Checks if matrix is n by one or one by n.
        /// </summary>
        /// <returns>Length, if vector; zero else.</returns>
        public int VectorLength()
        {
            if (ColumnCount > 1 && RowCount > 1) return 0;
            else return Math.Max(ColumnCount, RowCount);
        }

        /// <summary>
        /// Checks if number of rows equals number of columns.
        /// </summary>
        /// <returns>True iff matrix is n by n.</returns>
        public bool IsSquare()
        {
            return (ColumnCount == RowCount);
        }

        /// <summary>
        /// Checks if matrix is involutary, e.i. if A*A = id.
        /// </summary>
        /// <returns>True iff matrix is involutary.</returns>
        public bool IsInvolutary()
        {
            return (this*this == Identity(RowCount));
        }

        /// <summary>
        /// Checks if A[i, j] == A[j, i].
        /// </summary>
        /// <returns>True iff matrix is symmetric.</returns>
        public bool IsSymmetric()
        {
            for (var i = 1; i <= RowCount; i++) for (var j = 1; j <= ColumnCount; j++) if (this[i, j] != this[j, i]) return false;

            return true;
        }

        /// <summary>
        /// Checks for orthogonality by testing if A*A' == id.
        /// </summary>
        /// <returns>True iff matrix is orthogonal.</returns>
        public bool IsOrthogonal()
        {
            return (IsSquare() && this*Transpose() == Identity(RowCount));
        }

        /// <summary>
        /// Checks if matrix is lower or upper trapeze.
        /// </summary>
        /// <returns>True iff matrix is trapeze.</returns>
        public bool IsTrapeze()
        {
            return (IsUpperTrapeze() || IsLowerTrapeze());
        }

        /// <summary>
        /// Checks if matrix is trapeze and square.
        /// </summary>
        /// <returns>True iff matrix is triangular.</returns>
        public bool IsTriangular()
        {
            return (IsLowerTriangular() || IsUpperTriangular());
        }

        /// <summary>
        /// Checks if matrix is square and upper trapeze.
        /// </summary>
        /// <returns>True iff matrix is upper triangular.</returns>
        public bool IsUpperTriangular()
        {
            return (IsSquare() && IsUpperTrapeze());
        }

        /// <summary>
        /// Checks if matrix is square and lower trapeze.
        /// </summary>
        /// <returns>True iff matrix is lower triangular.</returns>
        public bool IsLowerTriangular()
        {
            return (IsSquare() && IsLowerTrapeze());
        }

        /// <summary>
        /// Checks if A[i, j] == 0 for i < j.
        /// </summary>
        /// <returns>True iff matrix is upper trapeze.</returns>
        public bool IsUpperTrapeze()
        {
            for (var j = 1; j <= ColumnCount; j++) for (var i = j + 1; i <= RowCount; i++) if (this[i, j] != 0) return false;

            return true;
        }

        /// <summary>
        /// Checks if A[i, j] == 0 for i > j.
        /// </summary>
        /// <returns>True iff matrix is lower trapeze.</returns>
        public bool IsLowerTrapeze()
        {
            for (var i = 1; i <= RowCount; i++) for (var j = i + 1; j <= ColumnCount; j++) if (this[i, j] != 0) return false;

            return true;
        }

        #endregion

        #endregion

        #region Overrides & Operators

        public override String ToString()
        {
            var s = "";
            Complex buf;

            for (var i = 1; i <= RowCount; i++)
            {
                for (var j = 1; j <= ColumnCount; j++)
                {
                    buf = this[i, j];
                    if (buf.Real == double.PositiveInfinity || buf.Real == double.NegativeInfinity
                        || buf.Imag == double.PositiveInfinity || buf.Imag == double.NegativeInfinity) s += "oo";
                    else if (buf.Real == double.NaN || buf.Imag == double.NaN) s += "?";
                    else s += buf.ToString();

                    s += ";" + "\t";
                }

                s += "\\" + System.Environment.NewLine;
            }

            return s;
        }

        public String ToString(String format)
        {
            var s = "";
            Complex buf;

            for (var i = 1; i <= RowCount; i++)
            {
                for (var j = 1; j <= ColumnCount; j++)
                {
                    buf = this[i, j];
                    if (buf.Real == double.PositiveInfinity || buf.Real == double.NegativeInfinity
                        || buf.Imag == double.PositiveInfinity || buf.Imag == double.NegativeInfinity) s += "oo";
                    else if (buf.Real == double.NaN || buf.Imag == double.NaN) s += "?";
                    else s += buf.ToString(format);

                    s += ";" + "\t";
                }

                s += "\\" + System.Environment.NewLine;
            }

            return s;
        }

        public override bool Equals(object obj)
        {
            return obj.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return -1;
        }

        public static bool operator ==(ArrayMatrix A, ArrayMatrix B)
        {
            if (A.RowCount != B.RowCount || A.ColumnCount != B.ColumnCount) return false;

            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= A.ColumnCount; j++) if (A[i, j] != B[i, j]) return false;

            return true;
        }

        public static bool operator !=(ArrayMatrix A, ArrayMatrix B)
        {
            return !(A == B);
        }

        public static ArrayMatrix operator +(ArrayMatrix A, ArrayMatrix B)
        {
            if (A.RowCount != B.RowCount || A.ColumnCount != B.ColumnCount) throw new ArgumentException("Matrices must be of the same dimension.");

            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= A.ColumnCount; j++) A[i, j] += B[i, j];

            return A;
        }

        public static ArrayMatrix operator -(ArrayMatrix A, ArrayMatrix B)
        {
            if (A.RowCount != B.RowCount || A.ColumnCount != B.ColumnCount) throw new ArgumentException("Matrices must be of the same dimension.");

            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= A.ColumnCount; j++) A[i, j] -= B[i, j];

            return A;
        }

        public static ArrayMatrix operator -(ArrayMatrix A)
        {
            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= A.ColumnCount; j++) A[i, j] = -A[i, j];

            return A;
        }

        public static ArrayMatrix operator *(ArrayMatrix A, ArrayMatrix B)
        {
            if (A.ColumnCount != B.RowCount) throw new ArgumentException("Inner matrix dimensions must agree.");

            var C = new ArrayMatrix(A.RowCount, B.ColumnCount);

            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= B.ColumnCount; j++) C[i, j] = Dot(A.Row(i), B.Column(j));

            return C;
        }

        public static ArrayMatrix operator *(ArrayMatrix A, Complex x)
        {
            var B = new ArrayMatrix(A.RowCount, A.ColumnCount);

            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= A.ColumnCount; j++) B[i, j] = A[i, j]*x;

            return B;
        }

        public static ArrayMatrix operator *(Complex x, ArrayMatrix A)
        {
            var B = new ArrayMatrix(A.RowCount, A.ColumnCount);

            for (var i = 1; i <= A.RowCount; i++) for (var j = 1; j <= A.ColumnCount; j++) B[i, j] = A[i, j]*x;

            return B;
        }

        public static ArrayMatrix operator *(ArrayMatrix A, double x)
        {
            return (new Complex(x))*A;
        }

        public static ArrayMatrix operator *(double x, ArrayMatrix A)
        {
            return (new Complex(x))*A;
        }

        public static ArrayMatrix operator /(ArrayMatrix A, Complex x)
        {
            return (1/x)*A;
        }

        public static ArrayMatrix operator /(ArrayMatrix A, double x)
        {
            return (new Complex(1/x))*A;
        }

        public static ArrayMatrix operator ^(ArrayMatrix A, int k)
        {
            if (k < 0)
                if (A.IsSquare()) return A.InverseLeverrier() ^ (-k);
                else throw new InvalidOperationException("Cannot take non-square matrix to the power of zero.");
            else if (k == 0)
                if (A.IsSquare()) return ArrayMatrix.Identity(A.RowCount);
                else throw new InvalidOperationException("Cannot take non-square matrix to the power of zero.");
            else if (k == 1)
                if (A.IsSquare()) return A;
                else throw new InvalidOperationException("Cannot take non-square matrix to the power of one.");
            else
            {
                var M = A;
                for (var i = 1; i < k; i++) M *= A;

                return M;
            }
        }

        #endregion

        #region Virtualös

        /// <summary>
        /// Access the component in row i, column j of a non-empty matrix.
        /// </summary>
        /// <param name="i">One-based row index.</param>
        /// <param name="j">One-based column index.</param>
        /// <returns></returns>
        public virtual Complex this[int i, int j]
        {
            get
            {
                if (i > 0 && i <= RowCount && j > 0 && j <= ColumnCount) //Complex buf;
                    //
                    //try
                    //{
                    //    buf = (Complex)(((ArrayList)Values[i - 1])[j - 1]);
                    //}
                    //catch
                    //{
                    //    buf = new Complex((double)((int)(((ArrayList)Values[i - 1])[j - 1])));
                    //}
                    //
                    // return buf;                    
                    return (Complex) (((ArrayList) Values[i - 1])[j - 1]);
                else throw new ArgumentOutOfRangeException("Indices must not exceed size of matrix.");
            }
            set
            {
                if (i <= 0 || j <= 0) throw new ArgumentOutOfRangeException("Indices must be real positive.");

                if (i > RowCount)
                {
                    // dynamically add i-Rows new rows...
                    for (var k = 0; k < i - RowCount; k++)
                    {
                        Values.Add(new ArrayList(ColumnCount));

                        // ...with Cols columns
                        for (var t = 0; t < ColumnCount; t++) ((ArrayList) Values[RowCount + k]).Add(Complex.Zero);
                    }

                    RowCount = i; // ha!
                }

                if (j > ColumnCount)
                {
                    // dynamically add j-Cols columns to each row
                    for (var k = 0; k < RowCount; k++) for (var t = 0; t < j - ColumnCount; t++) ((ArrayList) Values[k]).Add(Complex.Zero);

                    ColumnCount = j;
                }

                ((ArrayList) Values[i - 1])[j - 1] = value;
                //this.Values[i - 1, j - 1] = value; 
            }
        }

        /// <summary>
        /// Access to the i-th component of an n by one matrix (column vector) or one by n matrix (row vector).
        /// </summary>
        /// <param name="i">One-based index.</param>
        /// <returns></returns>
        public virtual Complex this[int i]
        {
            get
            {
                //Complex buf;

                //if (this.rowCount == 1)
                //    try
                //    {
                //        buf = (Complex)(((ArrayList)Values[0])[i - 1]);
                //    }
                //    catch
                //    {
                //        buf = new Complex((double)((int)(((ArrayList)Values[0])[i - 1])));
                //    }
                //else
                //    try
                //    {
                //        buf = (Complex)(((ArrayList)Values[i - 1])[0]);
                //    }
                //    catch
                //    {
                //        buf = new Complex((double)((int)(((ArrayList)Values[i - 1])[0])));
                //    }

                //return buf;

                if (RowCount == 1) // row vector
                    return (Complex) (((ArrayList) Values[0])[i - 1]);
                else if (ColumnCount == 1) // coumn vector
                    return (Complex) (((ArrayList) Values[i - 1])[0]);
                else // neither
                    throw new InvalidOperationException("General matrix acces requires double indexing.");
            }
            set
            {
                if (RowCount == 1)
                {
                    // row vector

                    // dynamically extend vector if necessary
                    if (i > ColumnCount)
                    {
                        // dynamically add j-Cols columns to each row
                        for (var t = 0; t < i - ColumnCount; t++) ((ArrayList) Values[0]).Add(Complex.Zero);

                        ColumnCount = i;
                    }

                    ((ArrayList) Values[0])[i - 1] = value;
                }
                else if (ColumnCount == 1)
                {
                    // column vector

                    if (i > RowCount)
                    {
                        // dynamically add i-Rows new rows...
                        for (var k = 0; k < i - RowCount; k++)
                        {
                            Values.Add(new ArrayList(ColumnCount));

                            // ...with one column each
                            ((ArrayList) Values[RowCount + k]).Add(Complex.Zero);
                        }

                        RowCount = i; // ha!
                    }

                    ((ArrayList) Values[i - 1])[0] = value;
                }
                else throw new InvalidOperationException("Cannot access multidimensional matrix via single index.");
            }
        }

        #endregion

        #region Structs & Enums

        public enum DefinitenessType
        {
            PositiveDefinite,
            PositiveSemidefinite,
            NegativeDefinite,
            NegativeSemidefinite,
            Indefinite
        }

        #endregion
    }
}