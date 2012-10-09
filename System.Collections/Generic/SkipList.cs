namespace System.Collections.Generic
{
    using Visitors;
    using Properties;


    /// <summary>
    /// A Skip List data structure
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SkipList<TKey, TValue> : IVisitableDictionary<TKey, TValue>
    {
        #region Globals

        readonly IComparer<TKey> _comparerToUse;
        readonly int _maxLevelToUse;
        readonly double _probabilityToUse;
        int _currentLevel;
        int _itemsCount;
        readonly SkipListNode<TKey, TValue>[] _headNodes;
        readonly SkipListNode<TKey, TValue> _tail = new SkipListNode<TKey, TValue>();

        // Initialise the random number generator with the current time.
        readonly Random _rand = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SkipList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public SkipList() : this(16, 0.5, Comparer<TKey>.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkipList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public SkipList(IComparer<TKey> comparer) : this(16, 0.5, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkipList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="maxLevel">The max level.</param>
        /// <param name="probability">The probability.</param>
        /// <param name="comparer">The comparer.</param>
        public SkipList(int maxLevel, double probability, IComparer<TKey> comparer)
        {
            if (maxLevel < 1) throw new ArgumentOutOfRangeException("maxLevel", maxLevel, Resources.MaximumLevelBiggerThan0);

            if (comparer == null) throw new ArgumentNullException("comparer");

            if ((probability > 0.9) || (probability < 0.1)) throw new ArgumentOutOfRangeException("probability", probability, Resources.InvalidProbability);

            _comparerToUse = comparer;
            _maxLevelToUse = maxLevel;
            _probabilityToUse = probability;

            // Initialise the skip list to empty nodes, and link the heads and the tails
            _headNodes = new SkipListNode<TKey, TValue>[maxLevel];

            _headNodes[0] = new SkipListNode<TKey, TValue>
                            {
                                Right = _tail
                            };

            for (var i = 1; i < maxLevel; i++)
            {
                _headNodes[i] = new SkipListNode<TKey, TValue>
                                {
                                    Down = _headNodes[i - 1], Right = _tail
                                };
                //headNodes[i - 1].Up = headNodes[i];
            }
        }

        #endregion

        #region IVisitableCollection<T> Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IVisitor<KeyValuePair<TKey, TValue>> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }

            // Start at the bottom level and add all the keys to the return array.
            var startNode = _headNodes[0];

            for (int i = 0; i < Count; i++)
            {
                startNode = startNode.Right;
                visitor.Visit(new KeyValuePair<TKey, TValue>(startNode.Key, startNode.Value));

                if (visitor.HasCompleted)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value><c>true</c> if this collection is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
        public bool IsFull
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return _itemsCount;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            // Set all the heads' references to the tail to eliminate the items in between
            for (int i = 0; i < _maxLevelToUse; i++)
            {
                _headNodes[i].Right = _tail;
            }

            _itemsCount = 0;
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>		
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var node = Find(item.Key);

            return null != node && node.Value.Equals(item.Value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");

            if ((array.Length - arrayIndex) < Count) throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);

            using (var enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    array.SetValue(enumerator.Current, arrayIndex++);
                }
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A value indicating whether the key was found in the tree.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        #endregion

        #region IDictionary<T> Members


        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<TKey> Keys
        {
            get
            {
                // Start at the bottom level and add all the keys to the return array.
                var startNode = _headNodes[0];
                var keys = new TKey[Count];

                for (var i = 0; i < Count; i++)
                {
                    startNode = startNode.Right;
                    keys[i] = startNode.Key;
                }

                return keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<TValue> Values
        {
            get
            {
                // Start at the bottom level and add all the values to the return array.
                var startNode = _headNodes[0];
                var values = new TValue[Count];

                for (var i = 0; i < Count; i++)
                {
                    startNode = startNode.Right;
                    values[i] = startNode.Value;
                }

                return values;
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
                var node = Find(key);

                if (node == null) throw new ArgumentOutOfRangeException(Resources.KeyDoesNotExist);
                return node.Value;
            }
            set
            {
                var node = Find(key);

                if (node == null) throw new ArgumentOutOfRangeException(Resources.KeyDoesNotExist);
                node.Value = value;
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            var rightNodes = FindRightMostNodes(key);

            // Check if the item allready exists in the list.  If it does, throw an exception -
            // we will not allow duplicate items here.
            if ((rightNodes[0].Right != _tail) && (_comparerToUse.Compare(rightNodes[0].Right.Key, key) == 0))
            {
                throw new ArgumentException(Resources.ItemAlreadyInList);
            }
            var newLevel = PickRandomLevel();

            if (newLevel > _currentLevel)
            {
                for (var i = _currentLevel + 1; i <= newLevel; i++)
                {
                    rightNodes[i] = _headNodes[i];
                }

                _currentLevel = newLevel;
            }

            var newNode = new SkipListNode<TKey, TValue>(key, value)
                          {
                              Right = rightNodes[0].Right
                          };

            // Insert the item in the first level
            rightNodes[0].Right = newNode;

            // And now insert the node in the rest of the levels, making sure
            // to update the the links
            for (var i = 1; i <= _currentLevel; i++)
            {
                var previousNode = newNode;
                newNode = new SkipListNode<TKey, TValue>(key, value)
                          {
                              Right = rightNodes[i].Right
                          };

                rightNodes[i].Right = newNode;

                newNode.Down = previousNode;
            }

            _itemsCount++;
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return Find(key) != null;
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A value indicating whether the key was found (and removed) in the tree.</returns>
        public bool Remove(TKey key)
        {
            var rightNodes = FindRightMostNodes(key);

            // See if we actually found the node
            if ((rightNodes[0].Right == _tail) || (_comparerToUse.Compare(rightNodes[0].Right.Key, key) != 0)) return false;

            for (var i = 0; i < _currentLevel; i++)
            {
                // Since the node is consecutive levels, as soon as we don't find it on the next
                // level, we can stop.
                if ((rightNodes[i].Right != _tail) && (_comparerToUse.Compare(rightNodes[i].Right.Key, key) == 0)) rightNodes[i].Right = rightNodes[i].Right.Right;
                else break;
            }

            _itemsCount--;
            return true;
        }

        /// <summary>
        /// Tries to get the value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A value indiciating whether the node with the specified key was found in the tree.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = Find(key);

            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            value = node.Value;
            return true;
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator for enumerating though the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            // Start at the bottom level and add all the keys to the return array.
            SkipListNode<TKey, TValue> startNode = _headNodes[0];

            while (startNode.Right != _tail)
            {
                startNode = startNode.Right;
                yield return new KeyValuePair<TKey, TValue>(startNode.Key, startNode.Value);
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator for enumerating though the colleciton.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IComparable Members

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
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj.GetType() == GetType())
            {
                var s = obj as SkipList<TKey, TValue>;
                return Count.CompareTo(s.Count);
            }
            return String.Compare(GetType().FullName, obj.GetType().FullName, StringComparison.Ordinal);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the comparer used to compare items in this instance.
        /// </summary>
        /// <value>The comparer.</value>
        public IComparer<TKey> Comparer
        {
            get
            {
                return _comparerToUse;
            }
        }

        /// <summary>
        /// Gets the probability.
        /// </summary>
        /// <value>The probability.</value>
        public double Probability
        {
            get
            {
                return _probabilityToUse;
            }
        }

        /// <summary>
        /// Gets the max level.
        /// </summary>
        /// <value>The max level.</value>
        public int MaxListLevel
        {
            get
            {
                return _maxLevelToUse;
            }
        }

        /// <summary>
        /// Gets the current list level.
        /// </summary>
        /// <value>The current list level.</value>
        public int CurrentListLevel
        {
            get
            {
                return _currentLevel;
            }
        }

        #endregion

        #region Private Members

        private SkipListNode<TKey, TValue> Find(TKey key)
        {
            if (Count == 0) return null;
            // Start at the top list header node
            var currentNode = _headNodes[_currentLevel];

            while (true)
            {
                while ((currentNode.Right != _tail) && (_comparerToUse.Compare(currentNode.Right.Key, key) < 0))
                {
                    currentNode = currentNode.Right;
                }

                // Check if there is a next level, and if there is move down.
                if (currentNode.Down == null)
                {
                    break;
                }
                currentNode = currentNode.Down;
            }

            // Do one final comparison to see if the key to the right equals this key.
            // If it doesn't match, it would be bigger than this key.
            return _comparerToUse.Compare(currentNode.Right.Key, key) == 0 ? currentNode.Right : null;
        }

        private int PickRandomLevel()
        {
            var randomLevel = 0;

            while ((_rand.NextDouble() < _probabilityToUse) && (randomLevel <= _currentLevel + 1) && (randomLevel < _maxLevelToUse))
            {
                randomLevel++;
            }

            return randomLevel;
        }

        private SkipListNode<TKey, TValue>[] FindRightMostNodes(TKey key)
        {
            var rightNodes = new SkipListNode<TKey, TValue>[_maxLevelToUse];

            // Start at the top list header node
            var currentNode = _headNodes[_currentLevel];

            for (var i = _currentLevel; i >= 0; i--)
            {
                while ((currentNode.Right != _tail) && (_comparerToUse.Compare(currentNode.Right.Key, key) < 0))
                {
                    currentNode = currentNode.Right;
                }

                // Store this node - the new node will be to the right of it.
                rightNodes[i] = currentNode;

                // Check if there is a next level, and if there is move down.
                if (i > 0)
                {
                    currentNode = currentNode.Down;
                }
            }
            return rightNodes;
        }

        #endregion
    }
}
