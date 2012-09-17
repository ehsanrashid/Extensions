namespace System.Collections.Generic
{
    /// <summary>
    /// An interface combining the IVisitableCollection and IList interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVisitableList<T> : IVisitableCollection<T>, IList<T>
    {
    }
}
