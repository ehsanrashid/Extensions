namespace System
{
    public interface IEventArgs<out T>
    {
        T Value { get; }
    }

    public interface IEventArgs<out T, out U>
    {
        T Value1 { get; }
        U Value2 { get; }
    }
}
