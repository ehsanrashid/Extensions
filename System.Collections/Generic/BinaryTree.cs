namespace System.Collections.Generic
{
    using Visitors;
    using Properties;

    /// <summary>
    ///   An implementation of a Binary Tree data structure.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public sealed class BinaryTree<T> : IVisitableCollection<T>, ITree<T>
    {
        /// <summary>
        ///   Gets or sets the value contained in this node.
        /// </summary>
        /// <value> The value contained in this node. </value>
        public T Data { get; set; }

        /// <summary>
        ///   Gets or sets the left subtree.
        /// </summary>
        /// <value> The left subtree. </value>
        public BinaryTree<T> Left { get; set; }

        /// <summary>
        ///   Gets or sets the right subtree.
        /// </summary>
        /// <value> The right subtree. </value>
        public BinaryTree<T> Right { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinaryTree&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="data"> The data contained in this node. </param>
        public BinaryTree(T data)
            : this(data, null, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinaryTree&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="data"> The data. </param>
        /// <param name="left"> The data of the left subtree. </param>
        /// <param name="right"> The data of the right subtree. </param>
        public BinaryTree(T data, T left, T right)
            : this(data, new BinaryTree<T>(left), new BinaryTree<T>(right))
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinaryTree&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="data"> The data contained in this node. </param>
        /// <param name="left"> The left subtree. </param>
        /// <param name="right"> The right subtree. </param>
        public BinaryTree(T data, BinaryTree<T> left, BinaryTree<T> right)
        {
            Left = left;
            Right = right;
            Data = data;
        }

        #region IVisitableCollection<T> Members
        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            using (var enumerator = GetEnumerator())
                while (enumerator.MoveNext())
                {
                    visitor.Visit(enumerator.Current);
                    if (visitor.HasCompleted)
                        break;
                }
        }

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
            get { return Count == 0; }
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value> <c>true</c> if this collection is full; otherwise, <c>false</c> . </value>
        public bool IsFull
        {
            get { return (Left != null) && (Right != null); }
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(T item)
        {
            using (var enumerator = GetEnumerator())
                while (enumerator.MoveNext())
                    if (item.Equals(enumerator.Current))
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
            using (var enumerator = GetEnumerator())
                while (enumerator.MoveNext())
                {
                    if (arrayIndex >= array.Length) throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
                    array[arrayIndex++] = enumerator.Current;
                }
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.ICollection"></see> . </returns>
        public int Count
        {
            get
            {
                var count = 0;
                if (null != Left) count++;
                if (null != Right) count++;
                return count;
            }
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        public void Add(T item)
        {
            Add(new BinaryTree<T>(item));
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
        public bool Remove(T item)
        {
            if (Left != null)
                if (Left.Data.Equals(item))
                {
                    Left = null;
                    return true;
                }
            if (Right != null)
                if (Right.Data.Equals(item))
                {
                    Right = null;
                    return true;
                }
            return false;
        }

        /// <summary>
        ///   Removes the specified child.
        /// </summary>
        /// <param name="child"> The child. </param>
        /// <returns> A value indicating whether the child was found (and removed) from this tree. </returns>
        public bool Remove(BinaryTree<T> child)
        {
            if (Left != null)
                if (Left == child)
                {
                    RemoveLeft();
                    return true;
                }
            if (Right != null)
                if (Right == child)
                {
                    RemoveRight();
                    return true;
                }
            return false;
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            var stack = new VisitableStack<BinaryTree<T>>();
            stack.Push(this);
            while (!stack.IsEmpty)
            {
                var tree = stack.Pop();
                yield return tree.Data;
                if (tree.Left != null)
                    stack.Push(tree.Left);
                if (tree.Right != null)
                    stack.Push(tree.Right);
            }
        }

        /// <summary>
        ///   Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            Left = null;
            Right = null;
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
                var tree = obj as BinaryTree<T>;
                return Count.CompareTo(tree.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
        }
        #endregion

        #region ITree<T> Members
        /// <summary>
        ///   Adds the specified child to the tree.
        /// </summary>
        /// <param name="child"> The child to add.. </param>
        void ITree<T>.Add(ITree<T> child)
        {
            Add((BinaryTree<T>) child);
        }

        /// <summary>
        ///   Gets the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        /// <returns> The child at the specific index. </returns>
        ITree<T> ITree<T>.GetChild(int index)
        {
            return GetChild(index);
        }

        /// <summary>
        ///   Removes the specified child.
        /// </summary>
        /// <param name="child"> The child. </param>
        /// <returns> A value indicating whether the specified tree was found. </returns>
        bool ITree<T>.Remove(ITree<T> child)
        {
            return Remove((BinaryTree<T>) child);
        }

        /// <summary>
        ///   Finds the node for which the given predicate holds true.
        /// </summary>
        /// <param name="condition"> The condition to test on the data item. </param>
        /// <returns> The first node that matches the condition supplied. If a node is not found, null is returned. </returns>
        ITree<T> ITree<T>.FindNode(Predicate<T> condition)
        {
            return FindNode(condition);
        }
        #endregion

        #region Public Members
        /// <summary>
        ///   Finds the node with the specified condition.  If a node is not found matching
        ///   the specified condition, null is returned.
        /// </summary>
        /// <param name="condition"> The condition to test. </param>
        /// <returns> The first node that matches the condition supplied. If a node is not found, null is returned. </returns>
        public BinaryTree<T> FindNode(Predicate<T> condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (condition.Invoke(Data)) return this;

            if (Left != null)
            {
                var ret = Left.FindNode(condition);
                if (ret != null)
                    return ret;
            }
            if (Right != null)
            {
                var ret = Right.FindNode(condition);
                if (ret != null)
                    return ret;
            }
            return null;
        }

        /// <summary>
        ///   Gets the degree (number of childnodes) of this node.
        /// </summary>
        /// <value> The degree of this node. </value>
        public int Degree
        {
            get { return Count; }
        }

        /// <summary>
        ///   Gets the child at the specified index.
        /// </summary>
        /// <param name="index"> The index of the child in question. </param>
        /// <returns> The child at the specified index. </returns>
        public BinaryTree<T> GetChild(int index)
        {
            if (index == 0) return Left;
            if (index == 1) return Right;
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        ///   Gets the height of the this tree.
        /// </summary>
        /// <value> The height. </value>
        public int Height
        {
            get { return Degree == 0 ? 0 : 1 + FindMaximumChildHeight(); }
        }

        /// <summary>
        ///   Performs a depth first traversal on this tree with the specified visitor.
        /// </summary>
        /// <param name="orderedVisitor"> The ordered visitor. </param>
        public void DepthFirstTraversal(OrderedVisitor<T> orderedVisitor)
        {
            if (orderedVisitor.HasCompleted) return;

            // Preorder visit
            orderedVisitor.VisitPreOrder(Data);
            if (Left != null)
                Left.DepthFirstTraversal(orderedVisitor);
            // Inorder visit
            orderedVisitor.VisitInOrder(Data);
            if (Right != null)
                Right.DepthFirstTraversal(orderedVisitor);
            // PostOrder visit
            orderedVisitor.VisitPostOrder(Data);
        }

        /// <summary>
        ///   Performs a breadth first traversal on this tree with the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void BreadthFirstTraversal(IVisitor<T> visitor)
        {
            var q = new VisitableQueue<BinaryTree<T>>();
            q.Enqueue(this);
            while (!q.IsEmpty)
            {
                var t = q.Dequeue();
                visitor.Visit(t.Data);
                for (var i = 0; i < t.Degree; i++)
                {
                    var child = t.GetChild(i);
                    if (child != null)
                        q.Enqueue(child);
                }
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is leaf node.
        /// </summary>
        /// <value> <c>true</c> if this instance is leaf node; otherwise, <c>false</c> . </value>
        public bool IsLeafNode
        {
            get { return Degree == 0; }
        }

        /// <summary>
        ///   Removes the left child.
        /// </summary>
        public void RemoveLeft()
        {
            Left = null;
        }

        /// <summary>
        ///   Removes the left child.
        /// </summary>
        public void RemoveRight()
        {
            Right = null;
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="subtree"> The subtree. </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        public void Add(BinaryTree<T> subtree)
        {
            if (Left == null) Left = subtree;
            else if (Right == null) Right = subtree;
            else
                throw new InvalidOperationException(Resources.BinaryTreeIsFull);
        }
        #endregion

        #region Private Members
        /// <summary>
        ///   Finds the maximum height between the childnodes.
        /// </summary>
        /// <returns> The maximum height of the tree between all paths from this node and all leaf nodes. </returns>
        private int FindMaximumChildHeight()
        {
            var leftHeight = 0;
            var rightHeight = 0;
            if (Left != null)
                leftHeight = Left.Height;
            if (Right != null)
                rightHeight = Right.Height;
            return (leftHeight > rightHeight) ? leftHeight : rightHeight;
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        ///   Gets the <see cref="BinaryTree&lt;T&gt;" /> at the specified index.
        /// </summary>
        /// <value> </value>
        public BinaryTree<T> this[int i]
        {
            get { return GetChild(i); }
        }
        #endregion

        #region ICollection<T> Members
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
        /// <returns> A enumerator to enumerate though the collection. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}