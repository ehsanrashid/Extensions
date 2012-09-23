namespace System
{
    using ComponentModel;

    public class CancelEventArgs<T> : CancelEventArgs, IEventArgs<T>
    {
        public T Value { get; private set; }

        public CancelEventArgs(T value, bool cancel)
            : base(cancel)
        {
            Value = value;
        }

        public CancelEventArgs(T value)
        {
            Value = value;
        }

        public static implicit operator CancelEventArgs<T>(T value)
        {
            return new CancelEventArgs<T>(value);
        }
    }

    public class CancelEventArgs<T, U> : CancelEventArgs, IEventArgs<T, U>
    {
        public T Value1 { get; private set; }
        public U Value2 { get; private set; }

        public CancelEventArgs(T value1, U value2, bool cancel)
            : base(cancel)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public CancelEventArgs(T value1, U value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

    }
}