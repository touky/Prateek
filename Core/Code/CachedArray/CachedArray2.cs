namespace Prateek.Core.Code.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Core.Code.CachedArray;
    using UnityEngine;

    ///------------------------------------------------------------------------
    public struct CachedArray2<T> : ICachedArray<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyList<T>, ICollection, IList
    {
        #region Static and Constants
        public const int ARRAY_SIZE = 2;
        #endregion

        #region Fields
        private int count;
        private T defaultValue;
        private T value0;
        private T value1;
        #endregion

        #region Properties
        ///--------------------------------------------------------------------
        public int Size
        {
            get { return ARRAY_SIZE; }
        }
        #endregion

        #region Class Methods
        ///--------------------------------------------------------------------
        private T GetSetCached(bool get, int index, T value = default)
        {
            Debug.Assert(index >= 0 && index < count);

            switch (index)
            {
                case 0: { return get ? value0 : value0 = value; }
                case 1: { return get ? value1 : value1 = value; }
            }

            return default;
        }

        private int CachedAdd(T value)
        {
            Debug.Assert(count < Size);

            SetCached(count++, value);
            return count - 1;
        }

        private bool CachedRemove(T value)
        {
            var index = CachedIndexOf(value);
            if (index == Const.INDEX_NONE)
            {
                return false;
            }

            CachedRemoveAt(index);
            return true;
        }

        private void CachedRemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < count);
            Debug.Assert(count > 0);

            for (var i = index; i < count; i++)
            {
                SetCached(i, GetCached(i + 1));
            }

            count--;
        }

        private void CachedInsert(int index, T value)
        {
            Debug.Assert(index >= 0 && index < count);
            Debug.Assert(count < ARRAY_SIZE);

            count++;
            for (var i = count - 1; i > index; i--)
            {
                SetCached(i, GetCached(i - 1));
            }

            SetCached(index, value);
        }

        private void CachedClear()
        {
            count = 0;
            for (var i = 0; i < ARRAY_SIZE; i++)
            {
                GetSetCached(false, i);
            }
        }

        public int CachedIndexOf(T value)
        {
            Debug.Assert(value is T);

            for (var i = 0; i < count; i++)
            {
                if (value.Equals(GetSetCached(true, i)))
                {
                    return i;
                }
            }

            return Const.INDEX_NONE;
        }

        private T GetCached(int index)
        {
            Debug.Assert(index >= 0 && index < count);

            return GetSetCached(true, index);
        }

        private T SetCached(int index, T value)
        {
            Debug.Assert(index >= 0 && index < count);

            return GetSetCached(false, index, value);
        }
        #endregion

        #region ICachedArray<T> Members
        public int Count
        {
            get { return count; }
        }
        #endregion

        #region IList
        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        object IList.this[int index]
        {
            get { return GetCached(index); }
            set
            {
                Debug.Assert(value is T);

                SetCached(index, (T) value);
            }
        }

        public T this[int index]
        {
            get { return GetCached(index); }
            set { SetCached(index, value); }
        }

        public int Add(object value)
        {
            Debug.Assert(value is T);

            return CachedAdd((T) value);
        }

        public void Clear()
        {
            CachedClear();
        }

        public bool Contains(object value)
        {
            Debug.Assert(value is T);

            return CachedIndexOf((T) value) != Const.INDEX_NONE;
        }

        public int IndexOf(object value)
        {
            Debug.Assert(value is T);

            return CachedIndexOf((T) value);
        }

        public void Insert(int index, object value)
        {
            Debug.Assert(value is T);

            CachedInsert(index, (T) value);
        }

        public void Remove(object value)
        {
            Debug.Assert(value is T);

            CachedRemove((T) value);
        }

        public void RemoveAt(int index)
        {
            CachedRemoveAt(index);
        }

        public void CopyTo(Array array, int index)
        {
            Debug.Assert(array.Length - index < count);

            for (var i = 0; i < count; i++)
            {
                array.SetValue(GetCached(i), index + i);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new CachedArrayEnumerator<T>(this);
        }
        #endregion

        #region IList<T>
        public int IndexOf(T item)
        {
            return CachedIndexOf(item);
        }

        public void Insert(int index, T item)
        {
            CachedInsert(index, item);
        }

        public void Add(T item)
        {
            CachedAdd(item);
        }

        public void AddRange(T[] items)
        {
            Debug.Assert(items != null);

            foreach (var item in items)
            {
                CachedAdd(item);
            }
        }

        public bool Contains(T item)
        {
            return CachedIndexOf(item) != Const.INDEX_NONE;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Debug.Assert(array.Length - arrayIndex < count);

            for (var i = 0; i < count; i++)
            {
                array[arrayIndex + i] = GetCached(i);
            }
        }

        public bool Remove(T item)
        {
            return CachedRemove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new CachedArrayEnumerator<T>(this);
        }
        #endregion
    }
}
