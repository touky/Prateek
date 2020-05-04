namespace Mayfair.Core.Code.Utils.Types
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
            Debug.Assert(index >= 0 && index < this.count);

            switch (index)
            {
                case 0: { return get ? this.value00 : this.value00 = value; }
                case 1: { return get ? this.value01 : this.value01 = value; }
                case 2: { return get ? this.value02 : this.value02 = value; }
                case 3: { return get ? this.value03 : this.value03 = value; }
                case 4: { return get ? this.value04 : this.value04 = value; }
                case 5: { return get ? this.value05 : this.value05 = value; }
                case 6: { return get ? this.value06 : this.value06 = value; }
                case 7: { return get ? this.value07 : this.value07 = value; }
                case 8: { return get ? this.value08 : this.value08 = value; }
                case 9: { return get ? this.value09 : this.value09 = value; }
                case 10: { return get ? this.value10 : this.value10 = value; }
                case 11: { return get ? this.value11 : this.value11 = value; }
                case 12: { return get ? this.value12 : this.value12 = value; }
                case 13: { return get ? this.value13 : this.value13 = value; }
                case 14: { return get ? this.value14 : this.value14 = value; }
                case 15: { return get ? this.value15 : this.value15 = value; }
                case 16: { return get ? this.value16 : this.value16 = value; }
                case 17: { return get ? this.value17 : this.value17 = value; }
                case 18: { return get ? this.value18 : this.value18 = value; }
                case 19: { return get ? this.value19 : this.value19 = value; }
            }

            return default;
        }
        #endregion
        #endregion

        #region IList
        public int Count
        {
            get { return this.count; }
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
            Debug.Assert(array.Length - index < this.count);

            for (int i = 0; i < this.count; i++)
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

            foreach (T item in items)
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
            Debug.Assert(array.Length - arrayIndex < this.count);

            for (int i = 0; i < this.count; i++)
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
            Debug.Assert(this.count < SIZE);

            InternalSet(this.count++, value);
            return this.count - 1;
        }

        private bool InternalRemove(T value)
        {
            int index = InternalIndexOf(value);
            if (index == Consts.INDEX_NONE)
            {
                return false;
            }

            InternalRemoveAt(index);
            return true;
        }

        private void InternalRemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < this.count);
            Debug.Assert(this.count > 0);

            for (int i = index; i < this.count; i++)
            {
                InternalSet(i, InternalGet(i + 1));
            }

            this.count--;
        }

        private void InternalInsert(int index, T value)
        {
            Debug.Assert(index >= 0 && index < this.count);
            Debug.Assert(this.count < SIZE);

            this.count++;
            for (int i = this.count - 1; i > index; i--)
            {
                InternalSet(i, InternalGet(i - 1));
            }

            InternalSet(index, value);
        }

        private void InternalClear()
        {
            this.count = 0;
            for (int i = 0; i < SIZE; i++)
            {
                InternalGetSet(false, i);
            }
        }

        public int InternalIndexOf(T value)
        {
            Debug.Assert(value is T);

            for (int i = 0; i < this.count; i++)
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
            Debug.Assert(index >= 0 && index < this.count);

            return InternalGetSet(true, index);
        }

        private T InternalSet(int index, T value)
        {
            Debug.Assert(index >= 0 && index < this.count);

            return InternalGetSet(false, index, value);
        }
        #endregion
    }
}
