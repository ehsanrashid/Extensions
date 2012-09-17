using System.Linq;

namespace System.Collections.Generic
{
    using Runtime.Serialization;

    /// <summary>
    ///   A Dictionary that accepts multiple values for a unique key.
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    [Serializable]
    public sealed class HashList<TKey, TValue> : VisitableHashtable<TKey, IList<TValue>>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        public HashList() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="dictionary"> The dictionary. </param>
        public HashList(IDictionary<TKey, IList<TValue>> dictionary)
            : base(dictionary) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="comparer"> The comparer. </param>
        public HashList(IEqualityComparer<TKey> comparer)
            : base(comparer) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="capacity"> The capacity. </param>
        public HashList(int capacity)
            : base(capacity) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="capacity"> The capacity. </param>
        /// <param name="comparer"> The comparer. </param>
        public HashList(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;" /> class.
        /// </summary>
        /// <param name="info"> A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see
        ///    cref="T:System.Collections.Generic.Dictionary`2"></see> . </param>
        /// <param name="context"> A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see
        ///    cref="T:System.Collections.Generic.Dictionary`2"></see> . </param>
        HashList(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #region Public Members

        /// <summary>
        ///   Gets the count of values in this HashList.
        /// </summary>
        /// <value> The count of values. </value>
        public int ValueCount
        {
            get
            {
                var count = 0;
                using (var enumerator = GetEnumerator()) while (enumerator.MoveNext()) if (enumerator.Current.Value != null) count += enumerator.Current.Value.Count;
                return count;
            }
        }

        /// <summary>
        ///   Gets the count of values in this HashList.
        /// </summary>
        /// <value> The count of values. </value>
        public int KeyCount
        {
            get { return Count; }
        }

        /// <summary>
        ///   Gets an enumerator for enumerating though values.
        /// </summary>
        /// <returns> A enumerator for enumerating through values in the Hash IList. </returns>
        public IEnumerator<TValue> GetValueEnumerator()
        {
            // Note :
            // Can not use using {} for the enumerator here.
            // It appears that a reference is kept to the enumerator and the enumeration only happens
            // after the enumerator has been disposed - some interesting behaviour follows.  To do : Investigate IL.
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext()) if (enumerator.Current.Value != null) foreach (var val in enumerator.Current.Value) yield return val;
        }

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        public void Add(TKey key, TValue value)
        {
            IList<TValue> list;
            if (ContainsKey(key))
            {
                list = this[key];
                if (list == null)
                {
                    list = new List<TValue>();
                    this[key] = list;
                }
            }
            else
            {
                list = new List<TValue>();
                this[key] = list;
            }
            list.Add(value);
        }

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="values"> The values. </param>
        public void Add(TKey key, ICollection<TValue> values)
        {
            if (values == null) throw new ArgumentNullException("values");
            IList<TValue> list;
            if (ContainsKey(key))
            {
                list = this[key];
                if (list == null)
                {
                    list = new List<TValue>();
                    this[key] = list;
                }
            }
            else
            {
                list = new List<TValue>();
                this[key] = list;
            }
            ((List<TValue>) list).AddRange(values);
        }

        /// <summary>
        ///   Removes the first occurrence of the value found.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <returns> A indication of whether the item has been found (and removed) in the Hash IList. </returns>
        public bool Remove(TValue item)
        {
            var dictKeys = Keys;
            IList<TKey> keys = new List<TKey>(dictKeys);
            //for (var i = 0; i < keys.Count; i++)
            //{
            //    var values = this[keys[i]];
            //    if (values != null)
            //        if (values.Remove(item)) return true;
            //}
            //return false;
            return Enumerable.Select(keys, t => this[t]).Where(values => values != null).Any(values => values.Remove(item));
        }

        /// <summary>
        ///   Removes all the ocurrences of the item in the HashList
        /// </summary>
        /// <param name="item"> The item. </param>
        public void RemoveAll(TValue item)
        {
            var dictKeys = Keys;
            IList<TKey> keys = new List<TKey>(dictKeys);
            foreach (var key in keys)
            {
                var values = this[key];
                if (values != null)
                    for (var j = 0; j < values.Count; j++)
                        if (values[j].Equals(item))
                        {
                            values.RemoveAt(j);
                            j--;
                        }
            }
        }

        /// <summary>
        ///   Removes all the ocurrences of the item in the HashList
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="item"> The item. </param>
        /// <returns> An indeication of whether the key and value pair has been found (and removed). </returns>
        public bool Remove(TKey key, TValue item)
        {
            if (!ContainsKey(key)) return false;
            var values = this[key];
            return (null != values) && values.Remove(item);
        }

        #endregion

    }
}