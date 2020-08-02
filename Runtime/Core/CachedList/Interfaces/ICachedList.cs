namespace Prateek.Runtime.Core.CachedList.Interfaces
{
    ///------------------------------------------------------------------------
    public interface ICachedList<T>
    {
        #region Properties
        int Count { get; }
        T this[int index] { get; }
        #endregion
    }
}
