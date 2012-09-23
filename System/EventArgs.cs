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

    public class EventArgs<T, U> : EventArgs, IEventArgs<T, U>
    {
        public T Value1 { get; private set; }
        public U Value2 { get; private set; }

        public EventArgs(T value1, U value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }
}
