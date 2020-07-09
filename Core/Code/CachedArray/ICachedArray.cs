namespace Prateek.Core.Code.CachedArray
{
    using UnityEngine;
    using Prateek.Core.Code.Consts;

    ///------------------------------------------------------------------------
    public interface ICachedArray<T>
    {
        #region Properties
        int Count { get; }
        T this[int index] { get; }
        #endregion
    }

    internal interface IInternalCachedArray<T>
    {
        #region Properties
        int Count { get; set; }
        int Size { get; }
        T GetSetCached(bool get, int index, T value = default);
        #endregion
    }

    internal static class CachedArrayExtensions
    {
        internal static int CachedAdd<TCachedArray, T>(ref this TCachedArray cachedArray, T value)
            where TCachedArray : struct, IInternalCachedArray<T>
        {
            Debug.Assert(cachedArray.Count < cachedArray.Size);

            cachedArray.SetCached(cachedArray.Count++, value);
            return cachedArray.Count - 1;
        }

        internal static bool CachedRemove<TCachedArray, T>(ref this TCachedArray cachedArray, T value)
            where TCachedArray : struct, IInternalCachedArray<T>
        {
            var index = cachedArray.CachedIndexOf(value);
            if (index == Const.INDEX_NONE)
            {
                return false;
            }

            cachedArray.CachedRemoveAt(index, value);
            return true;
        }

        internal static void CachedRemoveAt<TCachedArray, T>(ref this TCachedArray cachedArray, int index, T value)
            where TCachedArray : struct, IInternalCachedArray<T>
        {
            Debug.Assert(index >= 0 && index < cachedArray.Count);
            Debug.Assert(cachedArray.Count > 0);

            for (var i = index; i < cachedArray.Count; i++)
            {
                cachedArray.SetCached(i, cachedArray.GetCached(i + 1, default(T)));
            }

            cachedArray.Count--;
        }

        internal static void CachedInsert<TCachedArray, T>(ref this TCachedArray cachedArray, int index, T value)
            where TCachedArray : struct, IInternalCachedArray<T>
        {
            Debug.Assert(index >= 0 && index < cachedArray.Count);
            Debug.Assert(cachedArray.Count < cachedArray.Size);

            cachedArray.Count++;
            for (var i = cachedArray.Count - 1; i > index; i--)
            {
                cachedArray.SetCached(i, cachedArray.GetCached(i - 1, value));
            }

            cachedArray.SetCached(index, value);
        }

        internal static void CachedClear<TCachedArray, T>(ref this TCachedArray cachedArray, T value)
            where TCachedArray : struct, IInternalCachedArray<T>
        {
            cachedArray.Count = 0;
            for (var i = 0; i < cachedArray.Size; i++)
            {
                cachedArray.GetSetCached(false, i);
            }
        }

        internal static int CachedIndexOf<TCachedArray, T>(ref this TCachedArray cachedArray, T value)
            where TCachedArray : struct, IInternalCachedArray<T>
        {
            Debug.Assert(value is T);

            for (var i = 0; i < cachedArray.Count; i++)
            {
                if (value.Equals(cachedArray.GetSetCached(true, i)))
                {
                    return i;
                }
            }

            return Const.INDEX_NONE;
        }

        internal static TData GetCached<TCachedArray, TData>(ref this TCachedArray cachedArray, int index, TData value)
            where TCachedArray : struct, IInternalCachedArray<TData>
        {
            Debug.Assert(index >= 0 && index < cachedArray.Count);

            return cachedArray.GetSetCached(true, index);
        }
        
        internal static TData SetCached<TCachedArray, TData>(ref this TCachedArray cachedArray, int index, TData value)
            where TCachedArray : struct, IInternalCachedArray<TData>
        {
            Debug.Assert(index >= 0 && index < cachedArray.Count);

            return cachedArray.GetSetCached(false, index, value);
        }
    }

}
