namespace System.Collections.Generic
{
    using Linq;

    using Collections;

    public class Tree<T> : //ITree<T>,
        ICollection<T>, IComparable
    {
        #region Fields
        private T _Data;

        public T Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        private Tree<T> _Parent;

        public Tree<T> Parent
        {
            get { return _Parent; }
            internal set { _Parent = value; }
        }

        private readonly List<Tree<T>> _Children;

        public List<Tree<T>> Children
        {
            get { return _Children; }
            //set { _Children = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        ///   Initializes a new instance of the <see cref="Tree&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="data"> The data held in this tree. </param>
        public Tree(T data)
        {
            _Data = data;
            _Parent = default(Tree<T>);
            _Children = new List<Tree<T>>();
        }
        #endregion

        /// <summary>
        ///   Adds the child tree to this node.
        /// </summary>
        /// <param name="child"> The child tree to add. </param>
        public void Add(Tree<T> child)
        {
            if (child._Parent != default(Tree<T>)) child._Parent.Remove(child);
            if (_Children.Contains(child)) return;
            _Children.Add(child);
            child._Parent = this;
        }

        /// <summary>
        ///   Removes the specified child node from the tree.
        /// </summary>
        /// <param name="child"> The child tree to remove. </param>
        /// <returns> A value indicating whether the child was found (and removed) from this tree. </returns>
        public bool Remove(Tree<T> child)
        {
            var success = _Children.Remove(child);
            if (success) child._Parent = default(Tree<T>);
            return success;
        }

        /// <summary>
        ///   Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value> <c>true</c> if this collection is empty; otherwise, <c>false</c> . </value>
        public bool IsEmpty
        {
            get { return (Count == 0); }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is leaf node.
        /// </summary>
        /// <value> <c>true</c> if this instance is leaf node; otherwise, <c>false</c> . </value>
        public virtual bool IsLeafNode
        {
            get { return (Count == 0); }
        }

        /// <summary>
        ///   Retrieves the Ancestors of this node, in the same order
        ///   as the path from the current node to the root.
        /// </summary>
        /// <value> The ancestors. </value>
        public Tree<T>[] Ancestors
        {
            get
            {
                var list = new List<Tree<T>>();
                var currentNode = this;
                while (currentNode._Parent != default(Tree<T>))
                {
                    list.Add(currentNode._Parent);
                    currentNode = currentNode._Parent;
                }
                return list.ToArray();
            }
        }

        /// <summary>
        ///   Finds the node with the specified condition.  If a node is not found matching
        ///   the specified condition, null is returned.
        /// </summary>
        /// <param name="condition"> The condition to test. </param>
        /// <returns> The first node that matches the condition supplied. If a node is not found, null is returned. </returns>
        public Tree<T> FindNode(Predicate<T> condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (condition.Invoke(_Data))
                return this;

            //for (int i = 0; i < Degree; i++)
            //{
            //    Tree<T> ret = _Children[i].FindNode(condition);
            //    if (ret != null)
            //        return ret;
            //}
            // ------------------
            //foreach (var tree in _Children)
            //{
            //    var ret = tree.FindNode(condition);
            //    if (default(Tree<T>) != ret)
            //        return ret;
            //}
            //return null;
            // ------------------
            return Enumerable.Select(_Children, tree => tree.FindNode(condition)).FirstOrDefault(ret => default(Tree<T>) != ret);

        }

        /// <summary>
        ///   Gets the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        /// <returns> The child at the specified index. </returns>
        public Tree<T> this[int index]
        {
            get { return GetChild(index); }
        }

        /// <summary>
        ///   Gets the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        /// <returns> The child at the specified index. </returns>
        public Tree<T> GetChild(int index)
        {
            if (0 > index || index >= _Children.Count)
                throw new ArgumentOutOfRangeException();
            return _Children[index];
        }

        /// <summary>
        ///   Gets the height of the this tree.
        /// </summary>
        /// <value> The height. </value>
        public int Height
        {
            get
            {
                if (Count == 0) return 0;
                return FindMaximumChildHeight() + 1;
            }
        }

        /// <summary>
        ///   Finds the maximum height between the childnodes.
        /// </summary>
        /// <returns> The maximum height of all paths between this node and all leaf nodes. </returns>
        private int FindMaximumChildHeight()
        {
            var maxHeight = 0;
            //foreach (Tree<T> child in _Children)
            //    maxHeight = Math.Max(child.Height, maxHeight);
            maxHeight = _Children.Max((child) => child.Height);
            return maxHeight;
        }

        /// <summary>
        ///   Removes the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        public void RemoveAt(int index)
        {
            if (0 > index || index > _Children.Count - 1) throw new ArgumentOutOfRangeException();
            _Children[index]._Parent = default(Tree<T>);
            _Children.RemoveAt(index);
        }

        #region ICollection<T> Members
        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.ICollection"></see> . </returns>
        public int Count
        {
            get { return _Children.Count; }
        }

        /// <summary>
        ///   Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            _Children.Clear();
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
            var child = new Tree<T>(item);
            _Children.Add(child);
            child._Parent = this;
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
            for (var i = 0; i < Count; i++)
                if (_Children[i]._Data.Equals(item))
                {
                    _Children[i]._Parent = default(Tree<T>);
                    _Children.RemoveAt(i);
                    return true;
                }
            return false;
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
                    if (arrayIndex >= array.Length)
                        throw new ArgumentException("Not Enough Space In Target Array");
                    array[arrayIndex++] = enumerator.Current;
                }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var stack = new Stack<Tree<T>>();
            stack.Push(this);
            while (stack.Count != 0)
            {
                var tree = stack.Pop();
                if (tree != default(Tree<T>))
                {
                    yield return tree._Data;
                    //for (int i = 0; i < tree.Degree; i++)
                    //{
                    //    stack.Push(tree[i]);
                    //}
                    foreach (var child in tree._Children)
                        stack.Push(child);
                }
            }
        }

        #region IEnumerable<T> Members
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #endregion

        #region IComparable Members
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
                var t = obj as Tree<T>;
                return Count.CompareTo(t.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
        }
        #endregion

        ///// <summary>
        ///// Performs a depth first traversal on this tree with the specified visitor.
        ///// </summary>
        ///// <param name="orderedVisitor">The ordered visitor.</param>
        //public void DepthFirstTraversal(OrderedVisitor<T> orderedVisitor)
        //{
        //    if (orderedVisitor.HasCompleted)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        orderedVisitor.VisitPreOrder(Data);

        //        for (int i = 0; i < Degree; i++)
        //        {
        //            if (GetChild(i) != null)
        //            {
        //                GetChild(i).DepthFirstTraversal(orderedVisitor);
        //            }
        //        }

        //        orderedVisitor.VisitPostOrder(Data);
        //    }
        //}

        ///// <summary>
        ///// Performs a breadth first traversal on this tree with the specified visitor.
        ///// </summary>
        ///// <param name="visitor">The visitor.</param>
        //public void BreadthFirstTraversal(IVisitor<T> visitor)
        //{
        //    VisitableQueue<Tree<T>> q = new VisitableQueue<Tree<T>>();

        //    q.Enqueue(this);

        //    while (!q.IsEmpty)
        //    {
        //        Tree<T> t = q.Dequeue();
        //        visitor.Visit(t.Data);

        //        for (int i = 0; i < t.Degree; i++)
        //        {
        //            Tree<T> child = t.GetChild(i);

        //            if (child != null)
        //            {
        //                q.Enqueue(child);
        //            }
        //        }
        //    }
        //}
    }

