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

    public class CancelEventArgs<T1, T2> : CancelEventArgs, IEventArgs<T1, T2>
    {
        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }

        public CancelEventArgs(T1 value1, T2 value2, bool cancel)
            : base(cancel)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public CancelEventArgs(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

    }
}