namespace System.Collections.Generic
{
    using Visitors;
    using Sorter;
    using Properties;

    /// <summary>
    ///   A general tree data structure that can hold any amount of nodes.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class GeneralTree<T> : IVisitableCollection<T>, ITree<T>, ISortable<GeneralTree<T>>
    {
        /// <summary>
        ///   Gets the parent of this node.
        /// </summary>
        /// <value> The parent of this node. </value>
        public GeneralTree<T> Parent { get; internal set; }

        /// <summary>
        ///   Gets the data.
        /// </summary>
        /// <value> The data. </value>
        public T Data { get; private set; }

        /// <summary>
        ///   Gets the child nodes of this node.
        /// </summary>
        /// <value> The child nodes. </value>
        public VisitableList<GeneralTree<T>> ChildNodes { get; private set; }


        #region Construction
        /// <summary>
        ///   Initializes a new instance of the <see cref="GeneralTree&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="data"> The data held in this tree. </param>
        public GeneralTree(T data)
        {
            ChildNodes = new VisitableList<GeneralTree<T>>();
            Data = data;
        }
        #endregion

        #region IVisitableCollection<T>  Members
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
                        throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
                    array[arrayIndex++] = enumerator.Current;
                }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            var stack = new VisitableStack<GeneralTree<T>>();
            stack.Push(this);
            while (!stack.IsEmpty)
            {
                var tree = stack.Pop();
                if (tree != null)
                {
                    yield return tree.Data;
                    for (var i = 0; i < tree.Degree; i++)
                        stack.Push(tree.GetChild(i));
                }
            }
        }
        #endregion

        #region VisitableCollection<T> Members
        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.ICollection"></see> . </returns>
        public int Count
        {
            get { return ChildNodes.Count; }
        }

        /// <summary>
        ///   Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            ChildNodes.Clear();
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
                var t = obj as GeneralTree<T>;
                return Count.CompareTo(t.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
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
            var child = new GeneralTree<T>(item);
            ChildNodes.Add(child);
            child.Parent = this;
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
            for (var i = 0; i < ChildNodes.Count; i++)
                if (ChildNodes[i].Data.Equals(item))
                {
                    ChildNodes[i].Parent = null;
                    ChildNodes.RemoveAt(i);
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
        #endregion

        #region ITree<T> Members
        /// <summary>
        ///   Adds the specified child to the tree.
        /// </summary>
        /// <param name="child"> The child to add.. </param>
        void ITree<T>.Add(ITree<T> child)
        {
            Add((GeneralTree<T>) child);
        }

        /// <summary>
        ///   Gets the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        /// <returns> The child node at the specified index. </returns>
        ITree<T> ITree<T>.GetChild(int index)
        {
            return GetChild(index);
        }

        /// <summary>
        ///   Removes the specified child.
        /// </summary>
        /// <param name="child"> The child. </param>
        /// <returns> A value indicating whether the child was found in this tree. </returns>
        bool ITree<T>.Remove(ITree<T> child)
        {
            return Remove((GeneralTree<T>) child);
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
        ///   Retrieves the Ancestors of this node, in the same order
        ///   as the path from the current node to the root.
        /// </summary>
        /// <value> The ancestors. </value>
        public GeneralTree<T>[] Ancestors
        {
            get
            {
                var list = new List<GeneralTree<T>>();
                var currentNode = this;
                while (currentNode.Parent != null)
                {
                    list.Add(currentNode.Parent);
                    currentNode = currentNode.Parent;
                }
                return list.ToArray();
            }
        }


        /// <summary>
        ///   Gets the degree (number of childnodes).
        /// </summary>
        /// <value> The degree. </value>
        public int Degree
        {
            get { return ChildNodes.Count; }
        }

        /// <summary>
        ///   Finds the node with the specified condition.  If a node is not found matching
        ///   the specified condition, null is returned.
        /// </summary>
        /// <param name="condition"> The condition to test. </param>
        /// <returns> The first node that matches the condition supplied. If a node is not found, null is returned. </returns>
        public GeneralTree<T> FindNode(Predicate<T> condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            if (condition.Invoke(Data)) return this;

            for (var i = 0; i < Degree; i++)
            {
                var ret = ChildNodes[i].FindNode(condition);
                if (ret != null)
                    return ret;
            }
            return null;
        }

        /// <summary>
        ///   Gets the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        /// <returns> The child at the specified index. </returns>
        public GeneralTree<T> GetChild(int index)
        {
            CheckValidIndex(index);
            return ChildNodes[index];
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
            orderedVisitor.VisitPreOrder(Data);
            for (var i = 0; i < Degree; i++)
                if (GetChild(i) != null)
                    GetChild(i).DepthFirstTraversal(orderedVisitor);
            orderedVisitor.VisitPostOrder(Data);
        }

        /// <summary>
        ///   Performs a breadth first traversal on this tree with the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void BreadthFirstTraversal(IVisitor<T> visitor)
        {
            var q = new VisitableQueue<GeneralTree<T>>();
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
        public virtual bool IsLeafNode
        {
            get { return Degree == 0; }
        }

        /// <summary>
        ///   Adds the child tree to this node.
        /// </summary>
        /// <param name="child"> The child tree to add. </param>
        public void Add(GeneralTree<T> child)
        {
            if (child.Parent != null)
                child.Parent.Remove(child);
            if (!ChildNodes.Contains(child))
            {
                ChildNodes.Add(child);
                child.Parent = this;
            }
        }

        /// <summary>
        ///   Removes the specified child node from the tree.
        /// </summary>
        /// <param name="child"> The child tree to remove. </param>
        /// <returns> A value indicating whether the child was found (and removed) from this tree. </returns>
        public bool Remove(GeneralTree<T> child)
        {
            if (ChildNodes.Remove(child))
            {
                child.Parent = null;
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Removes the child at the specified index.
        /// </summary>
        /// <param name="index"> The index. </param>
        public void RemoveAt(int index)
        {
            if ((index > ChildNodes.Count - 1) || (index < 0))
                throw new ArgumentOutOfRangeException();
            ChildNodes[index].Parent = null;
            ChildNodes.RemoveAt(index);
        }

        /// <summary>
        ///   Sorts all descendants (All nodes lower in the tree) recursively using the specified sorter.
        /// </summary>
        /// <param name="sorter"> The sorter to use in the sorting process. </param>
        public void SortAllDescendants(ISorter<GeneralTree<T>> sorter)
        {
            ChildNodes.Sort(sorter);
            foreach (var tree in ChildNodes)
                tree.SortAllDescendants(sorter);
        }

        /// <summary>
        ///   Sorts all descendants using the specified sorter.
        /// </summary>
        /// <param name="sorter"> The sorter to use in the sorting process. </param>
        /// <param name="comparison"> The comparison. </param>
        public void SortAllDescendants(ISorter<GeneralTree<T>> sorter, Comparison<GeneralTree<T>> comparison)
        {
            ChildNodes.Sort(sorter, comparison);
            foreach (var tree in ChildNodes)
                tree.SortAllDescendants(sorter, comparison);
        }

        /// <summary>
        ///   Sorts all descendants using the specified sorter.
        /// </summary>
        /// <param name="sorter"> The sorter to use in the sorting process. </param>
        /// <param name="comparer"> The comparer. </param>
        public void SortAllDescendants(ISorter<GeneralTree<T>> sorter, IComparer<GeneralTree<T>> comparer)
        {
            ChildNodes.Sort(sorter, comparer);
            foreach (var tree in ChildNodes)
                tree.SortAllDescendants(sorter, comparer);
        }

        #endregion

        #region Private Members
        /// <summary>
        ///   Checks if the specified index is valid.
        /// </summary>
        /// <param name="index"> The index to check. </param>
        void CheckValidIndex(int index)
        {
            if ((index < 0) || (index >= ChildNodes.Count))
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        ///   Finds the maximum height between the childnodes.
        /// </summary>
        /// <returns> The maximum height of all paths between this node and all leaf nodes. </returns>
        int FindMaximumChildHeight()
        {
            var max = 0;
            for (var i = 0; i < Degree; i++)
            {
                var childHeight = GetChild(i).Height;
                if (childHeight > max)
                    max = childHeight;
            }
            return max;
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        ///   Gets the <see cref="GeneralTree&lt;T&gt;" /> with the specified i.
        /// </summary>
        /// <value> </value>
        public GeneralTree<T> this[int i]
        {
            get
            {
                CheckValidIndex(i);
                return GetChild(i);
            }
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ISortable<GeneralTree<T>> Members
        /// <summary>
        ///   Sorts the direct children using the specified sorter.
        /// </summary>
        /// <param name="sorter"> The sorter to use in the sorting process. </param>
        void ISortable<GeneralTree<T>>.Sort(ISorter<GeneralTree<T>> sorter)
        {
            ChildNodes.Sort(sorter);
        }

        /// <summary>
        ///   Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter"> The sorter to use in the sorting process. </param>
        /// <param name="comparison"> The comparison. </param>
        public void Sort(ISorter<GeneralTree<T>> sorter, Comparison<GeneralTree<T>> comparison)
        {
            ChildNodes.Sort(sorter, comparison);
        }

        /// <summary>
        ///   Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter"> The sorter to use in the sorting process. </param>
        /// <param name="comparer"> The comparer. </param>
        public void Sort(ISorter<GeneralTree<T>> sorter, IComparer<GeneralTree<T>> comparer)
        {
            ChildNodes.Sort(sorter, comparer);
        }
        #endregion
    }
}