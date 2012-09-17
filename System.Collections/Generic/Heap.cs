namespace System.Collections.Generic
{
    using Visitors;
    using Properties;

    public enum HeapType
    {
        /// <summary>
        ///   Makes the heap a Min-Heap - the smallest item is kept in the root of the heap.
        /// </summary>
        MinHeap,

        /// <summary>
        ///   Makes the heap a Max-Heap - the largest item is kept in the root of the heap.
        /// </summary>
        MaxHeap
    }

    /// <summary>
    ///   An implementation of a Heap data structure.
    /// </summary>
    /// <typeparam name="T"> The type of item stored in the heap. </typeparam>
    public sealed class Heap<T> : IVisitableCollection<T>, IHeap<T>
    {
        /// <summary>
        ///   Gets the type of heap represented by this instance.
        /// </summary>
        /// <value> The type of heap. </value>
        public HeapType Type { get; private set; }

        /// <summary>
        ///   Gets the underlying list.
        /// </summary>
        /// <value> The underlying list. </value>
        internal VisitableList<T> List { get; private set; }

        public IComparer<T> Comparer { get; private set; }

        #region Construction
        /// <summary>
        ///   Initializes a new instance of the <see cref="Heap&lt;T&gt;" /> class.
        /// </summary>
        public Heap(HeapType type)
            : this(type, Comparer<T>.Default)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Heap&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="type"> The type of heap. </param>
        /// <param name="capacity"> The capacity. </param>
        public Heap(HeapType type, int capacity)
            : this(type, capacity, Comparer<T>.Default)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Heap&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="type"> The type of heap. </param>
        /// <param name="comparer"> The comparer to use. </param>
        public Heap(HeapType type, IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            Type = type;
            List = new VisitableList<T> { default(T) };
            Comparer = (type == HeapType.MinHeap) ? comparer : new InvertedComparer<T>(comparer);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Heap&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="type"> The type of heap. </param>
        /// <param name="capacity"> The initial capacity of the Heap. </param>
        /// <param name="comparer"> The comparer to use. </param>
        public Heap(HeapType type, int capacity, IComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            Type = type;
            List = new VisitableList<T>(capacity) { default(T) };
            Comparer = (type == HeapType.MinHeap) ? comparer : new InvertedComparer<T>(comparer);
        }
        #endregion

        #region Public Members
        /// <summary>
        ///   Gets the smallest item in the heap (located at the root).
        /// </summary>
        /// <returns> The value of the root of the Heap. </returns>
        public T Root
        {
            get
            {
                if (Count == 0) throw new InvalidOperationException(Resources.HeapIsEmpty);
                return List[1];
            }
        }

        /// <summary>
        ///   Removes the smallest item in the heap (located at the root).
        /// </summary>
        /// <returns> The value contained in the root of the Heap. </returns>
        public T RemoveRoot()
        {
            if (Count == 0)
                throw new InvalidOperationException(Resources.HeapIsEmpty);
            // The minimum item to return.
            var min = List[1];
            // The last item in the heap
            var last = List[Count];
            List.RemoveAt(Count);
            // If there's still items left in this heap, reheapify it.
            if (Count > 0)
            {
                // Re-heapify the binary tree to conform to the heap property 
                var counter = 1;
                while ((counter * 2) < (List.Count))
                {
                    var child = counter * 2;
                    if (((child + 1) < (List.Count)) &&
                        (Comparer.Compare(List[child + 1], List[child]) < 0))
                        child++;
                    if (Comparer.Compare(last, List[child]) <= 0)
                        break;
                    List[counter] = List[child];
                    counter = child;
                }
                List[counter] = last;
            }
            return min;
        }
        #endregion

        #region IVisitableCollection<T> Members
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

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(T item)
        {
            return List.Contains(item);
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
                throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            for (var i = 1; i < List.Count; i++)
                array[arrayIndex++] = List[i];
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.ICollection"></see> . </returns>
        public int Count
        {
            get { return List.Count - 1; }
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
            // Add a dummy to the end of the list (it will be replaced)
            List.Add(default(T));
            var counter = List.Count - 1;
            while ((counter > 1) && (Comparer.Compare(List[counter / 2], item) > 0))
            {
                List[counter] = List[counter / 2];
                counter = counter / 2;
            }
            List[counter] = item;
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            for (var i = 1; i < List.Count; i++)
            {
                if (visitor.HasCompleted)
                    break;
                visitor.Visit(List[i]);
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
            for (var i = 1; i < List.Count; i++)
                yield return List[i];
        }

        /// <summary>
        ///   Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            List.RemoveRange(1, List.Count - 1); // Clears all objects in this instance except the first dummy one.
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
            if (GetType() == obj.GetType())
            {
                var heap = obj as Heap<T>;
                return Count.CompareTo(heap.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
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
        /// <returns> An enumerator for enumerating though the colleciton. </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

    /*
    /// <summary>
    /// Binary Heap
    /// 
    /// The heap tree is “threaded”: the tree nodes are stored in an array list provided in the constructor.
    /// For each element at index i: its left child is at index 2i+1, its right child is at 2i+2 and
    /// its parent is at (i−1)/2.
    /// These relations are respectively implemented in RightChild, LeftChild and Parent methods.
    /// If the provided list is not empty, it will be “heapified”. This is done using the Heapify method
    /// and is similar to STL’s make_heap operation in C++.
    /// New elements can be inserted to the heap using the Insert method.
    /// A new element will be first put at the end of the tree, then pulled up until the tree satisfies
    /// the heap property again. This operation is implemented in the HeapUp method.
    /// The root element of the heap can be removed from the tree using the RemoveRoot method.
    /// The last element of the tree is then put at the root and pushed down until the tree satisfies the heap invariant.
    /// This is implemented using the HeapDown method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, ComVisible(false), DebuggerDisplay("Count = {Count}")]
    public class Heap<T> : IHeap<T>, IVisitableCollection<T> //where T : IComparable<T>
    {
        #region Fields
        private List<T> _list;
        internal IList<T> List { get { return _list; } }

        private IComparer<T> _comparer;

        private HeapType _heapType;
        public HeapType Type { get { return _heapType; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="type">The type of heap.</param>
        /// <param name="capacity">The initial capacity of the Heap.</param>
        /// <param name="comparer">The comparer to use.</param>
        public Heap(HeapType type, int capacity, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            _heapType = type;

            _list = new VisitableList<T>(capacity);
            _list.Add(default(T));  // Add a dummy item so our indexing starts at 1

            _comparer = (type == HeapType.MinHeap) ? comparer : new InvertedComparer<T>(comparer);
        }

        public Heap(HeapType type, IEnumerable<T> collection, IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _list = (collection == null) ? new List<T>() : new List<T>(collection);
            _heapType = type;
            switch (type)
            {
            case HeapType.MaxHeap:
                _comparer = comparer;
                break;
            case HeapType.MinHeap:
                _comparer = new InvertedComparer<T>(comparer);
                break;
            }

            if (_list.Count > 1)
                Heapify();
        }
        public Heap(IEnumerable<T> collection, IComparer<T> comparer)
            : this(HeapType.MaxHeap, collection, comparer)
        { }
        public Heap(IEnumerable<T> collection)
            : this(collection, Comparer<T>.Default)
        { }
        public Heap(IComparer<T> comparer)
            : this(null, comparer)
        { }
        public Heap()
            : this(Comparer<T>.Default)
        { }
        #endregion

        /// <summary>
        /// Inserts an item onto the heap.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _list.Add(item);
            HeapUp(Count - 1);
        }
        /// <summary>
        /// Removes and returns the root item from the collection.
        /// </summary>
        /// <returns>Returns the item at the root of the heap.</returns>
        public T RemoveRoot()
        {
            if (Count == 0)
                throw new InvalidOperationException("Empty heap.");
            T root = _list[0]; // Get the first item
            int lastIndex = Count - 1;
            _list[0] = _list[lastIndex];
            _list.RemoveAt(lastIndex);
            if (Count > 1)
                Sink(0);
            return root;
        }

        /// <summary>
        /// Return the root item from the collection, without removing it.
        /// </summary>
        /// <returns>Returns the item at the root of the heap.</returns>
        public T Root
        {
            get
            {
                if (Count == 0)
                    throw new InvalidOperationException("Empty heap.");
                return _list[0];
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }
        /// <summary>
        /// Sets the capacity to the actual number of elements in the BinaryHeap,
        /// if that number is less than a threshold value.
        /// </summary>
        /// <remarks>
        /// The current threshold value is 90% (.NET 3.5), but might change in a future release.
        /// </remarks>
        public void TrimExcess()
        {
            _list.TrimExcess();
        }

        public void AdjustRoot()
        {
            if (Count > 1)
                Sink(0);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public T[] ToArray()
        {
            T[] result = _list.ToArray();
            // We want to return the elements in the same order in which they are enumerated, which is sorted order, so we simply sort.
            Array.Sort(result, _comparer);
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
            // We want to return the elements in the same order in which they are enumerated, which is sorted order, so we simply sort.
            Array.Sort(array, arrayIndex, _list.Count, _comparer);
        }

        public IEnumerator<T> GetEnumerator()
        {
            // We want to enumerate in the order you would get if calling Dequeue until the queue is empty.
            // A simple way to achieve that is to simple sort the heap, and to return an iterator over the sorted copy.
            List<T> listCopy = new List<T>(_list);
            listCopy.Sort(_comparer);
            return listCopy.GetEnumerator();
        }

        #region Helpers
        // Shift index item up towards the root
        private void HeapUp(int index)
        {
            if (index >= Count) return;
            T item = _list[index];
            bool swapped = false;
            while (index > 0)
            {
                int parent = Parent(index);
                //if (parent < 0) break;
                if (_comparer.Compare(_list[parent], item) > 0)
                    break;
                _list[index] = _list[parent];
                swapped = true;
                index = parent;
            }
            if (swapped)
                _list[index] = item;
        }

        private void Heapify()
        {
            for (int root = (Count >> 1) - 1; root >= 0; --root)
            {
                Sink(root);
            }
        }

        // Shift large values towards the root of the tree and small values toward the leaves
        private void Sink(int parent) // HeapDown
        {
            if (parent >= (Count >> 1)) return;
            // --------------------------------------
            
            //T item = _list[parent];
            //while (parent < (Count >> 1))
            //{
            //    int lChild = LeftChild(parent);
            //    if (lChild >= Count)
            //        break;
            //    int rChild = lChild + 1;
            //    int xChild = (rChild < Count && _comparer.Compare(_list[rChild], _list[lChild]) > 0) ?
            //                    rChild :
            //                    lChild;
            //    if (_comparer.Compare(item, _list[xChild]) > 0)
            //        break;
            //    _list[parent] = _list[xChild]; // Swap
            //    parent = xChild;
            //}
            //_list[parent] = item;
            
            // --------------------------------------
            int lChild = LeftChild(parent);
            if (lChild >= Count)
                return;
            int rChild = lChild + 1;
            int xChild = (rChild < Count && _comparer.Compare(_list[rChild], _list[lChild]) > 0) ?
                            rChild :
                            lChild;
            T item = _list[parent];
            if (_comparer.Compare(item, _list[xChild]) > 0)
                return;
            _list[parent] = _list[xChild];
            _list[xChild] = item;
            Sink(xChild);
            // --------------------------------------
        }

        private int Parent(int index) { return (index - 1) >> 1; }
        private int LeftChild(int parent) { return (parent << 1) + 1; }
        private int RightChild(int parent) { return (parent << 1) + 2; }
        
        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }

        #endregion

        #region ICollection Members

        private ICollection Collection
        {
            get { return _list as ICollection; }
        }

        /// <summary>
        /// Get a count of the number of items in the collection.
        /// </summary>
        public int Count { get { return _list.Count; } }

        public void CopyTo(Array array, int index)
        {
            Collection.CopyTo(array, index);
        }

        public object SyncRoot { get { return Collection.SyncRoot; } }

        public bool IsSynchronized { get { return Collection.IsSynchronized; } }

        #endregion

        #region IVisitableCollection<T> Members

        public bool IsFixedSize { get { return false; } }

        public bool IsEmpty { get { return (Count == 0); } }

        public bool IsFull { get { return false; } }

        #endregion

        #region ICollection<T> Members

        public bool IsReadOnly { get { return false; } }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (GetType() == obj.GetType())
            {
                Heap<T> heap = obj as Heap<T>;
                return Count.CompareTo(heap.Count);
            }
            else
            {
                return GetType().FullName.CompareTo(obj.GetType().FullName);
            }
        }

        #endregion

        #region IVisitable<T> Members

        public void Accept(Visitors.IVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");

            foreach (T item in _list)
            {
                if (visitor.HasCompleted)
                    break;
                visitor.Visit(item);
            }
        }

        #endregion
    }
    */
}