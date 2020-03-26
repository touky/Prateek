namespace Assets.Prateek.ToConvert.CachedArray
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public struct StaticArray20<T> : IRangeList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyList<T>, ICollection, IList
    {
        #region Static and Constants
        private const int SIZE = 20;
        #endregion

        #region Fields
        private T value00;
        private T value01;
        private T value02;
        private T value03;
        private T value04;
        private T value05;
        private T value06;
        private T value07;
        private T value08;
        private T value09;
        private T value10;
        private T value11;
        private T value12;
        private T value13;
        private T value14;
        private T value15;
        private T value16;
        private T value17;
        private T value18;
        private T value19;

        private int count;
        #endregion

        #region Properties
        public int MaxSize
        {
            get { return SIZE; }
        }
        #endregion

        #region Class Methods
        #region Static values integration (10)
        private T InternalGetSet(bool get, int index, T value = default)
        {
            Debug.Assert(index >= 0 && index < count);

            switch (index)
            {
                case 0:  { return get ? value00 : value00 = value; }
                case 1:  { return get ? value01 : value01 = value; }
                case 2:  { return get ? value02 : value02 = value; }
                case 3:  { return get ? value03 : value03 = value; }
                case 4:  { return get ? value04 : value04 = value; }
                case 5:  { return get ? value05 : value05 = value; }
                case 6:  { return get ? value06 : value06 = value; }
                case 7:  { return get ? value07 : value07 = value; }
                case 8:  { return get ? value08 : value08 = value; }
                case 9:  { return get ? value09 : value09 = value; }
                case 10: { return get ? value10 : value10 = value; }
                case 11: { return get ? value11 : value11 = value; }
                case 12: { return get ? value12 : value12 = value; }
                case 13: { return get ? value13 : value13 = value; }
                case 14: { return get ? value14 : value14 = value; }
                case 15: { return get ? value15 : value15 = value; }
                case 16: { return get ? value16 : value16 = value; }
                case 17: { return get ? value17 : value17 = value; }
                case 18: { return get ? value18 : value18 = value; }
                case 19: { return get ? value19 : value19 = value; }
            }

            return default;
        }
        #endregion
        #endregion

        #region IList
        public int Count
        {
            get { return count; }
        }

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
            get { return InternalGet(index); }
            set
            {
                Debug.Assert(value is T);

                InternalSet(index, (T) value);
            }
        }

        public T this[int index]
        {
            get { return InternalGet(index); }
            set { InternalSet(index, value); }
        }

        public int Add(object value)
        {
            Debug.Assert(value is T);

            return InternalAdd((T) value);
        }

        public void Clear()
        {
            InternalClear();
        }

        public bool Contains(object value)
        {
            Debug.Assert(value is T);

            return InternalIndexOf((T) value) != Consts.INDEX_NONE;
        }

        public int IndexOf(object value)
        {
            Debug.Assert(value is T);

            return InternalIndexOf((T) value);
        }

        public void Insert(int index, object value)
        {
            Debug.Assert(value is T);

            InternalInsert(index, (T) value);
        }

        public void Remove(object value)
        {
            Debug.Assert(value is T);

            InternalRemove((T) value);
        }

        public void RemoveAt(int index)
        {
            InternalRemoveAt(index);
        }

        public void CopyTo(Array array, int index)
        {
            Debug.Assert(array.Length - index < count);

            for (var i = 0; i < count; i++)
            {
                array.SetValue(InternalGet(i), index + i);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new StaticArrayEnumerator<T>(this);
        }
        #endregion

        #region IList<T>
        public int IndexOf(T item)
        {
            return InternalIndexOf(item);
        }

        public void Insert(int index, T item)
        {
            InternalInsert(index, item);
        }

        public void Add(T item)
        {
            InternalAdd(item);
        }

        public void AddRange(T[] items)
        {
            Debug.Assert(items != null);

            foreach (var item in items)
            {
                InternalAdd(item);
            }
        }

        public bool Contains(T item)
        {
            return InternalIndexOf(item) != Consts.INDEX_NONE;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Debug.Assert(array.Length - arrayIndex < count);

            for (var i = 0; i < count; i++)
            {
                array[arrayIndex + i] = InternalGet(i);
            }
        }

        public bool Remove(T item)
        {
            return InternalRemove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new StaticArrayEnumerator<T>(this);
        }
        #endregion

        #region Internal manipulation
        private int InternalAdd(T value)
        {
            Debug.Assert(count < SIZE);

            InternalSet(count++, value);
            return count - 1;
        }

        private bool InternalRemove(T value)
        {
            var index = InternalIndexOf(value);
            if (index == Consts.INDEX_NONE)
            {
                return false;
            }

            InternalRemoveAt(index);
            return true;
        }

        private void InternalRemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < count);
            Debug.Assert(count > 0);

            for (var i = index; i < count; i++)
            {
                InternalSet(i, InternalGet(i + 1));
            }

            count--;
        }

        private void InternalInsert(int index, T value)
        {
            Debug.Assert(index >= 0 && index < count);
            Debug.Assert(count < SIZE);

            count++;
            for (var i = count - 1; i > index; i--)
            {
                InternalSet(i, InternalGet(i - 1));
            }

            InternalSet(index, value);
        }

        private void InternalClear()
        {
            count = 0;
            for (var i = 0; i < SIZE; i++)
            {
                InternalGetSet(false, i);
            }
        }

        public int InternalIndexOf(T value)
        {
            Debug.Assert(value is T);

            for (var i = 0; i < count; i++)
            {
                if (value.Equals(InternalGetSet(true, i)))
                {
                    return i;
                }
            }

            return Consts.INDEX_NONE;
        }

        private T InternalGet(int index)
        {
            Debug.Assert(index >= 0 && index < count);

            return InternalGetSet(true, index);
        }

        private T InternalSet(int index, T value)
        {
            Debug.Assert(index >= 0 && index < count);

            return InternalGetSet(false, index, value);
        }
        #endregion
    }
}
