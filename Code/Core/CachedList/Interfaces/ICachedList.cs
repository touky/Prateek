namespace Prateek.Core.Code.CachedList.Interfaces
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
