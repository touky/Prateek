namespace Prateek.Core.Code.CachedArray {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Core.Code.Consts;

    ///------------------------------------------------------------------------
    internal struct CachedArrayEnumerator<T> : IEnumerator, IEnumerator<T>
    {
        #region Fields
        private int cursor;

        public ICachedArray<T> array;
        #endregion

        #region Constructors
        public CachedArrayEnumerator(ICachedArray<T> array)
        {
            this.array = array;
            this.cursor = Const.CURSOR_RESET;
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
            this.cursor = Const.CURSOR_RESET;
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