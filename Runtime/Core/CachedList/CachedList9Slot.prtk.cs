// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 21/08/2020
//
//  Copyright � 2017-2020 "Touky" <touky at prateek dot top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

#define SIZE_VALID

namespace Prateek.Runtime.Core.CachedList
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using Prateek;
    using static Statics.Statics;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-

    

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.CachedList.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using UnityEngine;
#if !SIZE_VALID
#endif

    ///------------------------------------------------------------------------
    public struct CachedList9<T>
        : ICachedList<T>
        , IInternalCachedList<T>
        , ICollection<T>
        , IEnumerable<T>
        , IEnumerable
        , IList<T>
        , IReadOnlyList<T>
        , ICollection
        , IList
    {
        #region Fields
        public const int SIZE = 
#if SIZE_VALID
        9;
#else
        0;
#endif

        internal int count;

        internal T defaultValue;

        private T value0;
        private T value1;
        private T value2;
        private T value3;
        private T value4;
        private T value5;
        private T value6;
        private T value7;
        private T value8;
        #endregion

        #region Properties
        ///--------------------------------------------------------------------
        public int Size
        {
            get { return SIZE; }
        }
        #endregion

        ///--------------------------------------------------------------------
        #region CTor
        public CachedList9(T defaultValue)
        {
            this.defaultValue = defaultValue;
            value0 = defaultValue;
            value1 = defaultValue;
            value2 = defaultValue;
            value3 = defaultValue;
            value4 = defaultValue;
            value5 = defaultValue;
            value6 = defaultValue;
            value7 = defaultValue;
            value8 = defaultValue;
            count = 0;
        }
        #endregion

        ///--------------------------------------------------------------------
        #region Get/Set
        T IInternalCachedList<T>.GetSetCached(bool get, int index, T value = default)
        {
            Debug.Assert(index >= 0 && index < count);

            switch (index)
            {
                case 0: { return get ? value0 : value0 = value; }
                case 1: { return get ? value1 : value1 = value; }
                case 2: { return get ? value2 : value2 = value; }
                case 3: { return get ? value3 : value3 = value; }
                case 4: { return get ? value4 : value4 = value; }
                case 5: { return get ? value5 : value5 = value; }
                case 6: { return get ? value6 : value6 = value; }
                case 7: { return get ? value7 : value7 = value; }
                case 8: { return get ? value8 : value8 = value; }
                default: { return default; }
            }
        }
        #endregion

        #region ICachedArray<T> Members
        public int Count
        {
            get { return count; }
        }

        int IInternalCachedList<T>.Count
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CachedListEnumerator<T>(this);
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

        public IEnumerator<T> GetEnumerator()
        {
            return new CachedListEnumerator<T>(this);
        }
        #endregion
    }
}