    #region Tree
    //public class Tree<T> : Collection<Node<T>> where T : Node<T>
    //{
    //    private T _Parent;
    //    public T Parent
    //    {
    //        get { return _Parent; }
    //        set { _Parent = value; }
    //    }

    //    public Tree(Node<T> parent)
    //    {
    //        Parent = (T) parent;
    //    }

    //    public T Add(T Node)
    //    {
    //        base.Add(Node);
    //        Node.Parent = Parent;
    //        return Node;
    //    }

    //    new public T this[int index]
    //    {
    //        get { return (T) base[index]; }
    //    }

    //    public override String ToString()
    //    {
    //        return "Count = " + Count.ToString();
    //    }
    //};

    // ---------------------------------------------------------

    //public class Tree<T> : Collection<T> //where T : Node<T>
    //{
    //    public Tree(Node<T> parent)
    //    {
    //        _Parent = parent;
    //        //_children = new List<Tree<T>>();
    //    }

    //    private Node<T> _Parent;
    //    public Node<T> Parent
    //    {
    //        get { return _Parent; }
    //        set { _Parent = value; }
    //    }

    //    //private List<Tree<T>> _children;
    //    //public List<Tree<T>> Children
    //    //{
    //    //    get { return _children; }// Readonly
    //    //}

    //    //public Tree<T> this[int index]
    //    //{
    //    //    get { return _children[index]; }
    //    //}

    //    // Add a child tree node
    //    new public Node<T> Add(Node<T> child)
    //    {
    //        //base.Add(child);
    //        child.Parent = Parent;
    //        return child;
    //    }
    //}
    #endregion
}