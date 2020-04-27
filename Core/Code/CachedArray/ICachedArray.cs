namespace Prateek.Core.Code.CachedArray
{
    ///------------------------------------------------------------------------
    public interface ICachedArray<T>
    {
        #region Properties
        int Count { get; }
        T this[int index] { get; }
        #endregion
    }
}
