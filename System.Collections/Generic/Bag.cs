namespace System.Collections.Generic
{
    using System;
    using Collections;
    using Visitors;
    using Diagnostics;
    using Properties;

    /// <summary>
    ///   A Bag data structure.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public sealed class Bag<T> : IVisitableCollection<T>, IBag<T>
    {
        readonly VisitableHashtable<T, int> _data;

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of elements contained in the <see cref="T:System.Collections.ICollection"></see> . </returns>
        public int Count { get; private set; }

        #region Construction
        /// <summary>
        ///   Initializes a new instance of the <see cref="Bag&lt;T&gt;" /> class.
        /// </summary>
        public Bag()
        {
            _data = new VisitableHashtable<T, int>();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Bag&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="capacity"> The initial capacity of the bag. </param>
        public Bag(int capacity)
        {
            _data = new VisitableHashtable<T, int>(capacity);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Bag&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="comparer"> The comparer to use when testing equality. </param>
        public Bag(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _data = new VisitableHashtable<T, int>(comparer);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Bag&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="capacity"> The initial capacity of the bag. </param>
        /// <param name="comparer"> The comparer to use when testing equality. </param>
        public Bag(int capacity, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _data = new VisitableHashtable<T, int>(capacity, comparer);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Bag&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="dictionary"> The dictionary to copy values from. </param>
        Bag(IDictionary<T, int> dictionary)
        {
            #region Asserts
            Debug.Assert(dictionary != null);
            #endregion

            _data = new VisitableHashtable<T, int>(dictionary);
            // Update the count
            using (var enumerator = _data.GetEnumerator())
                while (enumerator.MoveNext())
                    Count += enumerator.Current.Value;
        }
        #endregion

        #region IBag<T> Members
        /// <summary>
        ///   Removes count occurrences of the specified item.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="max"> The maximum number of items to remove. </param>
        /// <returns> A value indicating whether the items have been removed (was found). </returns>
        public bool Remove(T item, int max)
        {
            if (!_data.ContainsKey(item)) return false;
            if (max >= _data[item])
            {
                Count -= _data[item];
                _data.Remove(item);
            }
            else
            {
                Count -= max;
                _data[item] -= max;
            }
            return true;
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item"> The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <param name="amount"> The amount. </param>
        /// <exception cref="T:System.NotSupportedException">The
        ///   <see cref="T:System.Collections.Generic.ICollection`1"></see>
        ///   is read-only.</exception>
        public void Add(T item, int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(Resources.OnlyAddPositiveAmount);
            if (_data.ContainsKey(item))
                _data[item] += amount;
            else
                _data.Add(item, amount);
            Count += amount;
        }

        IBag<T> IBag<T>.Difference(IBag<T> bag)
        {
            return Difference((Bag<T>) bag);
        }

        IBag<T> IBag<T>.Intersection(IBag<T> bag)
        {
            return Intersection((Bag<T>) bag);
        }

        bool IBag<T>.IsEqual(IBag<T> bag)
        {
            return IsEqual((Bag<T>) bag);
        }

        IBag<T> IBag<T>.Union(IBag<T> bag)
        {
            return Union((Bag<T>) bag);
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
            var enumerator = _data.GetEnumerator();
            var counter = arrayIndex;
            while (enumerator.MoveNext())
            {
                var itemCount = enumerator.Current.Value;
                var obj = enumerator.Current.Key;
                for (var i = 0; i < itemCount; i++)
                    array.SetValue(obj, counter++);
            }
            enumerator.Dispose();
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
            if (_data.ContainsKey(item))
                _data[item]++;
            else
                _data.Add(item, 1);
            Count++;
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
            if (_data.ContainsKey(item))
            {
                _data[item]--;
                if (_data[item] == 0)
                    _data.Remove(item);
                Count--;
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item"> The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see> . </param>
        /// <returns> true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see> ; otherwise, false. </returns>
        public bool Contains(T item)
        {
            return _data.ContainsKey(item);
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = _data.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current.Key;
        }

        /// <summary>
        ///   Clears all the objects in this instance.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
            Count = 0;
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
                var bag = obj as Bag<T>;
                return Count.CompareTo(bag.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        /// <summary>
        ///   Determines whether the specified bag is equal to this one.
        /// </summary>
        /// <param name="bag"> The bag. </param>
        /// <returns> <c>true</c> if the specified bag is equal this this one; otherwise, <c>false</c> . </returns>
        public bool IsEqual(Bag<T> bag)
        {
            if (bag == null)
                throw new ArgumentNullException("bag");
            if (bag.Count != Count)
                return false;
            var enumerator = _data.GetEnumerator();
            while (enumerator.MoveNext())
                if (!bag.Contains(enumerator.Current.Key))
                    return false;
                else if (bag[enumerator.Current.Key] != enumerator.Current.Value)
                    return false;
            return true;
        }

        /// <summary>
        ///   Removes all instances of  the specified item in the bag.
        /// </summary>
        /// <param name="item"> The item to be removed. </param>
        /// <returns> A value indicating if the item was found (and removed) from the bag. </returns>
        public bool RemoveAll(T item)
        {
            if (_data.ContainsKey(item))
            {
                Count -= _data[item];
                _data.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection. </returns>
        public IEnumerator<KeyValuePair<T, int>> GetCountEnumerator()
        {
            return _data.GetEnumerator();
        }

        /// <summary>
        ///   Computes the union of this bag and the specified bag.
        /// </summary>
        /// <param name="bag"> The bag. </param>
        /// <returns> The union of this bag and the bag specified. </returns>
        public Bag<T> Union(Bag<T> bag)
        {
            if (bag == null)
                throw new ArgumentNullException("bag");
            Bag<T> result;
            Dictionary<T, int>.Enumerator enumerator;
            // A small optimisation for big Bags - make a copy of the biggest Bag
            if (bag._data.Count > _data.Count)
            {
                result = new Bag<T>(bag._data);
                enumerator = _data.GetEnumerator();
            }
            else
            {
                result = new Bag<T>(_data);
                enumerator = bag._data.GetEnumerator();
            }
            while (enumerator.MoveNext()) result.Add(enumerator.Current.Key, enumerator.Current.Value);
            enumerator.Dispose();
            return result;
        }

        /// <summary>
        ///   Computes the difference between this bag and the specified bag.
        /// </summary>
        /// <param name="bag"> The bag. </param>
        /// <returns> The difference between this bag and the bag specified. </returns>
        public Bag<T> Difference(Bag<T> bag)
        {
            if (bag == null)
                throw new ArgumentNullException("bag");
            var result = new Bag<T>(_data);
            using (var enumerator = bag._data.GetEnumerator())
                while (enumerator.MoveNext())
                    if (result._data.ContainsKey(enumerator.Current.Key))
                    {
                        var itemCount = result._data[enumerator.Current.Key];
                        if (itemCount - enumerator.Current.Value <= 0)
                            result.RemoveAll(enumerator.Current.Key);
                        else
                            result.Remove(enumerator.Current.Key, enumerator.Current.Value);
                    }
            return result;
        }

        /// <summary>
        ///   Computes the intersection between this bag and the specified bag.
        /// </summary>
        /// <param name="bag"> The bag. </param>
        /// <returns> The intersection between this bag and the specified bag. </returns>
        public Bag<T> Intersection(Bag<T> bag)
        {
            if (bag == null)
                throw new ArgumentNullException("bag");
            var result = new Bag<T>();
            var enumerator = bag._data.GetEnumerator();
            while (enumerator.MoveNext())
                if (_data.ContainsKey(enumerator.Current.Key))
                    result.Add(enumerator.Current.Key,
                               Math.Min(enumerator.Current.Value, _data[enumerator.Current.Key])
                        );
            return result;
        }

        /// <summary>
        ///   Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor"> The visitor. </param>
        public void Accept(IVisitor<KeyValuePair<T, int>> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            _data.Accept(visitor);
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

        #region Operator Overloads
        /// <summary>
        ///   Indicates whether an item is present in this Bag and returns the count.
        /// </summary>
        /// <value> </value>
        public int this[T item]
        {
            get { return _data.ContainsKey(item) ? _data[item] : 0; }
        }

        /// <summary>
        ///   Operator + : Performs a union between two Bags.
        /// </summary>
        /// <param name="b1"> The left hand Bag. </param>
        /// <param name="b2"> The right hand Bag. </param>
        /// <returns> The union between this bag and the bag specified. </returns>
        public static Bag<T> operator +(Bag<T> b1, Bag<T> b2)
        {
            return b1.Union(b2);
        }

        /// <summary>
        ///   Operator - : Performs a difference operation between two Bags.
        /// </summary>
        /// <param name="b1"> The left hand Bag. </param>
        /// <param name="b2"> The right hand Bag. </param>
        /// <returns> The difference between this bag and the bag specified. </returns>
        public static Bag<T> operator -(Bag<T> b1, Bag<T> b2)
        {
            return b1.Difference(b2);
        }

        /// <summary>
        ///   Operator * : Performs a intersection between two Bags.
        /// </summary>
        /// <param name="b1"> The left hand Bag. </param>
        /// <param name="b2"> The right hand Bag. </param>
        /// <returns> The intersection between this bag and the bag specified. </returns>
        public static Bag<T> operator *(Bag<T> b1, Bag<T> b2)
        {
            return b1.Intersection(b2);
        }
        #endregion
    }
}