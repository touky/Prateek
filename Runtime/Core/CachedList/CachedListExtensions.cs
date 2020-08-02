namespace Prateek.Runtime.Core.CachedList
{
    using Prateek.Runtime.Core.CachedList.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using UnityEngine;

    ///------------------------------------------------------------------------
    internal static class CachedListExtensions
    {
        #region Class Methods
        internal static int CachedAdd<TCachedList, T>(ref this TCachedList cachedList, T value)
            where TCachedList : struct, IInternalCachedList<T>
        {
            Debug.Assert(cachedList.Count < cachedList.Size);

            cachedList.SetCached(cachedList.Count++, value);
            return cachedList.Count - 1;
        }

        internal static bool CachedRemove<TCachedList, T>(ref this TCachedList cachedList, T value)
            where TCachedList : struct, IInternalCachedList<T>
        {
            var index = cachedList.CachedIndexOf(value);
            if (index == Const.INDEX_NONE)
            {
                return false;
            }

            cachedList.CachedRemoveAt(index, value);
            return true;
        }

        internal static void CachedRemoveAt<TCachedList, T>(ref this TCachedList cachedList, int index, T value)
            where TCachedList : struct, IInternalCachedList<T>
        {
            Debug.Assert(index >= 0 && index < cachedList.Count);
            Debug.Assert(cachedList.Count > 0);

            for (var i = index; i < cachedList.Count; i++)
            {
                cachedList.SetCached(i, cachedList.GetCached(i + 1, default(T)));
            }

            cachedList.Count--;
        }

        internal static void CachedInsert<TCachedList, T>(ref this TCachedList cachedList, int index, T value)
            where TCachedList : struct, IInternalCachedList<T>
        {
            Debug.Assert(index >= 0 && index < cachedList.Count);
            Debug.Assert(cachedList.Count < cachedList.Size);

            cachedList.Count++;
            for (var i = cachedList.Count - 1; i > index; i--)
            {
                cachedList.SetCached(i, cachedList.GetCached(i - 1, value));
            }

            cachedList.SetCached(index, value);
        }

        internal static void CachedClear<TCachedList, T>(ref this TCachedList cachedList, T value)
            where TCachedList : struct, IInternalCachedList<T>
        {
            cachedList.Count = 0;
            for (var i = 0; i < cachedList.Size; i++)
            {
                cachedList.GetSetCached(false, i);
            }
        }

        internal static int CachedIndexOf<TCachedList, T>(ref this TCachedList cachedList, T value)
            where TCachedList : struct, IInternalCachedList<T>
        {
            Debug.Assert(value is T);

            for (var i = 0; i < cachedList.Count; i++)
            {
                if (value.Equals(cachedList.GetSetCached(true, i)))
                {
                    return i;
                }
            }

            return Const.INDEX_NONE;
        }

        internal static TData GetCached<TCachedList, TData>(ref this TCachedList cachedList, int index, TData value)
            where TCachedList : struct, IInternalCachedList<TData>
        {
            Debug.Assert(index >= 0 && index < cachedList.Count);

            return cachedList.GetSetCached(true, index);
        }

        internal static TData SetCached<TCachedList, TData>(ref this TCachedList cachedList, int index, TData value)
            where TCachedList : struct, IInternalCachedList<TData>
        {
            Debug.Assert(index >= 0 && index < cachedList.Count);

            return cachedList.GetSetCached(false, index, value);
        }
        #endregion
    }
}
