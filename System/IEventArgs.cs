namespace System
{
    public interface IEventArgs<out T>
    {
        T Value { get; }
    }

    public interface IEventArgs<out T1, out T2>
    {
        T1 Value1 { get; }
        T2 Value2 { get; }
    }
}
