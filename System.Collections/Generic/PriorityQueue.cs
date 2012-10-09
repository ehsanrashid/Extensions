namespace System.Collections.Generic
{
    using Visitors;
    using Properties;

    /// <summary>
    ///   Specifies the Priority Queue type (min or max).
    /// </summary>
    public enum PriorityQueueType
    {
        /// <summary>
        ///   Specify a Max Priority Queue.
        /// </summary>
        MinPriorityQueue = 0,

        /// <summary>
        ///   Specify a Min Priority Queue.
        /// </summary>
        MaxPriorityQueue = 1
    }

    /// <summary>
    /// An inplementation of a Priority Queue (can be min or max).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class PriorityQueue<T> : IVisitableCollection<T>, IQueue<T>
    {
        #region Globals

        readonly Heap<Association<int, T>> heap;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="queueType">Type of the queue.</param>
        public PriorityQueue(PriorityQueueType queueType)
        {
            if (queueType == PriorityQueueType.MaxPriorityQueue) heap = new Heap<Association<int, T>>(HeapType.MaxHeap, new AssociationKeyComparer<int, T>());
            else heap = new Heap<Association<int, T>>(HeapType.MinHeap, new AssociationKeyComparer<int, T>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="queueType">Type of the queue.</param>
        /// <param name="capacity">The initial capacity of the Priority Queue.</param>
        public PriorityQueue(PriorityQueueType queueType, int capacity)
        {
            if (queueType == PriorityQueueType.MaxPriorityQueue) heap = new Heap<Association<int, T>>(HeapType.MaxHeap, capacity, new AssociationKeyComparer<int, T>());
            else heap = new Heap<Association<int, T>>(HeapType.MaxHeap, capacity, new AssociationKeyComparer<int, T>());
        }

        #endregion

        #region IQueue<T> Members

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Dequeues the item from the head of the queue.
        /// </summary>
        /// <returns>The item at the head of the queue.</returns>
        public T Dequeue()
        {
            CheckListNotEmpty();
            return heap.RemoveRoot().Value;
        }

        /// <summary>
        /// Returns the first item in the queue without removing it from the queue.
        /// </summary>
        /// <returns>The item at the head of the queue.</returns>
        public T Peek()
        {
            CheckListNotEmpty();
            return heap.Root.Value;
        }

        #endregion

        #region IVisitableCollection<T> Members

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
        /// <value>
        /// 	<c>true</c> if this collection is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return Count == 0; }
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
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
        public int Count
        {
            get { return heap.Count; }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            IList<Association<int, T>> list = heap.List;
            for (var i = 1; i < heap.Count + 1; i++) if (list[i].Value.Equals(item)) return true;
            return false;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            if ((array.Length - arrayIndex) < Count) throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            IList<Association<int, T>> list = heap.List;
            for (var i = 1; i < list.Count; i++) array.SetValue(list[i].Value, arrayIndex++);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(T item)
        {
            heap.Add(new Association<int, T>(0, item));
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = heap.GetEnumerator();
            while (enumerator.MoveNext()) yield return enumerator.Current.Value;
        }

        /// <summary>
        /// Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            heap.Clear();
        }

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
            if (obj == null) throw new ArgumentNullException("obj");
            if (obj.GetType() == GetType())
            {
                var q = obj as PriorityQueue<T>;
                return Count.CompareTo(q.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
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
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator for enumerating through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <param name="priority">The priority of the item.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(T item, int priority)
        {
            heap.Add(new Association<int, T>(priority, item));
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor<T> visitor)
        {
            if (null == visitor) throw new ArgumentNullException("visitor");
            var list = heap.List;
            for (var i = 1; i < list.Count; i++)
            {
                visitor.Visit(list[i].Value);
                if (visitor.HasCompleted) break;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Association<int, T>> GetKeyEnumerator()
        {
            return heap.GetEnumerator();
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="priority">The priority.</param>
        public void Enqueue(T item, int priority)
        {
            Add(item, priority);
        }

        /// <summary>
        /// Increases the priority of all the items in the queue.
        /// </summary>
        /// <param name="by">The number that gets added to the priority of each item.</param>
        public void IncreaseItemPriority(int by)
        {
            var list = heap.List;
            for (var i = 1; i < list.Count; i++) list[i].Key += by;
        }

        /// <summary>
        /// Decreases the priority of all the items in the queue.
        /// </summary>
        /// <param name="by">The number that gets subtracted to the priority of each item.</param>
        public void DecreaseItemPriority(int by)
        {
            var list = heap.List;
            for (var i = 1; i < list.Count; i++) list[i].Key -= by;
        }

        /// <summary>
        /// Checks if the list is not empty, and if it is, throw an exception.
        /// </summary>
        void CheckListNotEmpty()
        {
            if (0 == heap.Count) throw new InvalidOperationException("The Priority Queue is empty.");
        }
    }

    /*
    /// <summary>
    ///   Priority Queue
    ///   In fact the heap is a priority queue. We just need a wrapper (delegator) object to create
    ///   the heap and the heap’s list. The code is self-explanatory.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable, ComVisible(false), DebuggerDisplay("Count = {Count}")]
    public class PriorityQueue<T> : IEnumerable<T>, ICollection where T : IComparable<T>
    {
        private readonly Heap<T> _heap;

        #region Constructors
        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer)
        {
            _heap = new Heap<T>(collection, comparer);
        }

        public PriorityQueue(IEnumerable<T> collection)
        {
            _heap = new Heap<T>(collection);
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            _heap = new Heap<T>(comparer);
        }

        public PriorityQueue()
        {
            _heap = new Heap<T>();
        }
        #endregion

        public void Enqueue(T item)
        {
            _heap.Add(item);
        }

        public T Dequeue()
        {
            return _heap.RemoveRoot();
        }

        public T Peek()
        {
            return _heap.Root;
        }

        public void Clear()
        {
            _heap.Clear();
        }

        public bool IsEmpty
        {
            get { return (Count == 0); }
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<T> enumerable = _heap;
            return enumerable.GetEnumerator();
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

        #region ICollection Members
        public int Count
        {
            get { return _heap.Count; }
        }

        public void CopyTo(Array array, int index)
        {
            _heap.CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return _heap.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _heap.SyncRoot; }
        }
        #endregion
    }
    */

    #region Old

    /*
    /// <summary>
    /// Provides a queue where the element with the lowest value is always at the front of the queue.
    /// </summary>
    /// <typeparam name="T">The type of elements in the priority queue.</typeparam>
    /// <remarks>
    /// <para>
    ///   The items must be immutable as long as they are in the <see cref="PriorityQueue{T}"/>. The only exception is the first
    ///   item, which you may modify if you call <see cref="AdjustFirstItem"/> immediately afterward.
    /// </para>
    /// <para>
    ///   The <see cref="PriorityQueue{T}"/> always places the element with the lowest value at the
    ///   front. If you wish to have the element with the highest value at the front, use a constructor
    ///   that takes a <see cref="IComparer{T}"/>, and pass an instance of the <see cref="InvertedComparer{T}"/> class.
    /// </para>
    /// </remarks>
    /// <threadsafety static="true" instance="false" />
    public sealed class PriorityQueue<T> : IEnumerable<T>, ICollection, IEnumerable where T : IComparable<T>
    {
        private readonly List<T> _heap; // List that stores the binary heap tree.
        private object _syncRoot;

        private PriorityQueue(List<T> heap, IComparer<T> comparer)
        {
            Comparer = comparer ?? Comparer<T>.Default;
            _heap = heap;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> that contains elements
        /// copied from the specified <see cref="IEnumerable{T}"/> and that uses the specified 
        /// <see cref="IComparer{T}"/> implementation to compare keys.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to 
        /// the new <see cref="PriorityQueue{T}"/>.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when 
        /// comparing elements, or <see langword="null"/> to use the default <see cref="System.Collections.Generic.Comparer{T}"/>
        /// for the type of element.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. If comparer is <see langword="null"/>, this constructor uses the default
        ///   generic equality comparer, <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation.
        /// </para>
        /// <para>
        ///   This constructor is an O(<em>n</em>) operation, where <em>n</em> is the number of elements
        ///   in <paramref name="collection"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer)
            : this(default(List<T>), comparer)
        {
            if (collection == default(IEnumerable<T>))
                throw new ArgumentNullException("collection");
            _heap = new List<T>(collection);
            if (_heap.Count > 1)
            {
                // Starting at the parent of the last element (which is the last non-leaf node in the tree), perform the
                // down-heap operation to establish the heap property. This provides O(n) initialization, faster than calling
                // Enqueue for each item which would be O(n log n)
                for (int index = (_heap.Count - 1) >> 1; index >= 0; --index)
                {
                    DownHeap(index);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty, 
        /// has the specified initial capacity, and uses the specified <see cref="IComparer{T}"/>
        /// implementation to compare elements.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="PriorityQueue{T}"/> can contain.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when 
        /// comparing elements, or <see langword="null"/> to use the default <see cref="System.Collections.Generic.Comparer{T}"/>
        /// for the type of element.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. If comparer is <see langword="null"/>, this constructor uses the default
        ///   generic equality comparer, <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation.
        /// </para>
        /// <para>
        ///   This constructor is an O(<em>n</em>) operation, where <em>n</em> is <paramref name="capacity"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public PriorityQueue(int capacity, IComparer<T> comparer)
            : this(new List<T>(capacity), comparer)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> that contains elements copied from the specified <see cref="IEnumerable{T}"/>
        /// and uses the default <see cref="IComparer{T}"/> implementation for the element type.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied into the <see cref="PriorityQueue{T}"/>.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. This constructor uses the default generic equality comparer, 
        ///   <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation. Alternatively, you can specify an implementation of the
        ///   <see cref="IComparer{T}"/> generic interface by using a constructor that accepts a
        ///   <em>comparer</em> parameter.
        /// </para>
        /// <para>
        ///   This constructor is an O(<em>n</em>) operation, where <em>n</em> is the number of elements
        ///   in <paramref name="collection"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public PriorityQueue(IEnumerable<T> collection)
            : this(collection, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty, has the default initial capacity, and uses the
        /// specified <see cref="IComparer{T}"/> implementation to compare elements.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when 
        /// comparing elements, or <see langword="null"/> to use the default <see cref="System.Collections.Generic.Comparer{T}"/>
        /// for the type of element.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. If comparer is <see langword="null"/>, this constructor uses the default
        ///   generic equality comparer, <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation.
        /// </para>
        /// <para>
        ///   This constructor is an O(1) operation.
        /// </para>
        /// </remarks>
        public PriorityQueue(IComparer<T> comparer)
            : this(new List<T>(), comparer)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty, 
        /// has the default initial capacity, and uses the default <see cref="IComparer{T}"/>
        /// implementation for the element type.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. This constructor uses the default generic equality comparer, 
        ///   <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation. Alternatively, you can specify an implementation of the
        ///   <see cref="IComparer{T}"/> generic interface by using a constructor that accepts a
        ///   <em>comparer</em> parameter.
        /// </para>
        /// <para>
        ///   This constructor is an O(1) operation.
        /// </para>
        /// </remarks>
        public PriorityQueue()
            : this((IComparer<T>) null)
        { }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> that is used to compare the elements of the
        /// priority queue.
        /// </summary>
        /// <value>
        /// The <see cref="IComparer{T}"/> generic interface implementation that is used to compare the elements of the
        /// priority queue.
        /// </value>
        /// <remarks>
        /// <para>
        ///   Getting the value of this property is an O(1) operation.
        /// </para>
        /// </remarks>
        public readonly IComparer<T> Comparer; // { get; private set; }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <value>
        /// The number of elements that the <see cref="PriorityQueue{T}"/> can contain before resizing is required.
        /// </value>
        /// <remarks>
        /// <para>
        ///   <see cref="Capacity"/> is the number of elements that the <see cref="PriorityQueue{T}"/> can store before 
        ///   resizing is required, while <see cref="Count"/> is the number of elements that are actually in the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   <see cref="Capacity"/> is always greater than or equal to <see cref="Count"/>. If <see cref="Count"/> exceeds
        ///   <see cref="Capacity"/> while adding elements, the capacity is increased by automatically reallocating the
        ///   internal array before copying the old elements and adding the new elements. 
        /// </para>
        /// <para>
        ///   The capacity can be decreased by setting the <see cref="Capacity"/> property explicitly. When the value of
        ///   <see cref="Capacity"/> is set explicitly, the internal array is also reallocated to accommodate the
        ///   specified capacity, and all the elements are copied. 
        /// </para>
        /// <para>
        ///   Retrieving the value of this property is an O(1) operation; setting the property is an O(<em>n</em>) operation, where <em>n</em> is the new capacity. 
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="Capacity"/> is set to a value that is less than count.</exception>
        public int Capacity
        {
            get { return _heap.Capacity; }
            set { _heap.Capacity = value; }
        }

        /// <summary>
        /// Adds an object to the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the queue. The value can be <see langword="null"/> for reference types.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> accepts <see langword="null"/> as a valid value for reference types and allows duplicate elements.
        /// </para>
        /// <para>
        ///   The new element's position is determined by the <see cref="IComparable{T}"/> implementation used to compare elements.
        ///   If the new element is smaller than the current first element in the <see cref="PriorityQueue{T}"/>, the new element
        ///   will become the first element in the queue. Otherwise, the existing first element will remain the first element.
        /// </para>
        /// <para>
        ///   If <see cref="Count"/> already equals <see cref="Capacity"/>, the capacity of the <see cref="PriorityQueue{T}"/> is
        ///   increased by automatically reallocating the internal array, and the existing elements are copied to the new array
        ///   before the new element is added. 
        /// </para>
        /// <para>
        ///   If <see cref="Count"/> is less than <see cref="Capacity"/>, this method is an O(log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        ///   If the capacity needs to be increased to accommodate the new element, this method becomes an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public void Enqueue(T item)
        {
            _heap.Add(item);
            UpHeap();
        }

        /// <summary>
        /// Removes and returns the element with the lowest value from the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <returns>The object with the lowest value that was removed from the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   This method is similar to the <see cref="Peek"/> method, but <see cref="Peek"/> does not modify the <see cref="PriorityQueue{T}"/>. 
        /// </para>
        /// <para>
        ///   If type <typeparamref name="T"/> is a reference type, <see langword="null"/> can be added to the <see cref="PriorityQueue{T}"/> as a value. 
        /// </para>
        /// <para>
        ///   This method is an O(log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{T}"/> is empty.</exception>
        public T Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Empty PriorityQueue Error");
            T result = _heap[0];
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            if (_heap.Count > 0)
            {
                DownHeap(0);
            }
            return result;
        }

        /// <summary>
        /// Returns the object with the lowest value in the <see cref="PriorityQueue{T}"/> without removing it.
        /// </summary>
        /// <returns>The object with the lowest value in the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   This method is similar to the <see cref="Dequeue"/> method, but <see cref="Peek"/> does not modify the <see cref="PriorityQueue{T}"/>. 
        /// </para>
        /// <para>
        ///   If type <typeparamref name="T"/> is a reference type, <see langword="null"/> can be added to the <see cref="PriorityQueue{T}"/> as a value. 
        /// </para>
        /// <para>
        ///   This method is an O(1) operation.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{T}"/> is empty.</exception>
        public T Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Empty PriorityQueue Error");
            return _heap[0];
        }

        /// <summary>
        /// Indicates that the current first item of the <see cref="PriorityQueue{T}"/> was modified and its priority has to be re-evaluated.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   If <typeparamref name="T"/> is a reference type and not immutable, it may be possible to modify the value of
        ///   items in the queue. In general, this is not allowed and doing this will break the priority queue and lead to
        ///   undefined behaviour.
        /// </para>
        /// <para>
        ///   However, it is allowed to modify the current first element in the queue (which is returned by <see cref="Peek"/>)
        ///   if this change is followed by an immediate call to <see cref="AdjustFirstItem"/> which re-evaluates
        ///   the item's value and moves a different item to the front if necessary.
        /// </para>
        /// <para>
        ///   In the scenario that you are removing an item from the <see cref="PriorityQueue{T}"/> and immediately replacing it with a new one,
        ///   using this function can yield better performance, as the sequence of <see cref="Dequeue"/>, modify, <see cref="Enqueue"/> is twice as slow
        ///   as doing <see cref="Peek"/>, modify, <see cref="AdjustFirstItem"/>.
        /// </para>
        /// <para>
        ///   Because the first element may change after calling <see cref="AdjustFirstItem"/>, it is not safe to continue
        ///   modifying that same element afterwards. You must call <see cref="Peek"/> again to get the new front element which
        ///   may now be changed.
        /// </para>
        /// <para>
        ///   This method is an O(log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{T}"/> is empty.</exception>
        public void AdjustFirstItem()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Empty PriorityQueue Error");
            DownHeap(0);
        }

        /// <summary>
        /// Removes all objects from the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   <see cref="Count"/> is set to zero, and references to other objects from elements of the collection are also released.
        /// </para>
        /// <para>
        ///   The capacity remains unchanged. To reset the capacity of the <see cref="PriorityQueue{T}"/>, call <see cref="TrimExcess"/>.
        ///   Trimming an empty <see cref="PriorityQueue{T}"/> sets the capacity of the <see cref="PriorityQueue{T}"/> to the default capacity.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public void Clear()
        {
            _heap.Clear();
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="PriorityQueue{T}"/>, if that number is less than a threshold value.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   This method can be used to minimize a collection's memory overhead if no new elements will be added to the collection.
        ///   The cost of reallocating and copying a large <see cref="PriorityQueue{T}"/> can be considerable, however, so the <see cref="TrimExcess"/> method
        ///   does nothing if the list is at more than 90 percent of capacity. This avoids incurring a large reallocation cost for
        ///   a relatively small gain.
        /// </para>
        /// <note>
        ///   The current threshold of 90 percent it depends on <see cref="List{T}"/> and might change in future releases of the .Net Framework. 
        /// </note>
        /// <para>
        ///   This method is an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// <para>
        ///   To reset a <see cref="PriorityQueue{T}"/> to its initial state, call the <see cref="Clear"/> method before calling the <see cref="TrimExcess"/> method.
        ///   Trimming an empty <see cref="PriorityQueue{T}"/> sets the capacity of the <see cref="PriorityQueue{T}"/> to the default capacity. 
        /// </para>
        /// <para>
        ///   The capacity can also be set using the <see cref="Capacity"/> property.
        /// </para>
        /// </remarks>
        public void TrimExcess()
        {
            _heap.TrimExcess();
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="PriorityQueue{T}"/>. The value can be <see langword="null"/> for reference types. </param>
        /// <returns><see langword="true"/> if item is found in the <see cref="PriorityQueue{T}"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        ///   This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/> for <typeparamref name="T"/>, the type of values in the priority queue.
        /// </para>
        /// <para>
        ///   This method performs a linear search; therefore, this method is an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public bool Contains(T item)
        {
            return _heap.Contains(item);
        }

        /// <summary>
        /// Copies the <see cref="PriorityQueue{T}"/> elements to a new array. 
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   The elements are copied to the <see cref="Array"/> in the same order in which the enumerator iterates through the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em> log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public T[] ToArray()
        {
            T[] result = _heap.ToArray();
            // We want to return the elements in the same order in which they are enumerated, which is sorted order, so we simply sort.
            Array.Sort(result, Comparer);
            return result;
        }

        /// <summary>
        /// Copies the <see cref="PriorityQueue{T}"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="PriorityQueue{T}"/>.
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <remarks>
        /// <para>
        ///   The elements are copied to the <see cref="Array"/> in the same order in which the enumerator iterates through the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em> log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>, or the
        /// number of elements in the source <see cref="PriorityQueue{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to
        /// the end of the destination array. 
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _heap.CopyTo(array, arrayIndex);
            // We want to return the elements in the same order in which they are enumerated, which is sorted order, so we simply sort.
            Array.Sort(array, arrayIndex, _heap.Count, Comparer);
        }

        private void UpHeap()
        {
            int index = _heap.Count - 1;
            T item = _heap[index];
            int parentIndex = (index - 1) >> 1;
            // Because we can't easily tell when parentIndex goes beyond 0, we check index instead; if that was already zero, then we're at the top
            while (index > 0 && Comparer.Compare(item, _heap[parentIndex]) < 0)
            {
                _heap[index] = _heap[parentIndex];
                index = parentIndex;
                parentIndex = (index - 1) >> 1;
            }
            _heap[index] = item;
        }

        private void DownHeap(int index)
        {
            T item = _heap[index];
            int count = _heap.Count;
            int firstChild = (index << 1) + 1;
            int secondChild = firstChild + 1;
            int smallestChild = (secondChild < count && Comparer.Compare(_heap[secondChild], _heap[firstChild]) < 0) ? secondChild : firstChild;
            while (smallestChild < count && Comparer.Compare(_heap[smallestChild], item) < 0)
            {
                _heap[index] = _heap[smallestChild];
                index = smallestChild;
                firstChild = (index << 1) + 1;
                secondChild = firstChild + 1;
                smallestChild = (secondChild < count && Comparer.Compare(_heap[secondChild], _heap[firstChild]) < 0) ? secondChild : firstChild;
            }
            _heap[index] = item;
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the values in the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the values in the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   The elements of the <see cref="PriorityQueue{T}"/> will be enumerated in the same order as if you
        ///   had called <see cref="Dequeue"/> until the <see cref="PriorityQueue{T}"/> was empty. I.e. the
        ///   elements are enumerated from lowest to highest value, in sorted order.
        /// </para>
        /// <para>
        ///   The contents of the <see cref="PriorityQueue{T}"/> are not modified by enumerating.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em> log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            // We want to enumerate in the order you would get if calling Dequeue until the queue is empty.
            // A simple way to achieve that is to simple sort the heap, and to return an iterator over
            // the sorted copy.
            List<T> heapCopy = new List<T>(_heap);
            heapCopy.Sort(Comparer);
            return heapCopy.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Gets the number of elements contained in the <see cref="PriorityQueue{T}"/>. 
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="PriorityQueue{T}"/>. 
        /// </value>
        /// <remarks>
        /// <para>
        ///   <see cref="Capacity"/> is the number of elements that the <see cref="PriorityQueue{T}"/> can store before 
        ///   resizing is required, while <see cref="Count"/> is the number of elements that are actually in the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   <see cref="Capacity"/> is always greater than or equal to <see cref="Count"/>. If <see cref="Count"/> exceeds
        ///   <see cref="Capacity"/> while adding elements, the capacity is increased by automatically reallocating the
        ///   internal array before copying the old elements and adding the new elements. 
        /// </para>
        /// <para>
        ///   Retrieving the value of this property is an O(1) operation.
        /// </para>
        /// </remarks>
        public int Count
        {
            get { return _heap.Count; }
        }

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            ((System.Collections.ICollection) _heap).CopyTo(array, index);
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        #endregion
    }
    */

    /*
    using System.Linq;
    using System.Text;

    public sealed class PriorityQueue<T> : IEnumerable<T>, ICollection, IEnumerable where T : IComparable<T>
    {
        public const int DefaultSize = 16;

        private T[] _Buffer = null;
        private int _Count = 0;
        private object _SyncRoot = new object();

        public readonly Comparison<T> Comparer;

        public PriorityQueue()
            : this(DefaultSize)
        {
        }

        public PriorityQueue(Comparison<T> comparer)
        {
            Comparer = comparer;
            _Buffer = new T[DefaultSize];
        }

        public PriorityQueue(Comparison<T> comparer, int capacity)
        {
            Comparer = comparer;
            _Buffer = new T[capacity];
        }

        public PriorityQueue(int capacity)
        {
            Comparer = Comparer<T>.Default.Compare;
            _Buffer = new T[capacity];
        }

        public PriorityQueue(IEnumerable<T> contents)
        {
            Comparer = Comparer<T>.Default.Compare;
            _Buffer = contents.ToArray();
            _Count = _Buffer.Length;
            HeapHelper<T>.Heapify(_Buffer, _Count, Comparer);
        }

        public int Capacity
        {
            get { return _Buffer.Length; }
        }

        public int Count
        {
            get { return _Count; }
        }

        private void Resize(int newSize)
        {
            T[] newBuffer = new T[newSize];
            T[] oldBuffer = _Buffer;
            Array.Copy(oldBuffer, newBuffer, Math.Min(oldBuffer.Length, newBuffer.Length));
            _Buffer = newBuffer;
        }

        private void Grow(int numberOfItems)
        {
            _Count += numberOfItems;
            if (_Count > _Buffer.Length)
                Resize(_Buffer.Length * 2);
        }

        public void Enqueue(T value)
        {
            Grow(1);
            _Buffer[_Count - 1] = value;
            HeapHelper<T>.SiftDown(_Buffer, 0, _Count - 1, Comparer);
        }

        public T Dequeue()
        {
            if (_Count <= 0)
                throw new InvalidOperationException("The queue is empty.");

            _Count -= 1;
            T result = _Buffer[0];
            if (_Count > 0)
            {
                _Buffer[0] = _Buffer[_Count];
                _Buffer[_Count] = default(T);
                HeapHelper<T>.SiftUp(_Buffer, 0, _Count, Comparer);
            }
            return result;
        }

        public bool Peek(out T result)
        {
            if (_Count <= 0)
            {
                result = default(T);
                return false;
            }
            else
            {
                result = _Buffer[0];
                return true;
            }
        }

        public void Clear()
        {
            _Count = 0;
            Array.Clear(_Buffer, 0, _Buffer.Length);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _Buffer.TakeWhile((item, index) => index < _Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Buffer.TakeWhile((item, index) => index < _Count).GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Array.Copy(_Buffer, 0, array, index, _Count);
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return _SyncRoot; }
        }
    }

    public static class HeapHelper<T>
    {
        public static void SiftDown(T[] buffer, int startPosition, int endPosition, Comparison<T> comparer)
        {
            T newItem = buffer[endPosition];
            int newItemPos = endPosition;

            while (endPosition > startPosition)
            {
                int sourcePosition = (endPosition - 1) >> 1;
                T sourceItem = buffer[sourcePosition];

                if (comparer(sourceItem, newItem) <= 0)
                    break;

                buffer[sourcePosition] = default(T);
                buffer[endPosition] = sourceItem;

                endPosition = sourcePosition;
            }

            if (comparer(buffer[newItemPos], newItem) == 0)
                buffer[newItemPos] = default(T);

            buffer[endPosition] = newItem;
        }

        public static void SiftUp(T[] buffer, int position, int endPosition, Comparison<T> comparer)
        {
            int startPosition = position;
            T newItem = buffer[position];
            int newItemPos = position;

            int childPosition = 2 * position + 1;
            int rightPosition;

            while (childPosition < endPosition)
            {
                rightPosition = childPosition + 1;
                if ((rightPosition < endPosition) && (
                    comparer(buffer[rightPosition], buffer[childPosition]) <= 0
                ))
                    childPosition = rightPosition;

                buffer[position] = buffer[childPosition];
                buffer[childPosition] = default(T);

                position = childPosition;
                childPosition = 2 * position + 1;
            }

            if (comparer(buffer[newItemPos], newItem) == 0)
                buffer[newItemPos] = default(T);

            buffer[position] = newItem;

            SiftDown(buffer, startPosition, position, comparer);
        }

        public static void Heapify(T[] buffer, int count, Comparison<T> comparer)
        {
            var positions = Enumerable.Range(0, count / 2).Reverse();
            foreach (int i in positions)
                SiftUp(buffer, i, count, comparer);
        }
    }
    */

    #endregion
}