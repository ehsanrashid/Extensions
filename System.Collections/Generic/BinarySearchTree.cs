namespace System.Collections.Generic
{
    using Visitors;
    using Diagnostics;
    using ObjectModel;
    using Properties;

    /// <summary>
    ///   An implementation of a Binary Search Tree data structure.
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    public class BinarySearchTree<TKey, TValue> : IVisitableDictionary<TKey, TValue>
    {
        BinaryTree<Association<TKey, TValue>> tree;

        /// <summary>
        ///   Gets the comparer.
        /// </summary>
        /// <value> The comparer. </value>
        public IComparer<TKey> Comparer { get; private set; }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </returns>
        public int Count { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinarySearchTree&lt;TKey,TValue&gt;" /> class.
        /// </summary>
        /// <param name="comparer"> The comparer to use when comparing items. </param>
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            Comparer = comparer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BinarySearchTree&lt;TKey,TValue&gt;" /> class.
        /// </summary>
        public BinarySearchTree()
        {
            Comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        ///   Gets the largest item contained in the search tree.
        /// </summary>
        /// <value> The largest item. </value>
        public KeyValuePair<TKey, TValue> Max
        {
            get
            {
                if (Count == 0) throw new InvalidOperationException(Resources.SearchTreeIsEmpty);
                BinaryTree<Association<TKey, TValue>> parent;
                return FindMaxNode(tree, out parent).Data.ToKeyValuePair();
            }
        }

        /// <summary>
        ///   Gets the smallest item contained in the search tree.
        /// </summary>
        /// <value> The item element. </value>
        public KeyValuePair<TKey, TValue> Min
        {
            get
            {
                if (Count == 0) throw new InvalidOperationException(Resources.SearchTreeIsEmpty);
                BinaryTree<Association<TKey, TValue>> parent;
                return FindMinNode(tree, out parent).Data.ToKeyValuePair();
            }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetSortedEnumerator()
        {
            if (tree != null)
            {
                var trackingVisitor = new TrackingVisitor<Association<TKey, TValue>>();
                var inOrderVisitor = new InOrderVisitor<Association<TKey, TValue>>(trackingVisitor);
                tree.DepthFirstTraversal(inOrderVisitor);
                List<Association<TKey, TValue>> trackingList = trackingVisitor.TrackingList;
                for (var i = 0; i < trackingList.Count; i++)
                    yield return trackingList[i].ToKeyValuePair();
            }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (tree != null)
            {
                var stack = new VisitableStack<BinaryTree<Association<TKey, TValue>>>();
                stack.Push(tree);
                while (!stack.IsEmpty)
                {
                    var t = stack.Pop();
                    yield return t.Data.ToKeyValuePair();
                    if (t.Left != null)
                        stack.Push(t.Left);
                    if (t.Right != null)
                        stack.Push(t.Right);
                }
            }
        }

        #region Private Members
        /// <summary>
        ///   Finds the node containing the specified data key.
        /// </summary>
        /// <param name="key"> The key to search for. </param>
        /// <returns> The node with the specified key if found. If the key is not in the tree, this method returns null. </returns>
        BinaryTree<Association<TKey, TValue>> FindNode(TKey key)
        {
            var currentNode = tree;
            while (currentNode != null)
            {
                var nodeResult = Comparer.Compare(key, currentNode.Data.Key);
                if (nodeResult == 0)
                    return currentNode;
                else if (nodeResult < 0)
                    currentNode = currentNode.Left;
                else
                    currentNode = currentNode.Right;
            }
            return null;
        }

        /// <summary>
        ///   Finds the node containing the specified data item.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="parent"> The parent node of the item found. </param>
        /// <returns> The node in the tree with the specified key if found, otherwise null. </returns>
        BinaryTree<Association<TKey, TValue>> FindNode(TKey key,
                                                       out BinaryTree<Association<TKey, TValue>> parent)
        {
            var currentNode = tree;
            parent = null;
            while (currentNode != null)
            {
                var nodeResult = Comparer.Compare(key, currentNode.Data.Key);
                if (nodeResult == 0)
                    return currentNode;
                else if (nodeResult < 0)
                {
                    parent = currentNode;
                    currentNode = currentNode.Left;
                }
                else
                {
                    parent = currentNode;
                    currentNode = currentNode.Right;
                }
            }
            return null;
        }

        /// <summary>
        ///   Finds the max node.
        /// </summary>
        /// <param name="startNode"> The start node. </param>
        /// <param name="parent"> The parent of the node found. </param>
        /// <returns> The maximum node underneath the node specified. If the node specified is a leaf node, it is returned. </returns>
        BinaryTree<Association<TKey, TValue>> FindMaxNode(BinaryTree<Association<TKey, TValue>> startNode,
                                                          out BinaryTree<Association<TKey, TValue>> parent)
        {
            #region Asserts
            Debug.Assert(startNode != null);
            #endregion

            var searchNode = startNode;
            parent = null;
            while (searchNode.Right != null)
            {
                parent = searchNode;
                searchNode = searchNode.Right;
            }
            return searchNode;
        }

        /// <summary>
        ///   Finds the min node.
        /// </summary>
        /// <param name="startNode"> The start node. </param>
        /// <param name="parent"> The parent of the node found. </param>
        /// <returns> The minimum node underneath the node specified. If the node specified is a leaf node, it is returned. </returns>
        BinaryTree<Association<TKey, TValue>> FindMinNode(BinaryTree<Association<TKey, TValue>> startNode,
                                                          out BinaryTree<Association<TKey, TValue>> parent)
        {
            #region Asserts
            Debug.Assert(startNode != null);
            #endregion

            var searchNode = startNode;
            parent = null;
            while (searchNode.Left != null)
            {
                parent = searchNode;
                searchNode = searchNode.Left;
            }
            return searchNode;
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
        /// <returns> A enumerator to enumerate through this collection. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IDictionary<TKey,TValue> Members
        /// <summary>
        ///   Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key"> The object to use as the key of the element to add. </param>
        /// <param name="value"> The object to use as the value of the element to add. </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.IDictionary`2"></see>
        ///   is read-only.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the
        ///   <see cref="T:System.Collections.Generic.IDictionary`2"></see>
        ///   .</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public void Add(TKey key, TValue value)
        {
            var item = new Association<TKey, TValue>(key, value);
            if (tree == null)
                tree = new BinaryTree<Association<TKey, TValue>>(item);
            else
            {
                var currentNode = tree;
                // Find the place to insert the item.
                //	- If the item is found, throw an exception - no duplicate items allowed.
                //  - If a leaf is reached, insert the item in the correct place.
                //  - Else, traverse the tree further
                while (true)
                {
                    var nodeResult = Comparer.Compare(item.Key, currentNode.Data.Key);
                    if (nodeResult == 0)
                        throw new ArgumentException(Resources.ItemAllreadyInTree);
                    else if (nodeResult < 0)
                        if (currentNode.Left == null)
                        {
                            currentNode.Left = new BinaryTree<Association<TKey, TValue>>(item);
                            break;
                        }
                        else
                            currentNode = currentNode.Left;
                    else if (currentNode.Right == null)
                    {
                        currentNode.Right = new BinaryTree<Association<TKey, TValue>>(item);
                        break;
                    }
                    else
                        currentNode = currentNode.Right;
                }
            }
            Count++;
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.
        /// </summary>
        /// <param name="key"> The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see> . </param>
        /// <returns> true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false. </returns>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return (FindNode(key) != null);
        }

        /// <summary>
        ///   Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see
        ///    cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see
        ///    cref="T:System.Collections.Generic.IDictionary`2"></see> . </returns>
        public ICollection<TKey> Keys
        {
            get
            {
                var list = new List<TKey>(Count);
                using (var enumerator = GetSortedEnumerator())
                    while (enumerator.MoveNext())
                        list.Add(enumerator.Current.Key);
                return new ReadOnlyCollection<TKey>(list);
            }
        }

        /// <summary>
        ///   Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key"> The key of the element to remove. </param>
        /// <returns> true if the element is successfully removed; otherwise, false. This method also returns false if key was not found in the original <see
        ///    cref="T:System.Collections.Generic.IDictionary`2"></see> . </returns>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.IDictionary`2"></see>
        ///   is read-only.</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool Remove(TKey key)
        {
            BinaryTree<Association<TKey, TValue>> parentNode = null;
            var currentNode = FindNode(key, out parentNode);
            // The node wasn't found in the tree.
            if (currentNode == null)
                return false;
            else
            {
                if (currentNode.Degree == 2)
                {
                    // Find the element with the largest key in the left sub-tree					
                    BinaryTree<Association<TKey, TValue>> searchParent;
                    var searchNode = FindMaxNode(currentNode.Left, out searchParent);
                    parentNode = searchParent == null ? currentNode : searchParent;
                    currentNode.Data = searchNode.Data;
                    currentNode = searchNode;
                }
                var leftOverChild = currentNode.Left == null ? currentNode.Right : currentNode.Left;
                if (currentNode == tree)
                    tree = leftOverChild;
                else if (currentNode == parentNode.Left)
                    parentNode.Left = leftOverChild;
                else
                    parentNode.Right = leftOverChild;
                Count--;
                return true;
            }
        }

        /// <summary>
        ///   Tries to get a value based on the key.  No exceptions are thrown.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <returns> A value indicating whether the key was found in the Binary Search Tree. </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindNode(key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = node.Data.Value;
                return true;
            }
        }

        /// <summary>
        ///   Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see
        ///    cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see
        ///    cref="T:System.Collections.Generic.IDictionary`2"></see> . </returns>
        public ICollection<TValue> Values
        {
            get
            {
                var list = new List<TValue>(Count);
                using (var enumerator = GetSortedEnumerator())
                    while (enumerator.MoveNext())
                        list.Add(enumerator.Current.Value);
                return new ReadOnlyCollection<TValue>(list);
            }
        }

        /// <summary>
        ///   Gets or sets the value of the node with the specified key.
        /// </summary>
        /// <value> </value>
        public TValue this[TKey key]
        {
            get
            {
                var node = FindNode(key);
                if (node == null)
                    throw new ArgumentException(Resources.KeyDoesNotExist);
                return node.Data.Value;
            }
            set
            {
                var node = FindNode(key);
                if (node == null)
                    throw new ArgumentException(Resources.KeyDoesNotExist);
                node.Data.Value = value;
            }
        }
        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var node = FindNode(item.Key);
            if (node == null)
                return false;
            else
                return item.Value.Equals(node.Data.Value);
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
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            using (var enumerator = tree.GetEnumerator())
                while (enumerator.MoveNext())
                {
                    if (arrayIndex >= array.Length)
                        throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
                    array[arrayIndex++] = enumerator.Current.ToKeyValuePair();
                }
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
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }
        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IVisitable<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<KeyValuePair<TKey, TValue>> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            if (tree != null)
                using (var enumerator = tree.GetEnumerator())
                    while (enumerator.MoveNext())
                    {
                        visitor.Visit(enumerator.Current.ToKeyValuePair());
                        if (visitor.HasCompleted)
                            break;
                    }
        }
        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        public void Clear()
        {
            tree = null;
            Count = 0;
        }
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
                var t = obj as BinarySearchTree<TKey, TValue>;
                return Count.CompareTo(t.Count);
            }
            else
                return GetType().FullName.CompareTo(obj.GetType().FullName);
        }
        #endregion

        #region IVisitableCollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        ///   Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value> <c>true</c> if this instance is fixed size; otherwise, <c>false</c> . </value>
        public bool IsFixedSize
        {
            get { return false; }
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
            get { return false; }
        }
        #endregion
    }
}