namespace System
{
    public class EventArgs<T> : EventArgs, IEventArgs<T>
    {
        public T Value { get; private set; }

        public EventArgs(T value)
        {
            Value = value;
        }
    }

    public class EventArgs<T1, T2> : EventArgs, IEventArgs<T1, T2>
    {
        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }

        public EventArgs(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}
