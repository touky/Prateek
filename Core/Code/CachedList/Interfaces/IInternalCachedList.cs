namespace Prateek.Core.Code.CachedList.Interfaces
{
    ///------------------------------------------------------------------------
    internal interface IInternalCachedList<T>
    {
        #region Properties
        int Count { get; set; }
        int Size { get; }
        T GetSetCached(bool get, int index, T value = default);
        #endregion
    }
}
