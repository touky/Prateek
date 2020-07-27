//#PRATEEK:#PRATEEK_DEFINE_SECTION#

namespace Prateek.Core.Code.CachedArray
{
    //#PRATEEK:#PRATEEK_CSHARP_NAMESPACE_CODE#

    //#PRATEEK:#PRATEEK_USING_NAMESPACE#

#if !SIZE_VALID
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
#endif
    using Prateek.Core.Code.Consts;

    ///------------------------------------------------------------------------
    /*#PRATEEK:public:PRATEEK#*/ struct CachedList/*#PRATEEK:#DEF_1#:PRATEEK#*/<T>
        : ICachedArray<T>
        , IInternalCachedArray<T>
        , ICollection<T>
        , IEnumerable<T>
        , IEnumerable
        , IList<T>
        , IReadOnlyList<T>
        , ICollection
        , IList
    {
        #region Fields
        internal int count;

        internal T defaultValue;

        //#PRATEEK:#FUNC_RESULT_0#
        #endregion

        #region Properties
        ///--------------------------------------------------------------------
        public int Size
        {
            get
            {
#if SIZE_VALID
                //#PRATEEK:return#FUNC_RESULT_2#
#else
                return 0;
#endif
            }
        }
        #endregion

        ///--------------------------------------------------------------------
        #region Get/Set
        T IInternalCachedArray<T>.GetSetCached(bool get, int index, T value = default)
        {
            Debug.Assert(index >= 0 && index < count);

            switch (index)
            {
                //#PRATEEK:#FUNC_RESULT_1#
                default: { return default; }
            }
        }
        #endregion

        #region ICachedArray<T> Members
        public int Count
        {
            get { return count; }
        }

        int IInternalCachedArray<T>.Count
        {
            get { return count; }
            set { count = value; }
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
            get { return this.GetCached(index, default(T)); }
            set
            {
                Debug.Assert(value is T);

                this.SetCached(index, (T) value);
            }
        }

        public T this[int index]
        {
            get { return this.GetCached(index, default(T)); }
            set { this.SetCached(index, value); }
        }

        public int Add(object value)
        {
            Debug.Assert(value is T);

            return this.CachedAdd((T) value);
        }

        public void Clear()
        {
            this.CachedClear(default(T));
        }

        public bool Contains(object value)
        {
            Debug.Assert(value is T);

            return this.CachedIndexOf((T) value) != Const.INDEX_NONE;
        }

        public int IndexOf(object value)
        {
            Debug.Assert(value is T);

            return this.CachedIndexOf((T) value);
        }

        public void Insert(int index, object value)
        {
            Debug.Assert(value is T);

            this.CachedInsert(index, (T) value);
        }

        public void Remove(object value)
        {
            Debug.Assert(value is T);

            this.CachedRemove((T) value);
        }

        public void RemoveAt(int index)
        {
            this.CachedRemoveAt(index, default(T));
        }

        public void CopyTo(Array array, int index)
        {
            Debug.Assert(array.Length - index < count);

            for (int i = 0; i < count; i++)
            {
                array.SetValue(this.GetCached(i, default(T)), index + i);
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
            return this.CachedIndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.CachedInsert(index, item);
        }

        public void Add(T item)
        {
            this.CachedAdd(item);
        }

        public void AddRange(T[] items)
        {
            Debug.Assert(items != null);

            foreach (T item in items)
            {
                this.CachedAdd(item);
            }
        }

        public bool Contains(T item)
        {
            return this.CachedIndexOf(item) != Const.INDEX_NONE;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Debug.Assert(array.Length - arrayIndex < count);

            for (int i = 0; i < count; i++)
            {
                array[arrayIndex + i] = this.GetCached(i, default(T));
            }
        }

        public bool Remove(T item)
        {
            return this.CachedRemove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new CachedArrayEnumerator<T>(this);
        }
        #endregion
    }
}
