namespace Mayfair.Core.Code.Utils.Types
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal struct StaticArrayEnumerator<T> : IEnumerator, IEnumerator<T>
    {
        #region Fields
        private int cursor;

        public IReadOnlyList<T> array;
        #endregion

        #region Constructors
        public StaticArrayEnumerator(StaticArray5<T> array)
        {
            this.array = array;
            this.cursor = -1;
        }

        public StaticArrayEnumerator(StaticArray10<T> list)
        {
            this.array = list;
            this.cursor = -1;
        }

        public StaticArrayEnumerator(StaticArray20<T> list)
        {
            this.array = list;
            this.cursor = -1;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            this.cursor++;
            return this.cursor < this.array.Count;
        }

        public void Reset()
        {
            this.cursor = -1;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region IEnumerator<T> Members
        public void Dispose() { }

        public T Current
        {
            get
            {
                try
                {
                    return this.array[this.cursor];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        #endregion
    }
}
