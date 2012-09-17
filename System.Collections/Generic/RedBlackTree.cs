/*
 * The insertion and deletion code is based on the code found at http://eternallyconfuzzled.com/tuts/redblack.htm.
 * It's an excellent tutorial - if you want to understand Red Black trees, look there first.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Visitors;
using System.Diagnostics;

namespace System.Collections.Generic
{
    using Properties;
    
    /// <summary>
    /// An implementation of a Red-Black tree.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class RedBlackTree<TKey, TValue> : IVisitableDictionary<TKey, TValue> //, ITree<TValue>
    {
        #region Globals
        private readonly IComparer<TKey> comparerToUse;
        private int count;
        private RedBlackTreeNode<TKey, TValue> root;
        #endregion

        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public RedBlackTree()
        {
            comparerToUse = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public RedBlackTree(IComparer<TKey> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            comparerToUse = comparer;
        }
        #endregion

        /// <summary>
        /// Gets the comparer used in this instance.
        /// </summary>
        /// <value>The comparer.</value>
        public IComparer<TKey> Comparer
        {
            get { return comparerToUse; }
        }

        /// <summary>
        /// Gets the largest item contained in the search tree.
        /// </summary>
        /// <value>The largest item.</value>
        public TKey Max
        {
            get
            {
                if (count == 0)
                    throw new InvalidOperationException(Resources.SearchTreeIsEmpty);
                return FindMaxNode(root).Key;
            }
        }

        /// <summary>
        /// Gets the smallest item contained in the search tree.
        /// </summary>
        /// <value>The item element.</value>
        public TKey Min
        {
            get
            {
                if (count == 0)
                    throw new InvalidOperationException(Resources.SearchTreeIsEmpty);
                return FindMinNode(root).Key;
            }
        }

        #region IVisitableDictionary<TKey,TValue> Members
        /// <summary>
        /// Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value><c>true</c> if this collection is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return count == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
        public bool IsFull
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            root = null;
            count = 0;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator for enumerating though the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj.GetType() == GetType())
            {
                var rb = obj as RedBlackTree<TKey, TValue>;
                return Count.CompareTo(rb.Count);
            }
            else
                return GetType().FullName.CompareTo(obj.GetType().FullName);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public void Add(TKey key, TValue value)
        {
            root = InsertNode(root, key, value);
            root.Color = NodeColor.Black;
            count++;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return FindNode(key) != null;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TKey> Keys
        {
            get
            {
                // Get the keys in sorted order
                var visitor = new KeyTrackingVisitor<TKey, TValue>();
                var inOrderVisitor = new InOrderVisitor<KeyValuePair<TKey, TValue>>(visitor);
                DepthFirstTraversal(inOrderVisitor);
                return new ReadOnlyCollection<TKey>(visitor.TrackingList);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool Remove(TKey key)
        {
            if (root != null)
            {
                var startNode = new RedBlackTreeNode<TKey, TValue>(default(TKey), default(TValue));
                RedBlackTreeNode<TKey, TValue> childNode = startNode;
                startNode.Right = root;
                RedBlackTreeNode<TKey, TValue> parent = null;
                RedBlackTreeNode<TKey, TValue> grandParent = null;
                RedBlackTreeNode<TKey, TValue> foundNode = null;
                bool direction = true;
                while (childNode[direction] != null)
                {
                    bool lastDirection = direction;
                    grandParent = parent;
                    parent = childNode;
                    childNode = childNode[direction];
                    int comparisonValue = comparerToUse.Compare(childNode.Key, key);
                    if (comparisonValue == 0)
                        foundNode = childNode;
                    direction = comparisonValue < 0;
                    if ((RedBlackTreeNode<TKey, TValue>.IsBlack(childNode)) &&
                        (RedBlackTreeNode<TKey, TValue>.IsBlack(childNode[direction])))
                        if (RedBlackTreeNode<TKey, TValue>.IsRed(childNode[!direction]))
                            parent = parent[lastDirection] = SingleRotation(childNode, direction);
                        else if (RedBlackTreeNode<TKey, TValue>.IsBlack(childNode[direction]))
                        {
                            RedBlackTreeNode<TKey, TValue> sibling = parent[!lastDirection];
                            if (sibling != null)
                                if ((RedBlackTreeNode<TKey, TValue>.IsBlack(sibling.Left)) &&
                                    (RedBlackTreeNode<TKey, TValue>.IsBlack(sibling.Right)))
                                {
                                    parent.Color = NodeColor.Black;
                                    sibling.Color = NodeColor.Red;
                                    childNode.Color = NodeColor.Red;
                                }
                                else
                                {
                                    bool parentDirection = grandParent.Right == parent;
                                    if (RedBlackTreeNode<TKey, TValue>.IsRed(sibling[lastDirection]))
                                        grandParent[parentDirection] = DoubleRotation(parent, lastDirection);
                                    else if (RedBlackTreeNode<TKey, TValue>.IsRed(sibling[!lastDirection]))
                                        grandParent[parentDirection] = SingleRotation(parent, lastDirection);
                                    childNode.Color = grandParent[parentDirection].Color = NodeColor.Red;
                                    grandParent[parentDirection].Left.Color = NodeColor.Black;
                                    grandParent[parentDirection].Right.Color = NodeColor.Black;
                                }
                        }
                }
                if (foundNode != null)
                {
                    foundNode.Key = childNode.Key;
                    parent[parent.Right == childNode] = childNode[childNode.Left == null];
                }
                root = startNode.Right;
                if (root != null)
                    root.Color = NodeColor.Black;
                if (foundNode != null)
                {
                    count--;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to the get the specified value.
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="value">The value of the item under the current key.</param>
        /// <returns>A value indicating whether the key was found in the tree.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            RedBlackTreeNode<TKey, TValue> node = FindNode(key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = node.Value;
                return true;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TValue> Values
        {
            get
            {
                var visitor = new ValueTrackingVisitor<TKey, TValue>();
                var inOrderVisitor = new InOrderVisitor<KeyValuePair<TKey, TValue>>(visitor);
                DepthFirstTraversal(inOrderVisitor);
                return new ReadOnlyCollection<TValue>(visitor.TrackingList);
            }
        }

        /// <summary>
        /// Gets or sets the value in the node with the specified key.
        /// </summary>
        /// <value></value>
        public TValue this[TKey key]
        {
            get
            {
                RedBlackTreeNode<TKey, TValue> node = FindNode(key);
                if (node == null)
                    throw new ArgumentException(Resources.KeyDoesNotExist);
                else
                    return node.Value;
            }
            set
            {
                RedBlackTreeNode<TKey, TValue> node = FindNode(key);
                if (node == null)
                    throw new ArgumentException(Resources.KeyDoesNotExist);
                else
                    node.Value = value;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            RedBlackTreeNode<TKey, TValue> node = FindNode(item.Key);
            if (node == null)
                return false;
            else
                return item.Value.Equals(node.Value);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if ((array.Length - arrayIndex) < count)
                throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            int counter = arrayIndex;
            using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = GetEnumerator())
                while (enumerator.MoveNext())
                {
                    array.SetValue(enumerator.Current, counter);
                    counter++;
                }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Finds the node with the specified key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>The node with the specified key, if found, null if not found.</returns>
        private RedBlackTreeNode<TKey, TValue> FindNode(TKey key)
        {
            if (root == null)
                return null;
            RedBlackTreeNode<TKey, TValue> currentNode = root;
            while (currentNode != null)
            {
                int nodeResult = comparerToUse.Compare(key, currentNode.Key);
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
        /// Finds the max node.
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <returns>The maximum node below this node.</returns>
        private RedBlackTreeNode<TKey, TValue> FindMaxNode(RedBlackTreeNode<TKey, TValue> startNode)
        {
            #region Asserts
            Debug.Assert(startNode != null);
            #endregion

            RedBlackTreeNode<TKey, TValue> searchNode = startNode;
            while (searchNode.Right != null)
                searchNode = searchNode.Right;
            return searchNode;
        }

        /// <summary>
        /// Finds the min node.
        /// </summary>
        /// <param name="startNode">The start node.</param>
        /// <returns>The mimimum node below this node.</returns>
        private RedBlackTreeNode<TKey, TValue> FindMinNode(RedBlackTreeNode<TKey, TValue> startNode)
        {
            #region Asserts
            Debug.Assert(startNode != null);
            #endregion

            RedBlackTreeNode<TKey, TValue> searchNode = startNode;
            while (searchNode.Left != null)
                searchNode = searchNode.Left;
            return searchNode;
        }

        /// <summary>
        /// A recursive implementation of insertion of a node into the tree.
        /// </summary>
        /// <param name="node">The start node.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The node created in the insertion.</returns>
        private RedBlackTreeNode<TKey, TValue> InsertNode(RedBlackTreeNode<TKey, TValue> node, TKey key, TValue value)
        {
            if (node == null)
                node = new RedBlackTreeNode<TKey, TValue>(key, value);
            else if (comparerToUse.Compare(key, node.Key) != 0)
            {
                bool direction = comparerToUse.Compare(node.Key, key) < 0;
                node[direction] = InsertNode(node[direction], key, value);
                if (RedBlackTreeNode<TKey, TValue>.IsRed(node[direction]))
                    if (RedBlackTreeNode<TKey, TValue>.IsRed(node[!direction]))
                    {
                        node.Color = NodeColor.Red;
                        node.Left.Color = NodeColor.Black;
                        node.Right.Color = NodeColor.Black;
                    }
                    else if (RedBlackTreeNode<TKey, TValue>.IsRed(node[direction][direction]))
                        node = SingleRotation(node, !direction);
                    else if (RedBlackTreeNode<TKey, TValue>.IsRed(node[direction][!direction]))
                        node = DoubleRotation(node, !direction);
            }
            return node;
        }

        /// <summary>
        /// Perform a single rotation on the node provided..
        /// </summary>
        /// <param name="node">The node on which to focus the rotation.</param>
        /// <param name="direction">The direction of the rotation.  If direction is equal to true, a right rotation is performed.  Other wise, a left rotation.</param>
        /// <returns>The new root of the cluster.</returns>
        private RedBlackTreeNode<TKey, TValue> SingleRotation(RedBlackTreeNode<TKey, TValue> node, bool direction)
        {
            RedBlackTreeNode<TKey, TValue> childSibling = node[!direction];
            node[!direction] = childSibling[direction];
            childSibling[direction] = node;
            node.Color = NodeColor.Red;
            childSibling.Color = NodeColor.Black;
            return childSibling;
        }

        /// <summary>
        /// Perform a double rotation on the node provided..
        /// </summary>
        /// <param name="node">The node on which to focus the rotation.</param>
        /// <param name="direction">The direction of the rotation.  If direction is equal to true, a right rotation is performed.  Other wise, a left rotation.</param>
        /// <returns>The new root of the cluster.</returns>
        private RedBlackTreeNode<TKey, TValue> DoubleRotation(RedBlackTreeNode<TKey, TValue> node, bool direction)
        {
            node[!direction] = SingleRotation(node[!direction], !direction);
            return SingleRotation(node, direction);
        }

        /// <summary>
        /// Visits the node in an inorder fashion.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="visitor">The visitor.</param>
        private void VisitNode(RedBlackTreeNode<TKey, TValue> node, OrderedVisitor<KeyValuePair<TKey, TValue>> visitor)
        {
            if (node != null)
            {
                var pair = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                visitor.VisitPreOrder(pair);
                VisitNode(node.Left, visitor);
                visitor.VisitInOrder(pair);
                VisitNode(node.Right, visitor);
                visitor.VisitPostOrder(pair);
            }
        }

        /// <summary>
        /// Performs a depth first traversal on the Red Black Tree.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void DepthFirstTraversal(OrderedVisitor<KeyValuePair<TKey, TValue>> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            VisitNode(root, visitor);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var stack = new VisitableStack<RedBlackTreeNode<TKey, TValue>>();
            stack.Push(root);
            while (!stack.IsEmpty)
            {
                RedBlackTreeNode<TKey, TValue> node = stack.Pop();
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                if (node.Left != null)
                    stack.Push(node.Left);
                if (node.Right != null)
                    stack.Push(node.Right);
            }
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor<KeyValuePair<TKey, TValue>> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            var stack = new VisitableStack<RedBlackTreeNode<TKey, TValue>>();
            stack.Push(root);
            while (!stack.IsEmpty)
                if (!visitor.HasCompleted)
                {
                    RedBlackTreeNode<TKey, TValue> node = stack.Pop();
                    visitor.Visit(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                    if (node.Left != null)
                        stack.Push(node.Left);
                    if (node.Right != null)
                        stack.Push(node.Right);
                }
        }
    }
}