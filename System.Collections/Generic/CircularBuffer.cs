namespace System.Collections.Generic
{
    using IO;
    using Threading;
    using Collections;
    using Properties;

    public class CircularBuffer<T> : ICollection<T>, ICollection
    {
        int _capacity;

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value == _capacity) return;
                if (value < Size) throw new ArgumentOutOfRangeException("value", "value must be greater than or equal to the buffer size.");
                var dst = new T[value];
                if (Size > 0) CopyTo(dst);
                _buffer = dst;
                _capacity = value;
            }
        }

        public int Size { get; private set; }

        T[] _buffer;

        int _head;
        int _tail;

        [NonSerialized]
        Object _syncRoot;

        public CircularBuffer(int capacity, bool allowOverflow = false)
        {
            if (capacity < 0) throw new ArgumentException(Resources.CapacityMustBeGreaterOrEqualZero, "capacity");
            _capacity = capacity;
            _buffer = new T[capacity];
            Size = 0;
            _head = 0;
            _tail = 0;
            AllowOverflow = allowOverflow;
        }

        public bool AllowOverflow { get; set; }

        public bool Contains(T item)
        {
            var bufferIndex = _head;
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < Size; ++i, ++bufferIndex)
            {
                if (bufferIndex == _capacity) bufferIndex = 0;
                if ((default(T).Equals(item) && default(T).Equals(_buffer[bufferIndex]))
                    || (!default(T).Equals(item) && comparer.Equals(_buffer[bufferIndex], item)))
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            Size = 0;
            _head = 0;
            _tail = 0;
        }

        public int Put(T[] src)
        {
            return Put(src, 0, src.Length);
        }

        public int Put(T[] src, int offset, int count)
        {
            var actualCount = AllowOverflow ? count : Math.Min(count, _capacity - Size);
            var srcIndex = offset;
            for (var i = 0; i < actualCount; ++i, ++_tail, ++srcIndex)
            {
                if (_tail == _capacity) _tail = 0;
                _buffer[_tail] = src[srcIndex];
            }
            Size = Math.Min(Size + actualCount, _capacity);
            return actualCount;
        }

        public void Put(T item)
        {
            if (!AllowOverflow && Size == _capacity) throw new InternalBufferOverflowException("Buffer is full.");
            _buffer[_tail] = item;
            if (_tail++ == _capacity) _tail = 0;
            Size++;
        }

        public void Skip(int count)
        {
            _head += count;
            if (_head >= _capacity) _head -= _capacity;
        }

        public T[] Get(int count)
        {
            var dst = new T[count];
            Get(dst);
            return dst;
        }

        public int Get(T[] dst)
        {
            return Get(dst, 0, dst.Length);
        }

        public int Get(T[] dst, int offset, int count)
        {
            var actualCount = Math.Min(count, Size);
            var dstIndex = offset;
            for (var i = 0; i < actualCount; i++, _head++, dstIndex++)
            {
                if (_head == _capacity) _head = 0;
                dst[dstIndex] = _buffer[_head];
            }
            Size -= actualCount;
            return actualCount;
        }

        public T Get()
        {
            if (Size == 0) throw new InvalidOperationException(Resources.BufferIsEmpty);
            var item = _buffer[_head];
            --Size;
            if (_head++ == _capacity) _head = 0;
            return item;
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Size);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (count > Size)
                throw new ArgumentOutOfRangeException("count", "count cannot be greater than the buffer size.");
            var bufferIndex = _head;
            for (var i = 0; i < count; ++i, ++bufferIndex, ++arrayIndex)
            {
                if (bufferIndex == _capacity) bufferIndex = 0;
                array[arrayIndex] = _buffer[bufferIndex];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var bufferIndex = _head;
            for (var i = 0; i < Size; ++i, ++bufferIndex)
            {
                if (bufferIndex == _capacity) bufferIndex = 0;
                yield return _buffer[bufferIndex];
            }
        }

        public T[] GetBuffer()
        {
            return _buffer;
        }

        public T[] ToArray()
        {
            var dst = new T[Size];
            CopyTo(dst);
            return dst;
        }

        #region ICollection<T> Members
        int ICollection<T>.Count
        {
            get { return Size; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T item)
        {
            Put(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            if (Size == 0)
                return false;
            Get();
            return true;
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

        #region ICollection Members
        int ICollection.Count
        {
            get { return Size; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (null == _syncRoot)
                    Interlocked.CompareExchange(ref _syncRoot, new Object(), null);
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            CopyTo((T[]) array, arrayIndex);
        }
        #endregion
    }
}