namespace System.IO
{
    using Collections.Generic;

    public class CircularStream : Stream
    {
        readonly CircularBuffer<byte> _buffer;

        public CircularStream(int bufferCapacity)
        {
            _buffer = new CircularBuffer<byte>(bufferCapacity);
        }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Capacity
        {
            get { return _buffer.Capacity; }
            set { _buffer.Capacity = value; }
        }

        public override long Length
        {
            get { return _buffer.Size; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public byte[] GetBuffer() { return _buffer.GetBuffer(); }

        public byte[] ToArray() { return _buffer.ToArray(); }

        public override void Flush() { }

        public override void Write(byte[] buffer, int offset, int count) { _buffer.Put(buffer, offset, count); }

        public override void WriteByte(byte value) { _buffer.Put(value); }

        public override int Read(byte[] buffer, int offset, int count) { return _buffer.Get(buffer, offset, count); }

        public override int ReadByte() { return _buffer.Get(); }

        public override long Seek(long offset, SeekOrigin origin) { throw new NotImplementedException(); }

        public override void SetLength(long value) { throw new NotImplementedException(); }
    }
}